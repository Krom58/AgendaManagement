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
    public partial class RegistrationSummary : Form
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
            _Main.Show();
            this.Close();
        }

        private void btnCalculateAndSave_Click(object sender, EventArgs e)
        {
            // หากเลือก “ผู้ลงทะเบียนทั้งหมด” (HeaderID == 0) ให้แสดงข้อความและไม่บันทึก
            if (Convert.ToInt32(comboBox1.SelectedValue) == 0)
            {
                MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้");
                return;
            }

            UpdatePreviewTable();

            int peopleCountSelf = int.Parse(label7.Text);
            int peopleCountProxy = int.Parse(label8.Text);
            long qShareSelf = long.Parse(label9.Text, System.Globalization.NumberStyles.AllowThousands);
            long qShareProxy = long.Parse(label10.Text, System.Globalization.NumberStyles.AllowThousands);
            int peopleCountTotal = int.Parse(label13.Text);
            long qShareTotal = long.Parse(label14.Text, System.Globalization.NumberStyles.AllowThousands);

            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกวาระที่ต้องการบันทึก");
                return;
            }
            int headerID = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();

                string sqlInsert = @"
        INSERT INTO RegistrationSummary (
            MeetingDate,
            PeopleCount_Self, QShare_Self,
            PeopleCount_Proxy, QShare_Proxy,
            PeopleCount_Total, QShare_Total
        )
        VALUES (
            @MeetingDate,
            @PeopleCount_Self, @QShare_Self,
            @PeopleCount_Proxy, @QShare_Proxy,
            @PeopleCount_Total, @QShare_Total
        );";
                using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PeopleCount_Self", peopleCountSelf);
                    cmd.Parameters.AddWithValue("@QShare_Self", qShareSelf);
                    cmd.Parameters.AddWithValue("@PeopleCount_Proxy", peopleCountProxy);
                    cmd.Parameters.AddWithValue("@QShare_Proxy", qShareProxy);
                    cmd.Parameters.AddWithValue("@PeopleCount_Total", peopleCountTotal);
                    cmd.Parameters.AddWithValue("@QShare_Total", qShareTotal);

                    cmd.ExecuteNonQuery();
                }

                string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate WHERE HeaderID = @HeaderID AND IsRegistered = 'บันทึกแล้ว'";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@HeaderID", headerID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show(
                            $"วาระที่ {comboBox1.Text} ได้ลงทะเบียนไปแล้วและไม่สามารถบันทึกซ้ำได้",
                            "ไม่สามารถบันทึกซ้ำได้",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }

                string updateQuery = @"
        UPDATE HeaderTemplate
        SET peopleCountTotal = @peopleCountTotal, 
            qShareTotal = @qShareTotal, 
            PeopleCount_Self = @PeopleCount_Self,
            PeopleCount_Proxy = @PeopleCount_Proxy,
            QShare_Self = @QShare_Self,
            QShare_Proxy = @QShare_Proxy,
            IsSummaryComplete = @IsSummaryComplete,
            IsRegistered = 'บันทึกแล้ว'
        WHERE HeaderID = @HeaderID";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@peopleCountTotal", peopleCountTotal);
                    updateCmd.Parameters.AddWithValue("@qShareTotal", qShareTotal);
                    updateCmd.Parameters.AddWithValue("@PeopleCount_Self", peopleCountSelf);
                    updateCmd.Parameters.AddWithValue("@PeopleCount_Proxy", peopleCountProxy);
                    updateCmd.Parameters.AddWithValue("@QShare_Self", qShareSelf);
                    updateCmd.Parameters.AddWithValue("@QShare_Proxy", qShareProxy);
                    updateCmd.Parameters.AddWithValue("@IsSummaryComplete", true);
                    updateCmd.Parameters.AddWithValue("@HeaderID", headerID);
                    updateCmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("บันทึกข้อมูลสรุปเรียบร้อย");
            // Store the currently selected value
            var selectedValue = comboBox1.SelectedValue;

            // Refresh the form
            LoadHeaderTemplateData();
            UpdatePreviewTable();

            // Restore the selected value
            comboBox1.SelectedValue = selectedValue;
        }

        private void UpdatePreviewTable()
        {
            int peopleCountSelf, peopleCountProxy, peopleCountTotal;
            long qShareSelf, qShareProxy, qShareTotal;
            bool dataFound = LoadSummaryDataFromHeaderTemplate(out peopleCountSelf, out peopleCountProxy, out qShareSelf, out qShareProxy, out peopleCountTotal, out qShareTotal);

            if (dataFound)
            {
                label7.Text = peopleCountSelf.ToString();
                label8.Text = peopleCountProxy.ToString();
                label9.Text = qShareSelf.ToString("N0");
                label10.Text = qShareProxy.ToString("N0");
                label13.Text = peopleCountTotal.ToString();
                label14.Text = qShareTotal.ToString("N0");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();

                    int pCountSelf = 0, pCountProxy = 0;
                    long qCountSelf = 0, qCountProxy = 0, totalQShareFromPerson = 0;

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS PeopleCount_Self, SUM(CONVERT(BIGINT, ShareCount)) AS QShare_Self FROM SelfRegistration", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pCountSelf = reader["PeopleCount_Self"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Self"]);
                                qCountSelf = reader["QShare_Self"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Self"]);
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS PeopleCount_Proxy, SUM(CONVERT(BIGINT, ShareCount)) AS QShare_Proxy FROM ProxyRegistration", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pCountProxy = reader["PeopleCount_Proxy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Proxy"]);
                                qCountProxy = reader["QShare_Proxy"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Proxy"]);
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(CONVERT(BIGINT, q_share)) AS TotalQShare FROM PersonData", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalQShareFromPerson = reader["TotalQShare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TotalQShare"]);
                            }
                        }
                    }
                    int totalPeople = pCountSelf + pCountProxy;
                    long totalQShareCalculated = qCountSelf + qCountProxy;

                    qShareTotal = totalQShareCalculated;
                    peopleCountTotal = totalPeople;

                    label7.Text = pCountSelf.ToString();
                    label8.Text = pCountProxy.ToString();
                    label9.Text = qCountSelf.ToString("N0");
                    label10.Text = qCountProxy.ToString("N0");
                    label13.Text = totalPeople.ToString();
                    label14.Text = totalQShareCalculated.ToString("N0");
                }
            }

            long totalQShareGlobal = 0;
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = "SELECT SUM(CONVERT(BIGINT, q_share)) FROM PersonData";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    totalQShareGlobal = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                }
            }
            double qShareCountSelf = double.Parse(label9.Text, System.Globalization.NumberStyles.AllowThousands);
            double qShareCountProxy = double.Parse(label10.Text, System.Globalization.NumberStyles.AllowThousands);
            double qpercentSelf = totalQShareGlobal > 0 ? (double)qShareCountSelf / totalQShareGlobal * 100 : 0;
            double qpercentProxy = totalQShareGlobal > 0 ? (double)qShareCountProxy / totalQShareGlobal * 100 : 0;

            label11.Text = qpercentSelf.ToString("F2") + "%";
            label12.Text = qpercentProxy.ToString("F2") + "%";
            double percentTotal = qpercentSelf + qpercentProxy;
            label15.Text = percentTotal.ToString("F2") + "%";
            label17.Text = totalQShareGlobal.ToString("N0");

            double qShareTotalValue = double.Parse(label14.Text, System.Globalization.NumberStyles.AllowThousands);
            double qShareGlobal = double.Parse(label17.Text, System.Globalization.NumberStyles.AllowThousands);
            double percentQShare = qShareGlobal > 0 ? (qShareTotalValue / qShareGlobal) * 100 : 0;
            label18.Text = percentQShare.ToString("F2") + "%";

            double percentDifference = percentTotal - percentQShare;
            label19.Text = percentDifference.ToString("F2") + "%";
        }

        private void RegistrationSummary_Load(object sender, EventArgs e)
        {
            LoadHeaderTemplateData();
            UpdatePreviewTable();
        }

        // ปรับปรุงให้ combobox มีรายการแรกเป็น “ผู้ลงทะเบียนทั้งหมด”
        private void LoadHeaderTemplateData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("HeaderID", typeof(int));
                dt.Columns.Add("AgendaNumber", typeof(string));
                dt.Columns.Add("AgendaTitle", typeof(string));
                dt.Columns.Add("AgendaDisplay", typeof(string));

                // เพิ่มรายการ “ผู้ลงทะเบียนทั้งหมด” โดยกำหนด HeaderID เป็น 0
                DataRow dr = dt.NewRow();
                dr["HeaderID"] = 0;
                dr["AgendaNumber"] = "";
                dr["AgendaTitle"] = "ผู้ลงทะเบียนทั้งหมด";
                dr["AgendaDisplay"] = "ผู้ลงทะเบียนทั้งหมด";
                dt.Rows.Add(dr);

                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            string query = "SELECT HeaderID, AgendaNumber, AgendaTitle FROM HeaderTemplate ORDER BY HeaderID";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                DataTable dtData = new DataTable();
                                adapter.Fill(dtData);
                                foreach (DataRow row in dtData.Rows)
                                {
                                    DataRow newRow = dt.NewRow();
                                    newRow["HeaderID"] = row["HeaderID"];
                                    newRow["AgendaNumber"] = row["AgendaNumber"];
                                    newRow["AgendaTitle"] = row["AgendaTitle"];
                                    newRow["AgendaDisplay"] = "วาระที่ " + row["AgendaNumber"] + " - " + row["AgendaTitle"];
                                    dt.Rows.Add(newRow);
                                }
                            }
                        }
                        else
                        {
                            // กรณีไม่มีข้อมูลใน HeaderTemplate
                            DataRow newRow = dt.NewRow();
                            newRow["HeaderID"] = 0;
                            newRow["AgendaNumber"] = "N/A";
                            newRow["AgendaTitle"] = "ข้อมูลจาก Self และ Proxy";
                            newRow["AgendaDisplay"] = "ข้อมูลจาก Self และ Proxy";
                            dt.Rows.Add(newRow);
                        }
                    }
                }
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "AgendaDisplay";
                comboBox1.ValueMember = "HeaderID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null) return;

            UpdatePreviewTable();
            int headerID = 0;
            if (comboBox1.SelectedValue is DataRowView)
            {
                DataRowView drv = (DataRowView)comboBox1.SelectedValue;
                headerID = Convert.ToInt32(drv["HeaderID"]);
            }
            else
            {
                headerID = Convert.ToInt32(comboBox1.SelectedValue);
            }

            if (headerID == 0)
            {
                btnCalculateAndSave.BackColor = System.Drawing.Color.LightCoral; // เปลี่ยนสีปุ่มเป็นสีแดงเมื่อเลือก "ผู้ลงทะเบียนทั้งหมด"
                return;
            }

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate WHERE HeaderID = @HeaderID AND IsRegistered = 'บันทึกแล้ว'";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@HeaderID", headerID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        btnCalculateAndSave.BackColor = System.Drawing.Color.LightCoral; // เปลี่ยนสีปุ่มเป็นสีแดงเมื่อวาระได้บันทึกไปแล้ว
                    }
                    else
                    {
                        btnCalculateAndSave.BackColor = System.Drawing.Color.LightGreen; // เปลี่ยนสีปุ่มเป็นสีเขียวเมื่อสามารถบันทึกได้
                    }
                }
            }
        }

        private bool LoadSummaryDataFromHeaderTemplate(out int peopleCountSelf, out int peopleCountProxy, out long qShareSelf, out long qShareProxy, out int peopleCountTotal, out long qShareTotal)
        {
            peopleCountSelf = peopleCountProxy = peopleCountTotal = 0;
            qShareSelf = qShareProxy = qShareTotal = 0;
            bool dataFound = false;

            int headerID = 0;
            if (comboBox1.SelectedValue is DataRowView)
            {
                DataRowView drv = (DataRowView)comboBox1.SelectedValue;
                headerID = Convert.ToInt32(drv["HeaderID"]);
            }
            else
            {
                headerID = Convert.ToInt32(comboBox1.SelectedValue);
            }

            // หากเลือก “ผู้ลงทะเบียนทั้งหมด” (HeaderID = 0) ให้ดึงข้อมูลคำนวณจากตารางอื่น
            if (headerID == 0)
                return false;

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = @"
        SELECT PeopleCountTotal, qShareTotal, PeopleCount_Self, PeopleCount_Proxy, QShare_Self, QShare_Proxy
        FROM HeaderTemplate
        WHERE HeaderID = @HeaderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HeaderID", headerID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) &&
                                !reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5))
                            {
                                peopleCountTotal = reader.GetInt32(0);
                                qShareTotal = reader.GetInt64(1);
                                peopleCountSelf = reader.GetInt32(2);
                                peopleCountProxy = reader.GetInt32(3);
                                qShareSelf = reader.GetInt64(4);
                                qShareProxy = reader.GetInt64(5);
                                dataFound = true;
                            }
                        }
                    }
                }
            }
            return dataFound;
        }
    }
}
