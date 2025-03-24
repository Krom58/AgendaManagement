using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Work1
{
    public partial class RegistrationSummary: Form
    {
        private Main _Main;
        public RegistrationSummary()
        {
            InitializeComponent();
        }
        public RegistrationSummary(Main main)
        {
            InitializeComponent();
            _Main = main;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            // เรียกให้ Form1 กลับมาแสดง
            _Main.Show();

            // ปิด Form2 เพื่อกลับไปใช้งาน Form1
            this.Close();
        }

        private void btnCalculateAndSave_Click(object sender, EventArgs e)
        {
            // 1. Update the preview table to refresh the latest data on the form
            UpdatePreviewTable();

            // 2. Retrieve the updated data from the labels
            int peopleCountSelf = int.Parse(label7.Text);
            int peopleCountProxy = int.Parse(label8.Text);
            long qShareSelf = long.Parse(label9.Text, System.Globalization.NumberStyles.AllowThousands);
            long qShareProxy = long.Parse(label10.Text, System.Globalization.NumberStyles.AllowThousands);
            double percentSelf = double.Parse(label11.Text.TrimEnd('%'));
            double percentProxy = double.Parse(label12.Text.TrimEnd('%'));
            int peopleCountTotal = int.Parse(label13.Text);
            long qShareTotal = long.Parse(label14.Text, System.Globalization.NumberStyles.AllowThousands);
            double percentTotal = double.Parse(label15.Text.TrimEnd('%'));

            // 3. Get the selected headerID from ComboBox (ValueMember)
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกวาระที่ต้องการบันทึก");
                return;
            }
            int headerID = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();

                // 4. Insert the summary data into RegistrationSummary table
                string sqlInsert = @"
            INSERT INTO RegistrationSummary (
                MeetingDate,
                PeopleCount_Self, QShare_Self, Percentage_Self,
                PeopleCount_Proxy, QShare_Proxy, Percentage_Proxy,
                PeopleCount_Total, QShare_Total, Percentage_Total
            )
            VALUES (
                @MeetingDate,
                @PeopleCount_Self, @QShare_Self, @Percentage_Self,
                @PeopleCount_Proxy, @QShare_Proxy, @Percentage_Proxy,
                @PeopleCount_Total, @QShare_Total, @Percentage_Total
            );";
                using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PeopleCount_Self", peopleCountSelf);
                    cmd.Parameters.AddWithValue("@QShare_Self", qShareSelf);
                    cmd.Parameters.AddWithValue("@Percentage_Self", percentSelf);
                    cmd.Parameters.AddWithValue("@PeopleCount_Proxy", peopleCountProxy);
                    cmd.Parameters.AddWithValue("@QShare_Proxy", qShareProxy);
                    cmd.Parameters.AddWithValue("@Percentage_Proxy", percentProxy);
                    cmd.Parameters.AddWithValue("@PeopleCount_Total", peopleCountTotal);
                    cmd.Parameters.AddWithValue("@QShare_Total", qShareTotal);
                    cmd.Parameters.AddWithValue("@Percentage_Total", percentTotal);

                    cmd.ExecuteNonQuery();
                }

                // 5. Check if the selected agenda in HeaderTemplate (by HeaderID) is already registered
                string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate WHERE HeaderID = @HeaderID AND IsRegistered = 'บันทึกแล้ว'";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@HeaderID", headerID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // If already registered, ask for confirmation
                        DialogResult result = MessageBox.Show(
                            $"วาระที่ {comboBox1.Text} ได้ลงทะเบียนไปแล้ว\nคุณต้องการบันทึกซ้ำหรือไม่?",
                            "ยืนยันการบันทึกซ้ำ",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                // 6. Update the HeaderTemplate table with the summary data and mark as registered using HeaderID
                string updateQuery = @"
            UPDATE HeaderTemplate
            SET peopleCountTotal = @peopleCountTotal, 
                qShareTotal = @qShareTotal, 
                IsRegistered = 'บันทึกแล้ว'
            WHERE HeaderID = @HeaderID";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@peopleCountTotal", peopleCountTotal);
                    updateCmd.Parameters.AddWithValue("@qShareTotal", qShareTotal);
                    updateCmd.Parameters.AddWithValue("@HeaderID", headerID);
                    updateCmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("บันทึกข้อมูลสรุปเรียบร้อย");
        }
        private void UpdatePreviewTable()
        {
            // ตัวแปรสำหรับเก็บข้อมูลจาก DB
            int peopleCountSelf = 0;
            long qShareSelf = 0;
            int peopleCountProxy = 0;
            long qShareProxy = 0;
            long totalQShare = 0;  // จำนวนหุ้นทั้งหมดจาก PersonData

            // ดึงข้อมูลจากฐานข้อมูล
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();

                // 1. ดึงข้อมูลจาก SelfRegistration
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS PeopleCount_Self, SUM(CONVERT(BIGINT, ShareCount)) AS QShare_Self FROM SelfRegistration", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            peopleCountSelf = reader["PeopleCount_Self"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Self"]);
                            qShareSelf = reader["QShare_Self"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Self"]);
                        }
                    }
                }

                // 2. ดึงข้อมูลจาก ProxyRegistration
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS PeopleCount_Proxy, SUM(CONVERT(BIGINT, ShareCount)) AS QShare_Proxy FROM ProxyRegistration", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            peopleCountProxy = reader["PeopleCount_Proxy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Proxy"]);
                            qShareProxy = reader["QShare_Proxy"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Proxy"]);
                        }
                    }
                }

                // 3. ดึงข้อมูลจาก PersonData เพื่อหาจำนวนหุ้นทั้งหมด (q_share)
                using (SqlCommand cmd = new SqlCommand("SELECT SUM(CONVERT(BIGINT, q_share)) AS TotalQShare FROM PersonData", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalQShare = reader["TotalQShare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TotalQShare"]);
                        }
                    }
                }
            }

            // คำนวณรวม (รวมทั้งสิ้น)
            int peopleCountTotal = peopleCountSelf + peopleCountProxy;
            long qShareTotal = qShareSelf + qShareProxy;

            // คำนวณร้อยละโดยอิงจาก totalQShare (หาก totalQShare > 0)
            double percentSelf = totalQShare > 0 ? (double)qShareSelf / totalQShare * 100 : 0;
            double percentProxy = totalQShare > 0 ? (double)qShareProxy / totalQShare * 100 : 0;
            double percentTotal = totalQShare > 0 ? (double)qShareTotal / totalQShare * 100 : 0;

            // อัปเดตข้อความใน Label
            label7.Text = peopleCountSelf.ToString();
            label8.Text = peopleCountProxy.ToString();
            label9.Text = qShareSelf.ToString("N0");
            label10.Text = qShareProxy.ToString("N0");
            label11.Text = percentSelf.ToString("F2") + "%";
            label12.Text = percentProxy.ToString("F2") + "%";
            label13.Text = peopleCountTotal.ToString();
            label14.Text = qShareTotal.ToString("N0");
            label15.Text = percentTotal.ToString("F2") + "%";
            label17.Text = totalQShare.ToString("N0");

            // คำนวณและอัปเดตข้อความใน label18 และ label19
            double percentTotalQShare = totalQShare > 0 ? (qShareTotal / (double)totalQShare) * 100 : 0;
            label18.Text = percentTotalQShare.ToString("F2") + "%";
            label19.Text = (percentTotal - percentTotalQShare).ToString("F2") + "%";
        }

        private void RegistrationSummary_Load(object sender, EventArgs e)
        {
            LoadHeaderTemplateData();
            UpdatePreviewTable();
        }
        private void LoadHeaderTemplateData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();
                    // ดึงข้อมูล HeaderID, AgendaNumber และ AgendaTitle
                    string query = "SELECT HeaderID, AgendaNumber, AgendaTitle FROM HeaderTemplate ORDER BY HeaderID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // เพิ่มคอลัมน์สำหรับแสดงใน ComboBox
                        dt.Columns.Add("AgendaDisplay", typeof(string), " 'วาระที่ ' +  AgendaNumber + ' - ' + AgendaTitle");

                        // กำหนด DataSource, DisplayMember, ValueMember ของ ComboBox
                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "AgendaDisplay";
                        comboBox1.ValueMember = "HeaderID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    }
}
