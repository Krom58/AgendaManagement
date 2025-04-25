using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;
using System.Data.SqlClient;

namespace Work1
{
    public partial class ImportExcel: Form
    {
        public ImportExcel()
        {
            InitializeComponent();
        }

        private Main _Main;

        // Constructor ที่รับ Form1 เข้ามา
        public ImportExcel(Main main)
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

        private void SelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(filePath);

                    // Set the license context for EPPlus
                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    // อ่านไฟล์ Excel โดยใช้ EPPlus
                    using (ExcelPackage package = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // อ่าน Worksheet แรก
                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        DataTable dt = new DataTable();

                        // สมมติว่าบรรทัดแรกเป็น header
                        for (int col = 1; col <= colCount; col++)
                        {
                            dt.Columns.Add(worksheet.Cells[1, col].Text);
                        }

                        // อ่านข้อมูลจากแถวที่ 2 เป็นต้นไป
                        for (int row = 2; row <= rowCount; row++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int col = 1; col <= colCount; col++)
                            {
                                dr[col - 1] = worksheet.Cells[row, col].Text;
                            }
                            dt.Rows.Add(dr);
                        }

                        // แสดงข้อมูลลงใน DataGridView
                        dataGridView.DataSource = dt;
                    }
                }
            }
        }

        private void ImportToDatabase_Click(object sender, EventArgs e)
        {
            if (dataGridView.DataSource == null)
            {
                MessageBox.Show("กรุณาเลือกไฟล์ Excel ก่อน");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM PersonData";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.ExecuteNonQuery();
                    }

                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        // ข้ามแถวที่เป็นแถวใหม่ (New Row) ของ DataGridView
                        if (row.IsNewRow) continue;

                        // ดึงค่าจากแต่ละคอลัมน์ (ปรับชื่อคอลัมน์ให้ตรงกับไฟล์ Excel)
                        string n_title = row.Cells["n_title"].Value?.ToString();
                        string n_first = row.Cells["n_first"].Value?.ToString();
                        string n_last = row.Cells["n_last"].Value?.ToString();
                        string q_share = row.Cells["q_share"].Value?.ToString();
                        string i_ref = row.Cells["i_ref"].Value?.ToString();

                        // ถ้า i_tax หรือ q_share เป็นค่าว่าง ให้กำหนดเป็นค่าว่าง
                        n_title = string.IsNullOrEmpty(n_title) ? string.Empty : n_title;
                        n_first = string.IsNullOrEmpty(n_first) ? string.Empty : n_first;
                        n_last = string.IsNullOrEmpty(n_last) ? string.Empty : n_last;
                        q_share = string.IsNullOrEmpty(q_share) ? string.Empty : q_share;
                        i_ref = string.IsNullOrEmpty(i_ref) ? string.Empty : i_ref;

                        // สร้างคำสั่ง SQL สำหรับแทรกข้อมูล
                        string query = "INSERT INTO PersonData (n_title, n_first, n_last, q_share, i_ref) VALUES (@n_title, @n_first, @n_last, @q_share, @i_ref)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@n_title", n_title);
                            cmd.Parameters.AddWithValue("@n_first", n_first);
                            cmd.Parameters.AddWithValue("@n_last", n_last);
                            cmd.Parameters.AddWithValue("@q_share", q_share);
                            cmd.Parameters.AddWithValue("@i_ref", i_ref);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("นำเข้าข้อมูลเรียบร้อยแล้ว");
                }
            }
            catch (Exception ex)
            {
                // จัดการ Exception โดยแสดงข้อความผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    }
}
