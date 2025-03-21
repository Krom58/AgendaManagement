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
    public partial class RegistrationViewer: Form
    {
        public RegistrationViewer()
        {
            InitializeComponent();
        }

        private Main _Main;

        public RegistrationViewer(Main main)
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

        private void RegistrationViewer_Load(object sender, EventArgs e)
        {
            LoadCounts();
        }
        private void LoadCounts()
        {
            int selfCount = 0, proxyCount = 0;
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                // นับจำนวนแถวใน SelfRegistration
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SelfRegistration", conn))
                {
                    selfCount = (int)cmd.ExecuteScalar();
                }
                // นับจำนวนแถวใน ProxyRegistration
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ProxyRegistration", conn))
                {
                    proxyCount = (int)cmd.ExecuteScalar();
                }
            }
            int totalCount = selfCount + proxyCount;
            lblTotal.Text = "" + totalCount.ToString();
            lblSelfCount.Text = "" + selfCount.ToString();
            lblProxyCount.Text = "" + proxyCount.ToString();
        }

        private void btnLoadSelf_Click(object sender, EventArgs e)
        {
            LoadSelfData();
            LoadCounts();  // รีเฟรชจำนวนหลังจากโหลดข้อมูล
        }

        private void btnLoadProxy_Click(object sender, EventArgs e)
        {
            LoadProxyData();
            LoadCounts();
        }
        // ฟังก์ชันโหลดข้อมูลจากตาราง SelfRegistration และแสดงใน DataGridView
        private void LoadSelfData()
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = "SELECT RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note FROM SelfRegistration ORDER BY RegistrationID";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewRegistration.DataSource = dt;
                    dataGridViewRegistration.Columns["RegistrationID"].HeaderText = "รหัสลงทะเบียน";
                    dataGridViewRegistration.Columns["Identifier"].HeaderText = "หมายเลข";
                    dataGridViewRegistration.Columns["PeopleCount"].HeaderText = "";
                    dataGridViewRegistration.Columns["FullName"].HeaderText = "ชื่อ - สกุล";
                    dataGridViewRegistration.Columns["ShareCount"].HeaderText = "จำนวนหุ้น";
                    dataGridViewRegistration.Columns["Note"].HeaderText = "หมายเหตุ";
                    dataGridViewRegistration.Columns["RegistrationID"].Visible = false;
                    dataGridViewRegistration.Columns["Identifier"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewRegistration.Columns["ShareCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewRegistration.Columns["PeopleCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewRegistration.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // จัดตำแหน่งข้อความใน Header ให้กึ่งกลาง
                    foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
        }

        // ฟังก์ชันโหลดข้อมูลจากตาราง ProxyRegistration และแสดงใน DataGridView
        private void LoadProxyData()
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = "SELECT RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note FROM ProxyRegistration ORDER BY RegistrationID";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewRegistration.DataSource = dt;
                    dataGridViewRegistration.Columns["RegistrationID"].HeaderText = "รหัสลงทะเบียน";
                    dataGridViewRegistration.Columns["Identifier"].HeaderText = "หมายเลข";
                    dataGridViewRegistration.Columns["PeopleCount"].HeaderText = "";
                    dataGridViewRegistration.Columns["FullName"].HeaderText = "ชื่อ - สกุล";
                    dataGridViewRegistration.Columns["ShareCount"].HeaderText = "จำนวนหุ้น";
                    dataGridViewRegistration.Columns["Note"].HeaderText = "หมายเหตุ (ผู้รับมอบฉันทะ)";
                    dataGridViewRegistration.Columns["RegistrationID"].Visible = false;
                    dataGridViewRegistration.Columns["Identifier"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewRegistration.Columns["ShareCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewRegistration.Columns["Note"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewRegistration.Columns["PeopleCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewRegistration.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // จัดตำแหน่งข้อความใน Header ให้กึ่งกลาง
                    foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchName = txtSearchFullName.Text.Trim();
            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("กรุณากรอกชื่อที่ต้องการค้นหา");
                return;
            }

            // Query ดึงข้อมูลจากทั้ง SelfRegistration และ ProxyRegistration ด้วย UNION ALL
            string query = @"
        SELECT 'มาเอง' AS Type, RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note
        FROM SelfRegistration
        WHERE FullName LIKE '%' + @FullName + '%'
        UNION ALL
        SELECT 'มอบฉันทะ' AS Type, RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note
        FROM ProxyRegistration
        WHERE FullName LIKE '%' + @FullName + '%'
        ORDER BY FullName";

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", searchName);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewRegistration.DataSource = dt;
                        dataGridViewRegistration.Columns["RegistrationID"].HeaderText = "รหัสลงทะเบียน";
                        dataGridViewRegistration.Columns["Identifier"].HeaderText = "หมายเลข";
                        dataGridViewRegistration.Columns["PeopleCount"].HeaderText = "";
                        dataGridViewRegistration.Columns["FullName"].HeaderText = "ชื่อ - สกุล";
                        dataGridViewRegistration.Columns["ShareCount"].HeaderText = "จำนวนหุ้น";
                        dataGridViewRegistration.Columns["Note"].HeaderText = "หมายเหตุ";
                        dataGridViewRegistration.Columns["RegistrationID"].Visible = false;
                        dataGridViewRegistration.Columns["ShareCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        // จัดตำแหน่งข้อความใน Header ให้กึ่งกลาง
                        foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
                        {
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
}
