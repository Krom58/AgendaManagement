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
using Microsoft.VisualBasic;

namespace Work1
{
    public partial class CheckDB: Form
    {
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
            string i_ref = textBox3.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
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
                    if (!string.IsNullOrEmpty(i_ref))
                    {
                        queryBuilder.Append(" AND i_ref LIKE @i_ref");
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
                        if (!string.IsNullOrEmpty(i_ref))
                        {
                            cmd.Parameters.AddWithValue("@i_ref", "%" + i_ref + "%");
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView.DataSource = dt;
                        dataGridView.Columns["SelfCount"].HeaderText = "มาเอง";
                        dataGridView.Columns["ProxyCount"].HeaderText = "มอบฉันทะ";
                        dataGridView.Columns["Note"].HeaderText = "หมายเหตุ";
                        dataGridView.Columns["q_share"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView.Columns["i_ref"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns["SelfCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns["ProxyCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        // ซ่อนคอลัมน์ Id
                        dataGridView.Columns["Id"].Visible = false;
                        dataGridView.Columns["RegStatus"].Visible = false;
                        foreach (DataGridViewColumn col in dataGridView.Columns)
                        {
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

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
                MessageBox.Show("ชื่อนี้ลงทะเบียนแล้ว ไม่สามารถลงทะเบียนซ้ำได้");
                return;
            }

            // เปิด FormRegistrationChoice เพื่อเลือกประเภทการเข้าร่วม
            FormRegistrationChoice choiceForm = new FormRegistrationChoice();
            if (choiceForm.ShowDialog() == DialogResult.OK)
            {
                string attendChoice = choiceForm.SelectedChoice;  // "มาเอง" หรือ "มอบฉันทะ"
                MultiInputForm multiInputForm = new MultiInputForm();
                if (multiInputForm.ShowDialog() == DialogResult.OK)
                {
                    string peopleCountInput = multiInputForm.PeopleCountInput;
                    string noteInput = multiInputForm.NoteInput;
                    // อัปเดทข้อมูลในตาราง PersonData โดยเซ็ตค่าในคอลัมน์ที่เกี่ยวข้องเป็น 1
                    using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                    {
                        conn.Open();
                        string updateQuery = @"
                    UPDATE PersonData
                    SET RegStatus = N'ลงทะเบียนแล้ว',
                        SelfCount = CASE WHEN @AttendType = N'มาเอง' THEN 1 ELSE SelfCount END,
                        Proxycount = CASE WHEN @AttendType = N'มอบฉันทะ' THEN 1 ELSE Proxycount END,
                        Note = @Note
                    WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@AttendType", attendChoice);
                            cmd.Parameters.AddWithValue("@Note", noteInput);
                            cmd.Parameters.AddWithValue("@Id", Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("ลงทะเบียนเรียบร้อยแล้ว");
                    LoadData(); // รีเฟรช DataGridView

                    // ดำเนินการแทรกข้อมูลลงในตารางลงทะเบียนเพิ่มเติม (ถ้ามี)
                    InsertRegistrationRecord(Id, attendChoice, noteInput, peopleCountInput);

                    // หลังจากลงทะเบียนแล้ว ให้เข้าสู่หน้าปริ้น
                    PrintAgenda printForm = new PrintAgenda(Id, attendChoice);
                    printForm.Show();
                }
            }
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM PersonData ORDER BY Id";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView.DataSource = dt;

                    dataGridView.Columns["SelfCount"].HeaderText = "มาเอง";
                    dataGridView.Columns["ProxyCount"].HeaderText = "มอบฉันทะ";
                    dataGridView.Columns["Note"].HeaderText = "หมายเหตุ";
                    dataGridView.Columns["q_share"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView.Columns["i_ref"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView.Columns["SelfCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView.Columns["ProxyCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    // ซ่อนคอลัมน์ Id
                    dataGridView.Columns["Id"].Visible = false;
                    dataGridView.Columns["RegStatus"].Visible = false;
                    foreach (DataGridViewColumn col in dataGridView.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
        }
        private void InsertRegistrationRecord(int Id, string attendChoice, string noteInput, string peopleCountInput)
        {
            // ดึงข้อมูลของบุคคลจาก PersonData (FullName และ q_share)
            string fullName = "";
            string shareCount = "";

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string queryPerson = "SELECT CONCAT(n_first, ' ', n_last) AS FullName, q_share, Note, Id FROM PersonData WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(queryPerson, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fullName = reader["FullName"].ToString();
                            shareCount = reader["q_share"].ToString();
                            noteInput = reader["Note"].ToString();
                        }
                    }
                }

                // กำหนดค่า PeopleCount เป็น 1 สำหรับทั้งสองกรณี
                int peopleCount;
                if (!int.TryParse(peopleCountInput, out peopleCount))
                {
                    peopleCount = 1; // กรณีไม่สามารถแปลงได้ ให้ฟิกเป็น 1
                }

                if (attendChoice == "มาเอง")
                {
                    // สร้าง Identifier สำหรับตาราง SelfRegistration (รูปแบบ A1, A2, ...)
                    string identifier = GetNextIdentifier(conn, "SelfRegistration", "B");

                    string insertQuery = @"
                INSERT INTO SelfRegistration (Identifier, PeopleCount, FullName, ShareCount, Note, Id)
                VALUES (@Identifier, @PeopleCount, @FullName, @ShareCount, @Note, @Id)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Identifier", identifier);
                        cmd.Parameters.AddWithValue("@PeopleCount", peopleCount);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@ShareCount", shareCount);
                        cmd.Parameters.AddWithValue("@Note", noteInput);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (attendChoice == "มอบฉันทะ")
                {
                    // สร้าง Identifier สำหรับตาราง ProxyRegistration (รูปแบบ B1, B2, ...)
                    string identifier = GetNextIdentifier(conn, "ProxyRegistration", "P");

                    string insertQuery = @"
                INSERT INTO ProxyRegistration (Identifier, PeopleCount, FullName, ShareCount, Note, Id)
                VALUES (@Identifier, @PeopleCount, @FullName, @ShareCount, @Note, @Id)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Identifier", identifier);
                        cmd.Parameters.AddWithValue("@PeopleCount", peopleCount);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@ShareCount", shareCount);
                        cmd.Parameters.AddWithValue("@Note", noteInput);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private string GetNextIdentifier(SqlConnection conn, string tableName, string prefix)
        {
            // Query นับจำนวนแถวในตารางที่ระบุ
            string query = $"SELECT COUNT(*) FROM {tableName}";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                int count = (int)cmd.ExecuteScalar();
                // Identifier คือ prefix + (count+1)
                return prefix + (count + 1).ToString();
            }
        }

        private void Reprint_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า DataGridView มีแถวที่เลือกหรือไม่
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการ reprint");
                return;
            }

            // สมมุติว่าเลือกแถวแรกที่ถูกเลือก
            int Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            string currentRegStatus = dataGridView.SelectedRows[0].Cells["RegStatus"].Value.ToString();

            // ตรวจสอบว่าบุคคลนี้ลงทะเบียนแล้วหรือไม่
            if (currentRegStatus != "ลงทะเบียนแล้ว")
            {
                MessageBox.Show("บุคคลนี้ยังไม่ได้ลงทะเบียน ไม่สามารถ reprint ได้");
                return;
            }

            // ตรวจสอบค่าในคอลัมน์ SelfCount และ ProxyCount
            int selfCount = 0;
            int proxyCount = 0;
            if (dataGridView.SelectedRows[0].Cells["SelfCount"].Value != null)
                int.TryParse(dataGridView.SelectedRows[0].Cells["SelfCount"].Value.ToString(), out selfCount);
            if (dataGridView.SelectedRows[0].Cells["ProxyCount"].Value != null)
                int.TryParse(dataGridView.SelectedRows[0].Cells["ProxyCount"].Value.ToString(), out proxyCount);

            string attendChoice = "";
            if (selfCount == 1)
                attendChoice = "มาเอง";
            else if (proxyCount == 1)
                attendChoice = "มอบฉันทะ";
            else
            {
                MessageBox.Show("ไม่พบข้อมูลการลงทะเบียนที่ถูกต้อง");
                return;
            }

            // เปิดหน้าปริ้นโดยส่ง personId และ attendChoice ไปด้วย
            PrintAgenda printForm = new PrintAgenda(Id, attendChoice);
            printForm.Show();
        }
    }
}
