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
        public CheckDB()
        {
            InitializeComponent();
        }

        private void CheckDB_Load(object sender, EventArgs e)
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

                    string query = "SELECT * FROM PersonData";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
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

            string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";

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
    }
}
