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
using System.Data.Common;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Work1
{
    public partial class RegistrationSummary : Form
    {
        private Main _Main;
        public RegistrationSummary()
        {
            InitializeComponent();
            // Set DropDownStyle to DropDownList to make ComboBox non-editable
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            // Set DrawMode to OwnerDrawFixed to handle custom drawing
            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += new DrawItemEventHandler(comboBox1_DrawItem);
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

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
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
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlInsert;

                    // Add parameters using DbCommand's parameter creation method
                    var param1 = cmd.CreateParameter();
                    param1.ParameterName = "@MeetingDate";
                    param1.Value = DateTime.Now;
                    cmd.Parameters.Add(param1);

                    var param2 = cmd.CreateParameter();
                    param2.ParameterName = "@PeopleCount_Self";
                    param2.Value = peopleCountSelf;
                    cmd.Parameters.Add(param2);

                    var param3 = cmd.CreateParameter();
                    param3.ParameterName = "@QShare_Self";
                    param3.Value = qShareSelf;
                    cmd.Parameters.Add(param3);

                    var param4 = cmd.CreateParameter();
                    param4.ParameterName = "@PeopleCount_Proxy";
                    param4.Value = peopleCountProxy;
                    cmd.Parameters.Add(param4);

                    var param5 = cmd.CreateParameter();
                    param5.ParameterName = "@QShare_Proxy";
                    param5.Value = qShareProxy;
                    cmd.Parameters.Add(param5);

                    var param6 = cmd.CreateParameter();
                    param6.ParameterName = "@PeopleCount_Total";
                    param6.Value = peopleCountTotal;
                    cmd.Parameters.Add(param6);

                    var param7 = cmd.CreateParameter();
                    param7.ParameterName = "@QShare_Total";
                    param7.Value = qShareTotal;
                    cmd.Parameters.Add(param7);

                    cmd.ExecuteNonQuery();
                }

                string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate WHERE HeaderID = @HeaderID AND IsRegistered = 'บันทึกแล้ว'";
                using (var checkCmd = conn.CreateCommand())
                {
                    checkCmd.CommandText = checkQuery;

                    var parameter = checkCmd.CreateParameter();
                    parameter.ParameterName = "@HeaderID";
                    parameter.Value = headerID;
                    checkCmd.Parameters.Add(parameter);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

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
                using (var updateCmd = conn.CreateCommand())
                {
                    updateCmd.CommandText = updateQuery;

                    var param1 = updateCmd.CreateParameter();
                    param1.ParameterName = "@peopleCountTotal";
                    param1.Value = peopleCountTotal;
                    updateCmd.Parameters.Add(param1);

                    var param2 = updateCmd.CreateParameter();
                    param2.ParameterName = "@qShareTotal";
                    param2.Value = qShareTotal;
                    updateCmd.Parameters.Add(param2);

                    var param3 = updateCmd.CreateParameter();
                    param3.ParameterName = "@PeopleCount_Self";
                    param3.Value = peopleCountSelf;
                    updateCmd.Parameters.Add(param3);

                    var param4 = updateCmd.CreateParameter();
                    param4.ParameterName = "@PeopleCount_Proxy";
                    param4.Value = peopleCountProxy;
                    updateCmd.Parameters.Add(param4);

                    var param5 = updateCmd.CreateParameter();
                    param5.ParameterName = "@QShare_Self";
                    param5.Value = qShareSelf;
                    updateCmd.Parameters.Add(param5);

                    var param6 = updateCmd.CreateParameter();
                    param6.ParameterName = "@QShare_Proxy";
                    param6.Value = qShareProxy;
                    updateCmd.Parameters.Add(param6);

                    var param7 = updateCmd.CreateParameter();
                    param7.ParameterName = "@IsSummaryComplete";
                    param7.Value = true;
                    updateCmd.Parameters.Add(param7);

                    var param8 = updateCmd.CreateParameter();
                    param8.ParameterName = "@HeaderID";
                    param8.Value = headerID;
                    updateCmd.Parameters.Add(param8);

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
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    int pCountSelf = 0, pCountProxy = 0;
                    long qCountSelf = 0, qCountProxy = 0, totalQShareFromPerson = 0;

                    string GetQuery(string databaseType, string tableName, string sumColumn)
                    {
                        switch (databaseType.ToLower())
                        {
                            case "mysql":
                            case "mariadb":
                                return $"SELECT COUNT(*) AS PeopleCount, SUM(CAST({sumColumn} AS SIGNED)) AS QShare FROM {tableName}";
                            case "postgresql":
                                return $"SELECT COUNT(*) AS PeopleCount, SUM(CAST({sumColumn} AS BIGINT)) AS QShare FROM {tableName}";
                            case "mssql":
                                return $"SELECT COUNT(*) AS PeopleCount, SUM(CAST({sumColumn} AS BIGINT)) AS QShare FROM {tableName}";
                            default:
                                throw new NotSupportedException($"Database type '{databaseType}' not supported");
                        }
                    }

                    string dbType = dbcfg.Config.Type;

                    // ตัวอย่างการดึงข้อมูลแบบ Dynamic Query
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = GetQuery(dbType, "SelfRegistration", "ShareCount");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pCountSelf = reader["PeopleCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount"]);
                                qCountSelf = reader["QShare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare"]);
                            }
                        }
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = GetQuery(dbType, "ProxyRegistration", "ShareCount");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pCountProxy = reader["PeopleCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount"]);
                                qCountProxy = reader["QShare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare"]);
                            }
                        }
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = GetQuery(dbType, "PersonData", "q_share");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalQShareFromPerson = reader["QShare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare"]);
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
            // Adjusted variable name to avoid conflict
            var iniPathForPreview = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfgForPreview = new DBConfig(iniPathForPreview);

            using (var conn = dbcfgForPreview.CreateConnection())
            {
                conn.Open();

                // กำหนดประเภทของฐานข้อมูลจาก Config
                string dbType = dbcfgForPreview.Config.Type;

                // เลือกคำสั่ง SQL ที่เหมาะสมตามฐานข้อมูล
                string query;
                switch (dbType.ToLower())
                {
                    case "mysql":
                    case "mariadb":
                        query = "SELECT SUM(CAST(q_share AS SIGNED)) FROM PersonData";
                        break;
                    case "mssql":
                        query = "SELECT SUM(CAST(q_share AS BIGINT)) FROM PersonData";
                        break;
                    case "postgresql":
                        query = "SELECT SUM(CAST(q_share AS BIGINT)) FROM PersonData";
                        break;
                    default:
                        throw new NotSupportedException($"Database type '{dbType}' not supported");
                }


                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
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

                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate";
                    using (DbCommand checkCmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        checkCmd.CommandText = checkQuery;
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            string query = "SELECT HeaderID, AgendaNumber, AgendaTitle FROM HeaderTemplate ORDER BY HeaderID";
                            using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                            {
                                cmd.CommandText = query;
                                using (DbDataAdapter adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                                {
                                    adapter.SelectCommand = cmd;
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

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM HeaderTemplate WHERE HeaderID = @HeaderID AND IsRegistered = 'บันทึกแล้ว'";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = checkQuery;
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = "@HeaderID";
                    parameter.Value = headerID;
                    cmd.Parameters.Add(parameter);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

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

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = @"
        SELECT peopleCountTotal, qShareTotal, PeopleCount_Self, PeopleCount_Proxy, QShare_Self, QShare_Proxy
        FROM HeaderTemplate
        WHERE HeaderID = @HeaderID";
                using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = query;
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = "@HeaderID";
                    parameter.Value = headerID;
                    cmd.Parameters.Add(parameter);

                    using (DbDataReader reader = cmd.ExecuteReader()) // Use DbDataReader instead of SqlDataReader
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

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox comboBox = (ComboBox)sender;
            string text = comboBox.Items[e.Index].ToString();

            // Draw the background of the item.
            e.DrawBackground();

            // Draw the text of the item.
            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            }

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
    }
}
