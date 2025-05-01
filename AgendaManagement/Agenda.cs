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
using System.IO;

namespace Work1
{
    public partial class Agenda: Form
    {
        public Agenda()
        {
            InitializeComponent();
            this.Load += Agenda_Load;
        }

        private Main _Main;

        // Constructor ที่รับ Form1 เข้ามา
        public Agenda(Main main)
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

        private void btnSaveHeaderTemplate_Click(object sender, EventArgs e)
        {
            // รับค่า
            string meetingNumber = txtMeetingNumber.Text.Trim();
            string agendaNumber = txtAgendaNumber.Text.Trim();
            string agendaTitle = txtAgendaTitle.Text.Trim();

            // ตรวจสอบข้อมูล
            if (string.IsNullOrEmpty(meetingNumber))
            {
                MessageBox.Show("กรุณากรอก 'ครั้งที่'");
                return;
            }
            if (string.IsNullOrEmpty(agendaNumber))
            {
                MessageBox.Show("กรุณากรอก 'วาระที่'");
                return;
            }
            if (string.IsNullOrEmpty(agendaTitle))
            {
                MessageBox.Show("กรุณากรอก 'หัวข้อวาระ'");
                return;
            }

            if (IsAgendaNumberDuplicate(agendaNumber))
            {
                MessageBox.Show("วาระที่นี้มีอยู่แล้ว กรุณาใช้หมายเลขอื่น");
                return;
            }

            // ตรวจสอบสถานะของ CheckBox
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("ไม่สามารถเลือกประเภทวาระได้มากกว่า 1 ประเภท", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int agendaType = 0;
            if (checkBox1.Checked)
            {
                agendaType = 1;
            }
            else if (checkBox2.Checked)
            {
                agendaType = 2;
            }
            else
            {
                MessageBox.Show("กรุณาเลือกประเภทวาระ (Agenda Type)");
                return;
            }

            try
            {
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO ""HeaderTemplate"" (""MeetingNumber"", ""AgendaNumber"", ""AgendaTitle"", ""AgendaType"")
                             VALUES (@MeetingNumber, @AgendaNumber, @AgendaTitle, @AgendaType)";
                    using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        cmd.CommandText = query;

                        var param1 = cmd.CreateParameter();
                        param1.ParameterName = "@MeetingNumber";
                        param1.Value = meetingNumber;
                        cmd.Parameters.Add(param1);

                        var param2 = cmd.CreateParameter();
                        param2.ParameterName = "@AgendaNumber";
                        param2.Value = agendaNumber;
                        cmd.Parameters.Add(param2);

                        var param3 = cmd.CreateParameter();
                        param3.ParameterName = "@AgendaTitle";
                        param3.Value = agendaTitle;
                        cmd.Parameters.Add(param3);

                        var param4 = cmd.CreateParameter();
                        param4.ParameterName = "@AgendaType";
                        param4.Value = agendaType;
                        cmd.Parameters.Add(param4);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("บันทึก Template เรียบร้อยแล้ว");

                txtMeetingNumber.Text = string.Empty;
                txtAgendaNumber.Text = string.Empty;
                txtAgendaTitle.Text = string.Empty;

                LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
        // เมธอดตรวจสอบว่า AgendaNumber ซ้ำหรือไม่
        private bool IsAgendaNumberDuplicate(string agendaNumber)
        {
            bool duplicate = false;
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM \"HeaderTemplate\" WHERE \"AgendaNumber\" = @AgendaNumber";
                using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = query;

                    var param = cmd.CreateParameter();
                    param.ParameterName = "@AgendaNumber";
                    param.Value = agendaNumber;
                    cmd.Parameters.Add(param);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                }
            }
            return duplicate;
        }
        private void Agenda_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }
        private void LoadDataFromDatabase()
        {
            try
            {
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    string query = "SELECT \"HeaderID\", \"MeetingNumber\", \"AgendaNumber\", \"AgendaTitle\" FROM \"HeaderTemplate\" ORDER BY \"HeaderID\"";
                    using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        cmd.CommandText = query;

                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dataGridViewTemplate.DataSource = dt;
                            dataGridViewTemplate.Columns["MeetingNumber"].HeaderText = "ครั้งที่";
                            dataGridViewTemplate.Columns["AgendaNumber"].HeaderText = "วาระที่";
                            dataGridViewTemplate.Columns["AgendaTitle"].HeaderText = "หัวข้อ";
                            dataGridViewTemplate.Columns["HeaderId"].Visible = false;
                            dataGridViewTemplate.Columns["MeetingNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewTemplate.Columns["AgendaNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewTemplate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        }
                    }
                }
                foreach (DataGridViewColumn col in dataGridViewTemplate.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามีแถวที่ถูกเลือกอยู่หรือไม่
            if (dataGridViewTemplate.SelectedRows.Count > 0)
            {
                // สมมุติว่าใช้แถวแรกที่ถูกเลือก
                int headerID = Convert.ToInt32(dataGridViewTemplate.SelectedRows[0].Cells["HeaderID"].Value);

                DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ที่จะลบข้อมูลนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteRecord(headerID);
                    LoadDataFromDatabase(); // รีเฟรช DataGridView หลังลบ
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกแถวที่จะลบ");
            }
        }
        private void DeleteRecord(int headerID)
        {
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = "DELETE FROM \"HeaderTemplate\" WHERE \"HeaderID\" = @HeaderID";
                using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = query;

                    var param = cmd.CreateParameter();
                    param.ParameterName = "@HeaderID";
                    param.Value = headerID;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
