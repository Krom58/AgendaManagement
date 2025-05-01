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
using Work1;

namespace AgendaDetail
{
    public partial class AgendaDetail : Form
    {
        private bool isSwapped = false; // ตัวแปรสถานะสำหรับการสลับ label (AgendaType 1)
        private DataTable dtHeaders;    // DataTable สำหรับเก็บข้อมูลวาระ (HeaderTemplate)
        private int currentAgendaIndex = 0; // ดัชนีวาระที่เลือกอยู่
        private Timer refreshTimer; // Timer สำหรับรีเฟรชฟอร์ม

        public AgendaDetail()
        {
            InitializeComponent();
            InitializeTimer(); // เรียกใช้การตั้งค่า Timer
        }

        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 3000; // ตั้งค่าให้ Timer ทำงานทุกๆ 3 วินาที (3000 มิลลิวินาที)
            refreshTimer.Tick += new EventHandler(RefreshTimer_Tick);
            refreshTimer.Start(); // เริ่มการทำงานของ Timer
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateAgendaDisplay(); // เรียกใช้ฟังก์ชันเพื่อรีเฟรชข้อมูลในฟอร์ม
        }

        private void AgendaDetail_Load(object sender, EventArgs e)
        {
            LoadHeaderDataForNavigation(); // โหลดข้อมูลวาระจาก DB ลงใน dtHeaders
            LoadPeopleCountTotal(); // โหลดข้อมูลจำนวนคนรวมจาก RegistrationSummary

            if (dtHeaders != null && dtHeaders.Rows.Count > 0)
            {
                currentAgendaIndex = 0;
                UpdateAgendaDisplay();
            }
        }

        private void LoadPeopleCountTotal()
        {
            try
            {
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT \"PeopleCount_Total\" FROM \"RegistrationSummary\"";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            int peopleCountTotal = Convert.ToInt32(result);
                            label7.Text = peopleCountTotal.ToString();
                        }
                        else
                        {
                            label7.Text = "No data available"; // Default label text when no data is found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void LoadHeaderDataForNavigation()
        {
            try
            {
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT ""HeaderID"", ""AgendaNumber"", ""AgendaTitle"", ""AgendaType"", ""qShareTotal"", ""peopleCountTotal""
                FROM ""HeaderTemplate""
                ORDER BY ""HeaderID""";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            dtHeaders = new DataTable();
                            adapter.Fill(dtHeaders);
                            // เพิ่มคอลัมน์ AgendaDisplay
                            dtHeaders.Columns.Add("AgendaDisplay", typeof(string), "'วาระที่ ' + AgendaNumber + ' - ' + AgendaTitle");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลวาระ: " + ex.Message);
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
            label23.Location = new Point(503, 643);
            label21.Location = new Point(503, 725);
            label5.Location = new Point(852, 643);
            label9.Location = new Point(852, 725);
            label11.Location = new Point(1152, 643);
            label13.Location = new Point(1152, 725);

            label11.Text = "0.0000%";
        }

        private void CalculateAndDisplayVoteSummary()
        {
            string agendaItem = HeaderLabel.Text; // ใช้ TextBox1 เป็นหัวข้อวาระ
            if (string.IsNullOrEmpty(agendaItem))
            {
                MessageBox.Show("กรุณาเลือกวาระ");
                return;
            }

            long disapprove = 0, abstain = 0, invalidBallot = 0, totalVotes = 0;
            long qShareTotal = ParseLabelToLong(label15.Text);

            try
            {
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string voteQuery = @"
                SELECT ""VoteType"", SUM(""ShareCount"") AS ""TotalShareCount""
                FROM ""VoteResults""
                WHERE ""AgendaItem"" = @AgendaItem
                GROUP BY ""VoteType""";
                    using (DbCommand voteCmd = conn.CreateCommand())
                    {
                        voteCmd.CommandText = voteQuery;
                        var param = voteCmd.CreateParameter();
                        param.ParameterName = "@AgendaItem";
                        param.Value = agendaItem;
                        voteCmd.Parameters.Add(param);

                        using (DbDataReader reader = voteCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string voteType = reader["\"VoteType\""].ToString();
                                long shareCount = reader["\"TotalShareCount\""] == DBNull.Value ? 0 : Convert.ToInt64(reader["\"TotalShareCount\""]);

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
                // ถ้า AgendaType = 1 (แบบที่ไม่รวมงดออกเสียงในการคำนวณ)
                if (dtHeaders.Rows[currentAgendaIndex]["AgendaType"].ToString() == "1")
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (dtHeaders == null || dtHeaders.Rows.Count == 0)
                return;
            if (currentAgendaIndex > 0)
            {
                currentAgendaIndex--;
                UpdateAgendaDisplay();
            }
            else
            {
                // Navigate to the RegiterationDetail page
                this.Hide();
                RegisterationDetail regDetail = new RegisterationDetail();
                regDetail.Show();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtHeaders == null || dtHeaders.Rows.Count == 0)
                return;
            if (currentAgendaIndex < dtHeaders.Rows.Count - 1)
            {
                currentAgendaIndex++;
                UpdateAgendaDisplay();
            }
            else
            {
                MessageBox.Show("นี่คือวาระสุดท้ายแล้ว");
            }
        }

        private void UpdateAgendaDisplay()
        {
            if (dtHeaders == null || dtHeaders.Rows.Count == 0)
                return;
            DataRow currentRow = dtHeaders.Rows[currentAgendaIndex];

            // นำ AgendaDisplay ไปแสดงใน TextBox1
            HeaderLabel.Text = currentRow["AgendaDisplay"].ToString();

            // อ่านค่า AgendaType, qShareTotal และ peopleCountTotal จาก DataRow
            int agendaType = Convert.ToInt32(currentRow["AgendaType"]);
            long qShareTotal = currentRow["qShareTotal"] == DBNull.Value ? 0 : Convert.ToInt64(currentRow["qShareTotal"]);
            int peopleCountTotal = currentRow["peopleCountTotal"] == DBNull.Value ? 0 : Convert.ToInt32(currentRow["peopleCountTotal"]);

            // อัปเดต label ที่เกี่ยวข้อง
            label15.Text = qShareTotal.ToString("N0");
            label17.Text = peopleCountTotal.ToString();
            label7.Text = qShareTotal.ToString("N0");

            // ปรับตำแหน่ง label ตาม AgendaType
            AdjustControlsForAgendaType(agendaType);

            // เรียกคำนวณและแสดงผลคะแนน (ใช้ฟังก์ชันเดิม)
            CalculateAndDisplayVoteSummary();
        }
    }
}
