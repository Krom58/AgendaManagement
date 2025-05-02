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
using System.IO;
using System.Data.Common;

namespace Work1
{
    public partial class AgendaSummary : Form
    {
        private Main _Main;
        private bool isSwapped = false; // ตัวแปรสถานะ
        public AgendaSummary()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // Set DropDownStyle to DropDownList to make ComboBox non-editable
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            // Set DrawMode to OwnerDrawFixed to handle custom drawing
            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += new DrawItemEventHandler(comboBox1_DrawItem);
        }
        public AgendaSummary(Main main)
        {
            InitializeComponent();
            _Main = main;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            _Main.Show();
            this.Close();
        }

        private void AgendaSummary_Load(object sender, EventArgs e)
        {
            LoadHeaderTemplateData();
            LoadPeopleCountTotal();

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox1_SelectedIndexChanged_1(comboBox1, EventArgs.Empty);
            }
        }

        private void LoadPeopleCountTotal()
        {
            try
            {
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT PeopleCount_Total FROM RegistrationSummary";
                    using (var cmd = conn.CreateCommand()) // Use DbConnection's CreateCommand method
                    {
                        cmd.CommandText = query;
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int peopleCountTotal = (int)result;
                            label7.Text = peopleCountTotal.ToString();
                        }
                        else
                        {
                            label7.Text = "0"; // or handle the null case appropriately
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void LoadVoteResults(string agendaItem)
        {
            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = "SELECT AgendaItem, VoteType, Identifier, PeopleCount, FullName, ShareCount FROM VoteResults WHERE AgendaItem = @AgendaItem";
                using (var cmd = conn.CreateCommand()) // Use DbConnection's CreateCommand method
                {
                    cmd.CommandText = query;

                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = "@AgendaItem";
                    parameter.Value = agendaItem;
                    cmd.Parameters.Add(parameter);

                    using (var adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView8.DataSource = dt;
                        dataGridView8.Columns["AgendaItem"].HeaderText = "วาระ";
                        dataGridView8.Columns["VoteType"].HeaderText = "ประเภทการลงคะแนน";
                        dataGridView8.Columns["Identifier"].HeaderText = "หมายเลข";
                        dataGridView8.Columns["PeopleCount"].HeaderText = "จำนวนคน";
                        dataGridView8.Columns["FullName"].HeaderText = "ชื่อ - สกุล";
                        dataGridView8.Columns["ShareCount"].HeaderText = "จำนวนหุ้น";

                        foreach (DataGridViewColumn col in dataGridView8.Columns)
                        {
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }

        private void btnSearchIdentifier_Click(object sender, EventArgs e)
        {
            string searchIdentifier = txtSearchIdentifier.Text.Trim();
            if (string.IsNullOrEmpty(searchIdentifier))
            {
                MessageBox.Show("กรุณากรอกหมายเลขที่ต้องการค้นหา");
                return;
            }

            string query = @"
            SELECT 'มาเอง' AS Type, RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note
            FROM SelfRegistration
            WHERE Identifier = @Identifier
            UNION ALL
            SELECT 'มอบฉันทะ' AS Type, RegistrationID, Identifier, PeopleCount, FullName, ShareCount, Note
            FROM ProxyRegistration
            WHERE Identifier = @Identifier
            ORDER BY FullName";

            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // ใช้ DbConnection และ DbCommand เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = "@Identifier";
                        parameter.Value = searchIdentifier;
                        cmd.Parameters.Add(parameter);

                        using (var adapter = dbcfg.Factory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("ไม่พบหมายเลขที่ค้นหา");
                            }
                            else
                            {
                                dataGridViewRegistration.DataSource = dt;
                                dataGridViewRegistration.Columns["RegistrationID"].HeaderText = "รหัสลงทะเบียน";
                                dataGridViewRegistration.Columns["Identifier"].HeaderText = "หมายเลข";
                                dataGridViewRegistration.Columns["PeopleCount"].HeaderText = "";
                                dataGridViewRegistration.Columns["FullName"].HeaderText = "ชื่อ - สกุล";
                                dataGridViewRegistration.Columns["ShareCount"].HeaderText = "จำนวนหุ้น";
                                dataGridViewRegistration.Columns["Note"].HeaderText = "หมายเหตุ";
                                dataGridViewRegistration.Columns["RegistrationID"].Visible = false;
                                dataGridViewRegistration.Columns["ShareCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dataGridViewRegistration.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                                foreach (DataGridViewColumn col in dataGridViewRegistration.Columns)
                                {
                                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
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

        private void btnDisapprove_Click(object sender, EventArgs e)
        {
            SaveSelectedData("ไม่เห็นด้วย");
            // เก็บค่า HeaderID ที่เลือกไว้ก่อน
            int currentHeaderID = comboBox1.SelectedValue != null ? Convert.ToInt32(comboBox1.SelectedValue) : 0;
            LoadHeaderTemplateData();
            // ตั้งค่า SelectedValue กลับไป
            if (currentHeaderID != 0)
                comboBox1.SelectedValue = currentHeaderID;
        }

        private void btnInvalidBallot_Click(object sender, EventArgs e)
        {
            SaveSelectedData("บัตรเสีย");
            int currentHeaderID = comboBox1.SelectedValue != null ? Convert.ToInt32(comboBox1.SelectedValue) : 0;
            LoadHeaderTemplateData();
            if (currentHeaderID != 0)
                comboBox1.SelectedValue = currentHeaderID;
        }

        private void btnAbstain_Click(object sender, EventArgs e)
        {
            SaveSelectedData("งดออกเสียง");
            int currentHeaderID = comboBox1.SelectedValue != null ? Convert.ToInt32(comboBox1.SelectedValue) : 0;
            LoadHeaderTemplateData();
            if (currentHeaderID != 0)
                comboBox1.SelectedValue = currentHeaderID;
        }

        private void SaveSelectedData(string voteType)
        {
            if (dataGridViewRegistration.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการบันทึก");
                return;
            }

            string agendaItem = comboBox1.GetItemText(comboBox1.SelectedItem);

            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // ใช้ DbConnection และ DbCommand เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridViewRegistration.SelectedRows)
                {
                    string identifier = row.Cells["Identifier"].Value.ToString();
                    int peopleCount = Convert.ToInt32(row.Cells["PeopleCount"].Value);
                    string fullName = row.Cells["FullName"].Value.ToString();
                    long shareCount = Convert.ToInt64(row.Cells["ShareCount"].Value);

                    string checkQuery = @"
            SELECT COUNT(*) 
            FROM VoteResults 
            WHERE AgendaItem = @AgendaItem AND FullName = @FullName";

                    using (var checkCmd = conn.CreateCommand())
                    {
                        checkCmd.CommandText = checkQuery;

                        var agendaParam = checkCmd.CreateParameter();
                        agendaParam.ParameterName = "@AgendaItem";
                        agendaParam.Value = agendaItem;
                        checkCmd.Parameters.Add(agendaParam);

                        var fullNameParam = checkCmd.CreateParameter();
                        fullNameParam.ParameterName = "@FullName";
                        fullNameParam.Value = fullName;
                        checkCmd.Parameters.Add(fullNameParam);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show($"ชื่อ {fullName} มีอยู่แล้วในวาระนี้");
                            continue;
                        }
                    }

                    string insertQuery = @"
            INSERT INTO VoteResults (AgendaItem, VoteType, Identifier, PeopleCount, FullName, ShareCount)
            VALUES (@AgendaItem, @VoteType, @Identifier, @PeopleCount, @FullName, @ShareCount)";

                    using (var insertCmd = conn.CreateCommand())
                    {
                        insertCmd.CommandText = insertQuery;

                        var agendaParam = insertCmd.CreateParameter();
                        agendaParam.ParameterName = "@AgendaItem";
                        agendaParam.Value = agendaItem;
                        insertCmd.Parameters.Add(agendaParam);

                        var voteTypeParam = insertCmd.CreateParameter();
                        voteTypeParam.ParameterName = "@VoteType";
                        voteTypeParam.Value = voteType;
                        insertCmd.Parameters.Add(voteTypeParam);

                        var identifierParam = insertCmd.CreateParameter();
                        identifierParam.ParameterName = "@Identifier";
                        identifierParam.Value = identifier;
                        insertCmd.Parameters.Add(identifierParam);

                        var peopleCountParam = insertCmd.CreateParameter();
                        peopleCountParam.ParameterName = "@PeopleCount";
                        peopleCountParam.Value = peopleCount;
                        insertCmd.Parameters.Add(peopleCountParam);

                        var fullNameParam = insertCmd.CreateParameter();
                        fullNameParam.ParameterName = "@FullName";
                        fullNameParam.Value = fullName;
                        insertCmd.Parameters.Add(fullNameParam);

                        var shareCountParam = insertCmd.CreateParameter();
                        shareCountParam.ParameterName = "@ShareCount";
                        shareCountParam.Value = shareCount;
                        insertCmd.Parameters.Add(shareCountParam);

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }

            LoadVoteResults(agendaItem);
        }


        private void LoadHeaderTemplateData()
        {
            try
            {
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // ใช้ DbConnection และ DbCommand เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT HeaderID,
                           AgendaNumber,
                           AgendaTitle,
                           AgendaType,
                           qShareTotal,
                           peopleCountTotal
                    FROM HeaderTemplate
                    ORDER BY HeaderID";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        using (var adapter = dbcfg.Factory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dt.Columns.Add("AgendaDisplay", typeof(string),
                                "'วาระที่ ' + AgendaNumber + ' - ' + AgendaTitle");

                            comboBox1.DataSource = dt;
                            comboBox1.DisplayMember = "AgendaDisplay";
                            comboBox1.ValueMember = "HeaderID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null) return;

            DataRowView drv = comboBox1.SelectedItem as DataRowView;
            if (drv != null)
            {
                int headerID = Convert.ToInt32(drv["HeaderID"]);
                int agendaType = Convert.ToInt32(drv["AgendaType"]);

                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string checkQuery = "SELECT IsAgendaClosed FROM HeaderTemplate WHERE HeaderID = @HeaderID";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = checkQuery;

                        var headerParam = cmd.CreateParameter();
                        headerParam.ParameterName = "@HeaderID";
                        headerParam.Value = headerID;
                        cmd.Parameters.Add(headerParam);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            btnEndAgenda.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            // Handle the case where no data is returned
                            btnEndAgenda.BackColor = Color.LightGreen;
                        }
                    }

                    DataTable dt = comboBox1.DataSource as DataTable;
                    if (dt == null) return;

                    DataRow[] rows = dt.Select($"HeaderID = {headerID}");
                    if (rows.Length == 0) return;

                    long qShareTotal = rows[0]["qShareTotal"] == DBNull.Value ? 0 : Convert.ToInt64(rows[0]["qShareTotal"]);
                    int peopleCountTotal = rows[0]["peopleCountTotal"] == DBNull.Value ? 0 : Convert.ToInt32(rows[0]["peopleCountTotal"]);

                    label15.Text = qShareTotal.ToString("N0");
                    label17.Text = peopleCountTotal.ToString();

                    string selectedAgendaItem = comboBox1.GetItemText(comboBox1.SelectedItem);
                    AdjustControlsForAgendaType(agendaType);
                    LoadVoteResults(selectedAgendaItem);

                    CalculateAndDisplayVoteSummary();
                }
            }
        }
        private void AdjustControlsForAgendaType(int agendaType)
        {
            if (agendaType == 1 && !isSwapped)
            {
                // Swap labels
                SwapLabels(label23, label21);
                SwapLabels(label5, label9);
                SwapLabels(label11, label13);

                // Set label11 to "-"
                label11.Text = "-";

                isSwapped = true; // ตั้งค่าสถานะเป็น true หลังจากสลับตำแหน่งแล้ว
            }
            else if (agendaType == 2 && isSwapped)
            {
                // Reset labels to original positions if needed
                ResetLabels();
                isSwapped = false; // ตั้งค่าสถานะเป็น false หลังจากรีเซ็ตตำแหน่งแล้ว
            }
        }

        private void SwapLabels(Label label1, Label label2)
        {
            Point tempLocation = label1.Location;
            label1.Location = label2.Location;
            label2.Location = tempLocation;
        }

        private void ResetLabels()
        {
            // Reset labels to their original positions
            label23.Location = new Point(59, 397);
            label21.Location = new Point(59, 443);
            label5.Location = new Point(330, 397);
            label9.Location = new Point(330, 443);
            label11.Location = new Point(570, 397);
            label13.Location = new Point(570, 443);

            // Reset label11 text
            label11.Text = "0.0000%";
        }

        private void CalculateAndDisplayVoteSummary()
        {
            string agendaItem = comboBox1.GetItemText(comboBox1.SelectedItem);
            if (string.IsNullOrEmpty(agendaItem))
            {
                MessageBox.Show("กรุณาเลือกวาระ");
                return;
            }

            long disapprove = 0, abstain = 0, invalidBallot = 0, totalVotes = 0;
            long qShareTotal = ParseLabelToLong(label15.Text);

            try
            {
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    string voteQuery = @"
            SELECT VoteType, SUM(ShareCount) AS TotalShareCount
            FROM VoteResults
            WHERE AgendaItem = @AgendaItem
            GROUP BY VoteType";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = voteQuery;

                        var agendaParam = cmd.CreateParameter();
                        agendaParam.ParameterName = "@AgendaItem";
                        agendaParam.Value = agendaItem;
                        cmd.Parameters.Add(agendaParam);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string voteType = reader["VoteType"].ToString();
                                long shareCount = reader["TotalShareCount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TotalShareCount"]);

                                switch (voteType)
                                {
                                    case "ไม่เห็นด้วย":
                                        disapprove += shareCount;
                                        break;
                                    case "งดออกเสียง":
                                        abstain += shareCount;
                                        break;
                                    case "บัตรเสีย":
                                        invalidBallot += shareCount;
                                        break;
                                }

                                totalVotes += shareCount;
                            }
                        }
                    }
                }

                long adjustedApprove;
                long totalVotesSum;
                if (comboBox1.SelectedItem is DataRowView drv && Convert.ToInt32(drv["AgendaType"]) == 1)
                {
                    // สำหรับ agendaType == 1
                    adjustedApprove = qShareTotal - (disapprove + invalidBallot);
                    label11.Text = "-"; // ตั้งค่าร้อยละของ "งดออกเสียง" เป็น "-"
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot; // ไม่รวม "งดออกเสียง"
                }
                else
                {
                    // สำหรับ agendaType อื่นๆ
                    adjustedApprove = qShareTotal - (disapprove + abstain + invalidBallot);
                    label11.Text = CalculatePercentage(abstain, qShareTotal);
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot + abstain;
                }

                label6.Text = disapprove.ToString("N0");
                label5.Text = abstain.ToString("N0");
                label8.Text = invalidBallot.ToString("N0");
                label7.Text = adjustedApprove.ToString("N0");

                label9.Text = totalVotesSum.ToString("N0");

                label10.Text = CalculatePercentage(disapprove, qShareTotal);
                label12.Text = CalculatePercentage(invalidBallot, qShareTotal);
                label18.Text = CalculatePercentage(adjustedApprove, qShareTotal);

                // Calculate the sum of percentages
                double totalPercentage = ParsePercentage(label18.Text) + ParsePercentage(label10.Text) + ParsePercentage(label12.Text) + (label11.Text == "-" ? 0 : ParsePercentage(label11.Text));
                label13.Text = $"{totalPercentage:F2}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดขณะคำนวณคะแนน: " + ex.Message);
            }
        }
        private long ParseLabelToLong(string text)
        {
            if (text.EndsWith("%"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            if (double.TryParse(text, out double result))
            {
                return (long)result; // Explicitly cast double to long
            }
            return 0;
        }

        private string CalculatePercentage(long value, long total)
        {
            double percentage = total > 0 ? (double)value / total * 100 : 0;
            return $"{percentage:F2}%";
        }

        private double ParsePercentage(string text)
        {
            if (text.EndsWith("%"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return 0;
        }

        private void btnEndAgenda_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null) return;

            // ถามผู้ใช้ว่ามั่นใจที่จะจบวาระหรือไม่
            DialogResult confirmResult = MessageBox.Show(
                "คุณแน่ใจหรือไม่ที่จะจบวาระนี้?",
                "ยืนยันการจบวาระ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
                return;

            int headerID = (int)comboBox1.SelectedValue;
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // ใช้ DbConnection และ DbCommand เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE HeaderTemplate SET IsAgendaClosed = true WHERE HeaderID = @HeaderID";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = updateQuery;

                        var headerParam = cmd.CreateParameter();
                        headerParam.ParameterName = "@HeaderID";
                        headerParam.Value = headerID;
                        cmd.Parameters.Add(headerParam);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }

            // Store the currently selected value
            var selectedValue = comboBox1.SelectedValue;

            // Refresh the form
            LoadHeaderTemplateData();

            // Restore the selected value
            comboBox1.SelectedValue = selectedValue;
        }

        private void DisableAgendaControls()
        {
            btnEndAgenda.Enabled = false;
            btnDisapprove.Enabled = false;
            btnInvalidBallot.Enabled = false;
            btnAbstain.Enabled = false;
            btnSearchIdentifier.Enabled = false;
            txtSearchIdentifier.Enabled = false;
            dataGridViewRegistration.ReadOnly = true;
            dataGridView8.ReadOnly = true;
        }

        private void EnableAgendaControls()
        {
            btnEndAgenda.Enabled = true;
            btnDisapprove.Enabled = true;
            btnInvalidBallot.Enabled = true;
            btnAbstain.Enabled = true;
            btnSearchIdentifier.Enabled = true;
            txtSearchIdentifier.Enabled = true;
            dataGridViewRegistration.ReadOnly = false;
            dataGridView8.ReadOnly = false;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView8.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการลบ");
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                "คุณแน่ใจหรือไม่ที่จะลบข้อมูลนี้?",
                "ยืนยันการลบ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
                return;

            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // ใช้ DbConnection และ DbCommand เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                try
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView8.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            string agendaItem = row.Cells["AgendaItem"].Value.ToString();
                            string identifier = row.Cells["Identifier"].Value.ToString();
                            string fullName = row.Cells["FullName"].Value.ToString();

                            string deleteQuery = @"
                    DELETE FROM VoteResults 
                    WHERE AgendaItem = @AgendaItem AND Identifier = @Identifier AND FullName = @FullName";

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = deleteQuery;

                                var agendaParam = cmd.CreateParameter();
                                agendaParam.ParameterName = "@AgendaItem";
                                agendaParam.Value = agendaItem;
                                cmd.Parameters.Add(agendaParam);

                                var identifierParam = cmd.CreateParameter();
                                identifierParam.ParameterName = "@Identifier";
                                identifierParam.Value = identifier;
                                cmd.Parameters.Add(identifierParam);

                                var fullNameParam = cmd.CreateParameter();
                                fullNameParam.ParameterName = "@FullName";
                                fullNameParam.Value = fullName;
                                cmd.Parameters.Add(fullNameParam);

                                cmd.ExecuteNonQuery();
                            }

                            dataGridView8.Rows.Remove(row);
                        }
                    }

                    MessageBox.Show("ลบข้อมูลเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }

            // Store the currently selected value
            var selectedValue = comboBox1.SelectedValue;

            // Refresh the form
            LoadHeaderTemplateData();

            // Restore the selected value
            comboBox1.SelectedValue = selectedValue;
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

        private void btnexportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    // Get the selected item text from ComboBox
                    string selectedItem = comboBox1.GetItemText(comboBox1.SelectedItem);

                    // Extract the "วาระที่" and the number from the selected item
                    string worksheetName = selectedItem.Split(' ')[1];

                    // Create the main worksheet for the agenda
                    var mainWorksheet = workbook.Worksheets.Add("วาระที่ " + worksheetName);

                    // Set the header from ComboBox
                    mainWorksheet.Cell(1, 1).Value = selectedItem;

                    // Set the table headers
                    mainWorksheet.Cell(3, 1).Value = "รายการ";
                    mainWorksheet.Cell(3, 2).Value = "จำนวนเสียง";
                    mainWorksheet.Cell(3, 3).Value = "ร้อยละ (%)";

                    // Define labels and their corresponding values
                    var labels = new (string, string, string)[]
                    {
                ("เห็นด้วย (Approved)", label7.Text, label18.Text),
                ("ไม่เห็นด้วย (Disapproved)", label6.Text, label10.Text),
                ("บัตรเสีย (Invalid ballots)", label8.Text, label12.Text),
                ("งดออกเสียง (Abstained)", label5.Text, label11.Text),
                ("รวม", label9.Text, label13.Text)
                    };

                    // Check the selected agenda type and adjust labels if needed
                    if (comboBox1.SelectedItem is DataRowView drv)
                    {
                        int agendaType = Convert.ToInt32(drv["AgendaType"]);
                        if (agendaType == 1)
                        {
                            // Swap labels for agendaType 1
                            labels = new (string, string, string)[]
                            {
                        ("เห็นด้วย (Approved)", label7.Text, label18.Text),
                        ("ไม่เห็นด้วย (Disapproved)", label6.Text, label10.Text),
                        ("บัตรเสีย (Invalid ballots)", label8.Text, label12.Text),
                        ("รวม", label9.Text, label13.Text),
                        ("งดออกเสียง (Abstained)", label5.Text, "-")
                            };
                        }
                        else if (agendaType == 2)
                        {
                            // Reset labels for other agenda types
                            labels = new (string, string, string)[]
                            {
                        ("เห็นด้วย (Approved)", label7.Text, label18.Text),
                        ("ไม่เห็นด้วย (Disapproved)", label6.Text, label10.Text),
                        ("บัตรเสีย (Invalid ballots)", label8.Text, label12.Text),
                        ("งดออกเสียง (Abstained)", label5.Text, label11.Text),
                        ("รวม", label9.Text, label13.Text)
                            };
                        }
                    }

                    // Set the labels
                    for (int i = 0; i < labels.Length; i++)
                    {
                        mainWorksheet.Cell(4 + i, 1).Value = labels[i].Item1;
                        mainWorksheet.Cell(4 + i, 2).Value = labels[i].Item2;
                        mainWorksheet.Cell(4 + i, 3).Value = labels[i].Item3;
                    }

                    mainWorksheet.Cell(10, 1).Value = "จำนวนหุ้นผู้เข้าประชุมทั้งหมด";
                    mainWorksheet.Cell(10, 3).Value = label15.Text;

                    mainWorksheet.Cell(11, 1).Value = "จำนวนคนผู้เข้าประชุม";
                    mainWorksheet.Cell(11, 3).Value = label17.Text;

                    // Set font size and font family
                    var range = mainWorksheet.RangeUsed();
                    range.Style.Font.FontSize = 24;
                    range.Style.Font.FontName = "Angsana New";

                    // Center align specific cells
                    mainWorksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    mainWorksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    mainWorksheet.Cell(3, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    mainWorksheet.Cell(4, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(4, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(5, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(5, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(6, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(6, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(7, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(7, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(8, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(8, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(10, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    mainWorksheet.Cell(11, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    // ปรับความกว้างของคอลัมน์ให้พอดีกับตัวอักษร ยกเว้นเซลล์ (1,1)
                    // เก็บค่าในเซลล์ (1,1) แล้วล้างออกชั่วคราว
                    var headerValue = mainWorksheet.Cell(1, 1).Value;
                    mainWorksheet.Cell(1, 1).Value = "";

                    // ปรับความกว้างของคอลัมน์ 1 โดยไม่คำนึงถึงเซลล์ (1,1)
                    mainWorksheet.Column(1).AdjustToContents();

                    // คืนค่าของเซลล์ (1,1) กลับมา
                    mainWorksheet.Cell(1, 1).Value = headerValue;

                    // ปรับความกว้างของคอลัมน์ที่ 2 และ 3 ตามตัวอักษร
                    mainWorksheet.Columns(2, 3).AdjustToContents();

                    // Define sections for each vote type
                    var voteTypes = new[] { "ไม่เห็นด้วย", "บัตรเสีย", "งดออกเสียง" };
                    foreach (var voteType in voteTypes)
                    {
                        var worksheet = workbook.Worksheets.Add(voteType);

                        // Export data from dataGridView8 to Excel
                        int startRow = 1 ; // Starting row for dataGridView8 data

                        // Set headers for each section
                        for (int i = 0; i < dataGridView8.Columns.Count; i++)
                        {
                            worksheet.Cell(startRow, i + 1).Value = dataGridView8.Columns[i].HeaderText;
                        }
                        startRow++;

                        // Add data for each vote type
                        foreach (DataGridViewRow row in dataGridView8.Rows)
                        {
                            if (row.Cells["VoteType"].Value?.ToString() == voteType)
                            {
                                for (int j = 0; j < dataGridView8.Columns.Count; j++)
                                {
                                    worksheet.Cell(startRow, j + 1).Value = row.Cells[j].Value?.ToString();
                                }
                                startRow++;
                            }
                        }
                        range = worksheet.RangeUsed();
                        range.Style.Font.FontSize = 24;
                        range.Style.Font.FontName = "Angsana New";
                        worksheet.Columns().AdjustToContents();
                        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }

                    // Save the workbook with the ComboBox selected item as the filename
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + selectedItem + ".xlsx";
                    workbook.SaveAs(filePath);

                    MessageBox.Show("Exported successfully to " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

