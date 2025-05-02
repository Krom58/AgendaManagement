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
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM PersonData";
                    using (var deleteCmd = conn.CreateCommand())
                    {
                        deleteCmd.CommandText = deleteQuery;
                        deleteCmd.ExecuteNonQuery();
                    }

                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string n_title = row.Cells["n_title"].Value?.ToString() ?? string.Empty;
                        string n_first = row.Cells["n_first"].Value?.ToString() ?? string.Empty;
                        string n_last = row.Cells["n_last"].Value?.ToString() ?? string.Empty;
                        string q_share = row.Cells["q_share"].Value?.ToString() ?? string.Empty;
                        string i_ref = row.Cells["i_ref"].Value?.ToString() ?? string.Empty;

                        string query = "INSERT INTO PersonData (n_title, n_first, n_last, q_share, i_ref) VALUES (@n_title, @n_first, @n_last, @q_share, @i_ref)";
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;

                            var param1 = cmd.CreateParameter();
                            param1.ParameterName = "@n_title";
                            param1.Value = n_title;
                            cmd.Parameters.Add(param1);

                            var param2 = cmd.CreateParameter();
                            param2.ParameterName = "@n_first";
                            param2.Value = n_first;
                            cmd.Parameters.Add(param2);

                            var param3 = cmd.CreateParameter();
                            param3.ParameterName = "@n_last";
                            param3.Value = n_last;
                            cmd.Parameters.Add(param3);

                            var param4 = cmd.CreateParameter();
                            param4.ParameterName = "@q_share";
                            param4.Value = q_share;
                            cmd.Parameters.Add(param4);

                            var param5 = cmd.CreateParameter();
                            param5.ParameterName = "@i_ref";
                            param5.Value = i_ref;
                            cmd.Parameters.Add(param5);

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
