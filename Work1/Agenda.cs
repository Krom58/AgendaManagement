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
    public partial class Agenda: Form
    {
        private string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";
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

            // ตัวอย่างการสร้าง FixedContent แบบคงที่ (ส่วนนี้เว้นไว้ให้ผู้พิมพ์แก้ไขได้เองเวลาปริ้น)
            string fixedContent = @"
    []เห็นด้วย        []ไม่เห็นด้วย      []งดออกเสียง
      (Approved)     (Disapproved)  (Abstained)
ลงชื่อ __________________ ผู้ถือหุ้น";

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

            // ตรวจสอบว่า AgendaNumber ไม่เกิน 9 และไม่ซ้ำกันในฐานข้อมูล
            if (IsAgendaNumberDuplicate(agendaNumber))
            {
                MessageBox.Show("วาระที่นี้มีอยู่แล้ว กรุณาใช้หมายเลขอื่น");
                return;
            }
            if (GetTotalAgendaCount() >= 9)
            {
                MessageBox.Show("สามารถบันทึกได้สูงสุด 9 วาระแล้ว");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO HeaderTemplate (MeetingNumber, AgendaNumber, AgendaTitle, FixedContent)
                                     VALUES (@MeetingNumber, @AgendaNumber, @AgendaTitle, @FixedContent)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MeetingNumber", meetingNumber);
                        cmd.Parameters.AddWithValue("@AgendaNumber", agendaNumber);
                        cmd.Parameters.AddWithValue("@AgendaTitle", agendaTitle);
                        cmd.Parameters.AddWithValue("@FixedContent", fixedContent);

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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM HeaderTemplate WHERE AgendaNumber = @AgendaNumber";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AgendaNumber", agendaNumber);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                }
            }
            return duplicate;
        }

        // เมธอดตรวจสอบจำนวนวาระที่บันทึกไว้ในฐานข้อมูล
        private int GetTotalAgendaCount()
        {
            int total = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM HeaderTemplate";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    total = (int)cmd.ExecuteScalar();
                }
            }
            return total;
        }

        private void Agenda_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }
        private void LoadDataFromDatabase()
        {
            string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT HeaderID, MeetingNumber, AgendaNumber, AgendaTitle FROM HeaderTemplate ORDER BY HeaderID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewTemplate.DataSource = dt;
                    }
                }
                dataGridViewTemplate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM HeaderTemplate WHERE HeaderID = @HeaderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HeaderID", headerID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
