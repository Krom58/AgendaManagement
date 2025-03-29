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

namespace AgendaDetail
{
    public partial class AgendaDetail : Form
    {
        private bool isSwapped = false; // ตัวแปรสถานะ
        public AgendaDetail()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
        }

        private void AgendaDetail_Load(object sender, EventArgs e)
        {
            LoadHeaderTemplateData();
            LoadPeopleCountTotal();

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox1_SelectedIndexChanged(comboBox1, EventArgs.Empty);
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
                   AgendaType,
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null) return;
            DataRowView drv = comboBox1.SelectedItem as DataRowView;
            if (drv != null)
            {
                int headerID = Convert.ToInt32(drv["HeaderID"]);
                int agendaType = Convert.ToInt32(drv["AgendaType"]);
                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT IsAgendaClosed FROM HeaderTemplate WHERE HeaderID = @HeaderID";
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@HeaderID", headerID);
                        if (drv != null)
                        {
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
                            AdjustControlsForAgendaType(agendaType);
                            CalculateAndDisplayVoteSummary();
                        }
                    }
                }
            }
        }
        private void AdjustControlsForAgendaType(int agendaType)
        {
            if (agendaType == 1 && !isSwapped)
            {
                SwapLabels(label23, label21);
                SwapLabels(label5, label9);
                SwapLabels(label11, label13);

                label11.Text = "-";

                isSwapped = true;
            }
            else if (agendaType == 2 && isSwapped)
            {
                ResetLabels();
                isSwapped = false;
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
            label23.Location = new Point(623, 533);
            label21.Location = new Point(623, 579);
            label5.Location = new Point(894, 533);
            label9.Location = new Point(894, 579);
            label11.Location = new Point(1134, 533);
            label13.Location = new Point(1134, 579);

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

                long adjustedApprove;
                long totalVotesSum;
                if (comboBox1.SelectedItem is DataRowView drv && Convert.ToInt32(drv["AgendaType"]) == 1)
                {
                    adjustedApprove = qShareTotal - (disapprove + invalidBallot);
                    label11.Text = "-";
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot;
                }
                else
                {
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
                return (long)result;
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
