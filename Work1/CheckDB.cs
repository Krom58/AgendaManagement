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
    public partial class CheckDB: Form
    {
        private string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";
        public CheckDB()
        {
            InitializeComponent();
        }
        private void CheckDB_Load(object sender, EventArgs e)
        {

        }

        private Main _Main;

        // Constructor ที่รับ Form1 เข้ามา
        public CheckDB(Main main)
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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string n_first = textBox1.Text.Trim();
            string n_last = textBox2.Text.Trim();
            string i_tax = textBox3.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    StringBuilder queryBuilder = new StringBuilder("SELECT * FROM PersonData WHERE 1=1");

                    if (!string.IsNullOrEmpty(n_first))
                    {
                        queryBuilder.Append(" AND n_first LIKE @n_first");
                    }
                    if (!string.IsNullOrEmpty(n_last))
                    {
                        queryBuilder.Append(" AND n_last LIKE @n_last");
                    }
                    if (!string.IsNullOrEmpty(i_tax))
                    {
                        queryBuilder.Append(" AND i_tax LIKE @i_tax");
                    }

                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), conn))
                    {
                        if (!string.IsNullOrEmpty(n_first))
                        {
                            cmd.Parameters.AddWithValue("@n_first", "%" + n_first + "%");
                        }
                        if (!string.IsNullOrEmpty(n_last))
                        {
                            cmd.Parameters.AddWithValue("@n_last", "%" + n_last + "%");
                        }
                        if (!string.IsNullOrEmpty(i_tax))
                        {
                            cmd.Parameters.AddWithValue("@i_tax", "%" + i_tax + "%");
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            // ตรวจสอบว่า DataGridView มีแถวที่เลือกหรือไม่
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการลงทะเบียน");
                return;
            }

            // สมมุติว่าเลือกแถวแรกที่ถูกเลือก
            int Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            string currentRegStatus = dataGridView.SelectedRows[0].Cells["RegStatus"].Value.ToString();

            // ตรวจสอบว่ามีสถานะ "ลงทะเบียนแล้ว" หรือไม่
            if (currentRegStatus == "ลงทะเบียนแล้ว")
            {
                MessageBox.Show("แถวนี้ถูกลงทะเบียนแล้ว ไม่สามารถลงทะเบียนซ้ำได้");
                return;
            }

            // เปิด FormRegistrationChoice เพื่อเลือกประเภทการเข้าร่วม
            FormRegistrationChoice choiceForm = new FormRegistrationChoice();
            if (choiceForm.ShowDialog() == DialogResult.OK)
            {
                string attendChoice = choiceForm.SelectedChoice;  // "มาเอง" หรือ "ตัวแทน"

                // อัปเดทข้อมูลในฐานข้อมูล โดยอัปเดทคอลัมน์ RegStatus และ AttendType
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = @"
        UPDATE PersonData
        SET RegStatus = N'ลงทะเบียนแล้ว', AttendType = @AttendType
        WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AttendType", attendChoice);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("ลงทะเบียนเรียบร้อยแล้ว");
                LoadData(); // รีเฟรช DataGridView เพื่อแสดงข้อมูลล่าสุด

                // หลังจากลงทะเบียนแล้ว ให้เข้าสู่หน้าปริ้น
                PrintAgenda printForm = new PrintAgenda(Id);
                printForm.Show();
            }
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM PersonData ORDER BY Id";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView.DataSource = dt;
                }
            }
        }
    }
}
