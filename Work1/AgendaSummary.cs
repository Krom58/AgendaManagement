﻿using System;
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
    public partial class AgendaSummary : Form
    {
        private Main _Main;
        public AgendaSummary()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
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
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();
                    string query = "SELECT PeopleCount_Total FROM RegistrationSummary";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int peopleCountTotal = (int)cmd.ExecuteScalar();
                        label7.Text = peopleCountTotal.ToString();
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
            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();
                string query = "SELECT AgendaItem, VoteType, Identifier, PeopleCount, FullName, ShareCount FROM VoteResults WHERE AgendaItem = @AgendaItem";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AgendaItem", agendaItem);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
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

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Identifier", searchIdentifier);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }
        }

        private void btnDisapprove_Click(object sender, EventArgs e)
        {
            SaveSelectedData("ไม่เห็นด้วย");
        }

        private void btnInvalidBallot_Click(object sender, EventArgs e)
        {
            SaveSelectedData("บัตรเสีย");
        }

        private void btnAbstain_Click(object sender, EventArgs e)
        {
            SaveSelectedData("งดออกเสียง");
        }

        private void SaveSelectedData(string voteType)
        {
            if (dataGridViewRegistration.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการบันทึก");
                return;
            }

            string agendaItem = comboBox1.GetItemText(comboBox1.SelectedItem);

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
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

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@AgendaItem", agendaItem);
                        checkCmd.Parameters.AddWithValue("@FullName", fullName);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show($"ชื่อ {fullName} มีอยู่แล้วในวาระนี้");
                            continue;
                        }
                    }

                    string insertQuery = @"
                INSERT INTO VoteResults (AgendaItem, VoteType, Identifier, PeopleCount, FullName, ShareCount)
                VALUES (@AgendaItem, @VoteType, @Identifier, @PeopleCount, @FullName, @ShareCount)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@AgendaItem", agendaItem);
                        insertCmd.Parameters.AddWithValue("@VoteType", voteType);
                        insertCmd.Parameters.AddWithValue("@Identifier", identifier);
                        insertCmd.Parameters.AddWithValue("@PeopleCount", peopleCount);
                        insertCmd.Parameters.AddWithValue("@FullName", fullName);
                        insertCmd.Parameters.AddWithValue("@ShareCount", shareCount);
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
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT HeaderID,
                       AgendaNumber,
                       AgendaTitle,
                       qShareTotal,
                       peopleCountTotal
                FROM HeaderTemplate
                ORDER BY HeaderID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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

                DataTable dt = comboBox1.DataSource as DataTable;
                if (dt == null) return;

                DataRow[] rows = dt.Select($"HeaderID = {headerID}");
                if (rows.Length == 0) return;

                long qShareTotal = rows[0]["qShareTotal"] == DBNull.Value ? 0 : Convert.ToInt64(rows[0]["qShareTotal"]);
                int peopleCountTotal = rows[0]["peopleCountTotal"] == DBNull.Value ? 0 : Convert.ToInt32(rows[0]["peopleCountTotal"]);

                label15.Text = qShareTotal.ToString("N0");
                label17.Text = peopleCountTotal.ToString();

                label7.Text = qShareTotal.ToString("N0");

                string selectedAgendaItem = comboBox1.GetItemText(comboBox1.SelectedItem);
                LoadVoteResults(selectedAgendaItem);

                CalculateAndDisplayVoteSummary();
            }
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
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();

                    string voteQuery = @"
            SELECT VoteType, SUM(ShareCount) AS TotalShareCount
            FROM VoteResults
            WHERE AgendaItem = @AgendaItem
            GROUP BY VoteType";
                    using (SqlCommand voteCmd = new SqlCommand(voteQuery, conn))
                    {
                        voteCmd.Parameters.AddWithValue("@AgendaItem", agendaItem);

                        using (SqlDataReader reader = voteCmd.ExecuteReader())
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

                long adjustedApprove = qShareTotal - (disapprove + abstain + invalidBallot);

                label6.Text = disapprove.ToString("N0");
                label5.Text = abstain.ToString("N0");
                label8.Text = invalidBallot.ToString("N0");
                label7.Text = adjustedApprove.ToString("N0");

                long totalVotesSum = adjustedApprove + disapprove + invalidBallot + abstain;
                label9.Text = totalVotesSum.ToString("N0");

                label10.Text = CalculatePercentage(disapprove, totalVotesSum);
                label11.Text = CalculatePercentage(abstain, totalVotesSum);
                label12.Text = CalculatePercentage(invalidBallot, totalVotesSum);
                label18.Text = CalculatePercentage(adjustedApprove, totalVotesSum);

                // Calculate the sum of percentages
                double totalPercentage = ParsePercentage(label18.Text) + ParsePercentage(label10.Text) + ParsePercentage(label12.Text) + ParsePercentage(label11.Text);
                label13.Text = $"{totalPercentage:F2}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดขณะคำนวณคะแนน: " + ex.Message);
            }
        }

        private long ParseLabelToLong(string text)
        {
            string[] parts = text.Split(' ');
            if (parts.Length > 0)
            {
                if (long.TryParse(parts[0].Replace(",", ""), out long result))
                {
                    return result;
                }
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
    }
}

