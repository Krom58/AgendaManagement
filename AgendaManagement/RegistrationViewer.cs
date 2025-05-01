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
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using Npgsql;

namespace Work1
{
    public partial class RegistrationViewer: Form
    {
        public RegistrationViewer()
        {
            InitializeComponent();
        }

        private Main _Main;
        // ตัวแปรสำหรับเก็บสถานะตารางที่โหลดล่าสุด ("SelfRegistration" หรือ "ProxyRegistration")
        private string currentTable = "";

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

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();

                // นับจำนวนแถวใน SelfRegistration
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM \"SelfRegistration\"";
                    selfCount = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // นับจำนวนแถวใน ProxyRegistration
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM \"ProxyRegistration\"";
                    proxyCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            int totalCount = selfCount + proxyCount;
            lblTotal.Text = totalCount.ToString();
            lblSelfCount.Text = selfCount.ToString();
            lblProxyCount.Text = proxyCount.ToString();
        }


        private void btnLoadSelf_Click(object sender, EventArgs e)
        {
            LoadSelfData();
            LoadCounts();  // รีเฟรชจำนวนหลังจากโหลดข้อมูล
            currentTable = "SelfRegistration";
        }

        private void btnLoadProxy_Click(object sender, EventArgs e)
        {
            LoadProxyData();
            LoadCounts();
            currentTable = "ProxyRegistration";
        }
        // ฟังก์ชันโหลดข้อมูลจากตาราง SelfRegistration และแสดงใน DataGridView
        private void LoadSelfData()
        {
            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = @"
            SELECT ""RegistrationID"", ""Identifier"", ""PeopleCount"", ""FullName"", ""ShareCount"", ""Note""
            FROM ""SelfRegistration""
            ORDER BY ""RegistrationID""";

                using (var adapter = dbcfg.Factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = conn.CreateCommand();
                    adapter.SelectCommand.CommandText = query;

                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewRegistration.DataSource = dt;

                    // ปรับ HeaderText และ Alignment ตามเดิม
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

                    foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }


        // ฟังก์ชันโหลดข้อมูลจากตาราง ProxyRegistration และแสดงใน DataGridView
        private void LoadProxyData()
        {
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = @"
                                SELECT ""RegistrationID"", ""Identifier"", ""PeopleCount"", ""FullName"", ""ShareCount"", ""Note""
                                FROM ""ProxyRegistration""
                                ORDER BY ""RegistrationID""";

                using (var adapter = dbcfg.Factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = conn.CreateCommand();
                    adapter.SelectCommand.CommandText = query;

                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewRegistration.DataSource = dt;

                    // ปรับ HeaderText และ Alignment ตามเดิม
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
                            SELECT 'มาเอง' AS Type, ""RegistrationID"", ""Identifier"", ""PeopleCount"", CONCAT(n_title, n_first, ' ', n_last) AS ""FullName"", ""ShareCount"", ""Note""
                            FROM ""SelfRegistration""
                            WHERE ""FullName"" LIKE '%' || @FullName || '%'
                            UNION ALL
                            SELECT 'มอบฉันทะ' AS Type, ""RegistrationID"", ""Identifier"", ""PeopleCount"", CONCAT(n_title, n_first, ' ', n_last) AS ""FullName"", ""ShareCount"", ""Note""
                            FROM ""ProxyRegistration""
                            WHERE ""FullName"" LIKE '%' || @FullName || '%'
                            ORDER BY ""FullName""";

            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            using (var conn = dbcfg.CreateConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        // Use DbParameter to add the parameter explicitly
                        var param = cmd.CreateParameter();
                        param.ParameterName = "@FullName";
                        param.Value = searchName;
                        cmd.Parameters.Add(param);

                        using (var adapter = dbcfg.Factory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;

                            var dt = new DataTable();
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

                            foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
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
        }

        private void btnexportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    // กำหนดชื่อ Worksheet และ File ตามตารางที่โหลด (SelfRegistration หรือ ProxyRegistration)
                    string wsName, fileName;
                    if (currentTable == "SelfRegistration")
                    {
                        wsName = "ผู้ลงทะเบียน มาเอง";
                        fileName = "ผู้ลงทะเบียน_มาเอง.xlsx";
                    }
                    else if (currentTable == "ProxyRegistration")
                    {
                        wsName = "ผู้ลงทะเบียน มอบฉันทะ";
                        fileName = "ผู้ลงทะเบียน_มอบฉันทะ.xlsx";
                    }
                    else
                    {
                        // กรณียังไม่ได้โหลดข้อมูล
                        MessageBox.Show("กรุณาโหลดข้อมูลก่อนส่งออก Excel");
                        return;
                    }

                    // สร้าง Worksheet ด้วยชื่อที่กำหนด
                    var ws = workbook.Worksheets.Add(wsName);

                    // เขียนส่วนหัวของ DataGridView ลงในแถวที่ 1
                    for (int i = 0; i < dataGridViewRegistration.Columns.Count; i++)
                    {
                        ws.Cell(1, i + 1).Value = dataGridViewRegistration.Columns[i].HeaderText;
                    }

                    // เขียนข้อมูลจาก DataGridView ลงใน Worksheet เริ่มที่แถวที่ 2
                    int currentRow = 2;
                    foreach (DataGridViewRow row in dataGridViewRegistration.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int j = 0; j < dataGridViewRegistration.Columns.Count; j++)
                            {
                                ws.Cell(currentRow, j + 1).Value = row.Cells[j].Value != null ? row.Cells[j].Value.ToString() : "";
                            }
                            currentRow++;
                        }
                    }

                    // เพิ่มช่องว่างระหว่างข้อมูลตารางและข้อมูล Label
                    currentRow += 2;

                    // ส่งออกข้อมูลจาก Label
                    ws.Cell(currentRow, 1).Value = "จำนวนผู้ลงทะเบียนทั้งหมด:";
                    ws.Cell(currentRow, 3).Value = lblTotal.Text;
                    currentRow++;

                    ws.Cell(currentRow, 1).Value = "จำนวนมาเอง:";
                    ws.Cell(currentRow, 3).Value = lblSelfCount.Text;
                    currentRow++;

                    ws.Cell(currentRow, 1).Value = "จำนวนมอบฉันทะ:";
                    ws.Cell(currentRow, 3).Value = lblProxyCount.Text;

                    // ตั้งค่าสไตล์ให้กับช่วงข้อมูลที่ใช้ (font, ขนาด, ชื่อฟอนต์)
                    var range = ws.RangeUsed();
                    range.Style.Font.FontSize = 24;
                    range.Style.Font.FontName = "Angsana New";
                    ws.Columns().AdjustToContents();

                    // บันทึกไฟล์ Excel ไปยังโฟลเดอร์ Documents
                    string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
                    workbook.SaveAs(filePath);

                    MessageBox.Show("Exported successfully to " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    }
}
