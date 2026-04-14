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
        private DataTable dtHeaders;    // DataTable สำหรับเก็บข้อมูลวาระ (HeaderTemplate)
        private int currentAgendaIndex = 0; // ดัชนีวาระที่เลือกอยู่
        private System.Windows.Forms.Timer refreshTimer; // Timer สำหรับ auto-refresh
        private int lastAgendaType = -1; // เก็บ AgendaType ล่าสุดเพื่อตรวจสอบการเปลี่ยนแปลง

        public AgendaDetail()
        {
            InitializeComponent();
            
            // ตั้งค่า Timer สำหรับ auto-refresh ทุก 3 วินาที
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000; // 3000 milliseconds = 3 วินาที
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // เรียก method RefreshCurrentPage ทุกๆ 3 วินาที
            RefreshCurrentPage();
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
                    string query = "SELECT PeopleCount_Total FROM RegistrationSummary";
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
                System.Diagnostics.Debug.WriteLine($"Error loading people count: {ex.Message}");
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
                SELECT HeaderID, AgendaNumber, AgendaTitle, AgendaType, qShareTotal, peopleCountTotal
                FROM HeaderTemplate
                ORDER BY HeaderID";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            DataTable newHeaders = new DataTable();
                            adapter.Fill(newHeaders);
                            // เพิ่มคอลัมน์ AgendaDisplay
                            newHeaders.Columns.Add("AgendaDisplay", typeof(string), "'วาระที่ ' + AgendaNumber + ' - ' + AgendaTitle");
                            dtHeaders = newHeaders;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading header data: {ex.Message}");
            }
        }

        private void AdjustControlsForAgendaType(int agendaType)
        {
            // ถ้า AgendaType ไม่เปลี่ยน ไม่ต้องทำอะไร
            if (agendaType == lastAgendaType)
                return;

            // รีเซ็ตตำแหน่งก่อนเสมอ
            ResetLabelsToDefault();

            // ถ้า AgendaType = 1 ให้สลับตำแหน่ง (ไม่รวมงดออกเสียง)
            if (agendaType == 1)
            {
                // สลับตำแหน่ง label ที่แสดงค่า
                SwapLabels(label5, label9);   // งดออกเสียง (label5) <-> รวม (label9)
                SwapLabels(label11, label13); // % งดออกเสียง (label11) <-> % รวม (label13)
                
                // สลับตำแหน่ง label หัวข้อ
                SwapLabels(label23, label21); // หัวข้อ "งดออกเสียง" <-> หัวข้อ "รวม"
            }

            // บันทึก AgendaType ปัจจุบัน
            lastAgendaType = agendaType;
        }

        private void SwapLabels(Label label1, Label label2)
        {
            Point tempLocation = label1.Location;
            label1.Location = label2.Location;
            label2.Location = tempLocation;
        }

        private void ResetLabelsToDefault()
        {
            // รีเซ็ตตำแหน่งเริ่มต้นของทุก label
            label23.Location = new Point(503, 643);  // หัวข้อ "งดออกเสียง"
            label21.Location = new Point(503, 725);  // หัวข้อ "รวม"
            label5.Location = new Point(852, 643);   // ค่า งดออกเสียง
            label9.Location = new Point(852, 725);   // ค่า รวม
            label11.Location = new Point(1152, 643); // % งดออกเสียง
            label13.Location = new Point(1152, 725); // % รวม

            // รีเซ็ตค่า text
            label11.Text = "0.00%";
        }

        private void CalculateAndDisplayVoteSummary()
        {
            string agendaItem = HeaderLabel.Text; // ใช้ HeaderLabel เป็นหัวข้อวาระ
            if (string.IsNullOrEmpty(agendaItem))
            {
                return; // ไม่แสดง MessageBox เพื่อไม่รบกวนการรีเฟรช
            }

            long disapprove = 0, abstain = 0, invalidBallot = 0, totalVotes = 0;
            long qShareTotal = ParseLabelToLong(label15.Text);

            try
            {
                // 1) สร้าง DBConfig instance
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                // 2) ใช้ DbConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();
                    string voteQuery = @"
                SELECT VoteType, SUM(ShareCount) AS TotalShareCount
                FROM VoteResults
                WHERE AgendaItem = @AgendaItem
                GROUP BY VoteType";
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

                // ตรวจสอบ AgendaType
                int currentAgendaType = dtHeaders != null && dtHeaders.Rows.Count > currentAgendaIndex 
                    ? Convert.ToInt32(dtHeaders.Rows[currentAgendaIndex]["AgendaType"]) 
                    : 2; // Default เป็น Type 2

                long adjustedApprove;
                long totalVotesSum;

                // *** AgendaType = 1 → ไม่รวมงดออกเสียง ***
                if (currentAgendaType == 1)
                {
                    // คำนวณแบบไม่รวมงดออกเสียง
                    adjustedApprove = qShareTotal - (disapprove + invalidBallot);
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot;
                    
                    // ใส่ค่าลง label (label ถูกสลับแล้วโดย AdjustControlsForAgendaType)
                    label6.Text = disapprove.ToString("N0");        // ไม่เห็นด้วย
                    label8.Text = invalidBallot.ToString("N0");     // บัตรเสีย
                    label7.Text = adjustedApprove.ToString("N0");   // เห็นด้วย
                    label5.Text = abstain.ToString("N0");           // งดออกเสียง (สลับมาที่แถว "รวม")
                    label9.Text = totalVotesSum.ToString("N0");     // รวม (สลับมาที่แถว "งดออกเสียง")
                    
                    // เปอร์เซ็นต์
                    label10.Text = CalculatePercentage(disapprove, qShareTotal);
                    label12.Text = CalculatePercentage(invalidBallot, qShareTotal);
                    label18.Text = CalculatePercentage(adjustedApprove, qShareTotal);
                    label11.Text = CalculatePercentage(abstain, qShareTotal);  // % งดออกเสียง (สลับมาที่แถว "รวม")
                    label13.Text = "-";  // % รวม (สลับมาที่แถว "งดออกเสียง") แสดง "-"
                }
                // *** AgendaType = 2 → รวมงดออกเสียง ***
                else
                {
                    // คำนวณแบบรวมงดออกเสียง
                    adjustedApprove = qShareTotal - (disapprove + abstain + invalidBallot);
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot + abstain;
                    
                    // ใส่ค่าลง label (ตำแหน่งปกติ)
                    label6.Text = disapprove.ToString("N0");        // ไม่เห็นด้วย
                    label5.Text = abstain.ToString("N0");           // งดออกเสียง
                    label8.Text = invalidBallot.ToString("N0");     // บัตรเสีย
                    label7.Text = adjustedApprove.ToString("N0");   // เห็นด้วย
                    label9.Text = totalVotesSum.ToString("N0");     // รวม

                    // เปอร์เซ็นต์
                    label10.Text = CalculatePercentage(disapprove, qShareTotal);
                    label11.Text = CalculatePercentage(abstain, qShareTotal);
                    label12.Text = CalculatePercentage(invalidBallot, qShareTotal);
                    label18.Text = CalculatePercentage(adjustedApprove, qShareTotal);
                    
                    double totalPercentage = ParsePercentage(label18.Text) + ParsePercentage(label10.Text) + 
                                           ParsePercentage(label12.Text) + ParsePercentage(label11.Text);
                    label13.Text = $"{totalPercentage:F2}%";
                }

                System.Diagnostics.Debug.WriteLine($"AgendaType: {currentAgendaType}, Approve: {adjustedApprove}, Disapprove: {disapprove}, Abstain: {abstain}, Invalid: {invalidBallot}, Total: {totalVotesSum}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating votes: {ex.Message}");
            }
        }

        private long ParseLabelToLong(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
                
            if (text.EndsWith("%"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            text = text.Replace(",", ""); // เอา comma ออก
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
            if (string.IsNullOrEmpty(text))
                return 0;
                
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
            {
                // หยุด Timer ก่อนปิดฟอร์ม
                refreshTimer?.Stop();
                
                // Navigate to RegisterationDetail page
                this.Hide();
                RegisterationDetail regDetail = new RegisterationDetail();
                regDetail.Show();
                return;
            }
            
            if (currentAgendaIndex > 0)
            {
                currentAgendaIndex--;
                lastAgendaType = -1; // รีเซ็ตเพื่อบังคับให้ปรับตำแหน่งใหม่
                UpdateAgendaDisplay();
            }
            else
            {
                // หยุด Timer ก่อนปิดฟอร์ม
                refreshTimer?.Stop();
                
                // Navigate to RegisterationDetail page
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
                lastAgendaType = -1; // รีเซ็ตเพื่อบังคับให้ปรับตำแหน่งใหม่
                UpdateAgendaDisplay();
            }
            else
            {
                MessageBox.Show("นี่คือวาระสุดท้ายแล้ว");
            }
        }

        // เพิ่ม method สำหรับรีเฟรชหน้าปัจจุบัน
        private void RefreshCurrentPage()
        {
            try
            {
                // โหลดข้อมูลวาระใหม่
                LoadHeaderDataForNavigation();
                
                // โหลดจำนวนคนรวมใหม่
                LoadPeopleCountTotal();
                
                // ตรวจสอบว่ายังมีข้อมูลวาระหรือไม่
                if (dtHeaders != null && dtHeaders.Rows.Count > currentAgendaIndex)
                {
                    DataRow currentRow = dtHeaders.Rows[currentAgendaIndex];
                    
                    // อัพเดทข้อมูลพื้นฐาน
                    HeaderLabel.Text = currentRow["AgendaDisplay"].ToString();
                    
                    int agendaType = Convert.ToInt32(currentRow["AgendaType"]);
                    long qShareTotal = currentRow["qShareTotal"] == DBNull.Value ? 0 : Convert.ToInt64(currentRow["qShareTotal"]);
                    int peopleCountTotal = currentRow["peopleCountTotal"] == DBNull.Value ? 0 : Convert.ToInt32(currentRow["peopleCountTotal"]);
                    
                    // อัปเดต label ที่เกี่ยวข้อง
                    label15.Text = qShareTotal.ToString("N0");
                    label17.Text = peopleCountTotal.ToString();
                    
                    // *** สำคัญ: ไม่ต้อง reset lastAgendaType เพื่อไม่ให้สลับซ้ำ ***
                    // เรียก AdjustControlsForAgendaType โดยไม่ reset
                    // มันจะสลับก็ต่อเมื่อ AgendaType เปลี่ยนจริงๆ เท่านั้น
                    AdjustControlsForAgendaType(agendaType);
                    
                    // เรียกคำนวณและแสดงผลคะแนนใหม่
                    CalculateAndDisplayVoteSummary();
                    
                    System.Diagnostics.Debug.WriteLine($"Page refreshed at {DateTime.Now:HH:mm:ss} - Agenda: {HeaderLabel.Text}, Type: {agendaType}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing page: {ex.Message}");
            }
        }

        private void UpdateAgendaDisplay()
        {
            if (dtHeaders == null || dtHeaders.Rows.Count == 0)
                return;
            DataRow currentRow = dtHeaders.Rows[currentAgendaIndex];

            // นำ AgendaDisplay ไปแสดงใน HeaderLabel
            HeaderLabel.Text = currentRow["AgendaDisplay"].ToString();

            // อ่านค่า AgendaType, qShareTotal และ peopleCountTotal จาก DataRow
            int agendaType = Convert.ToInt32(currentRow["AgendaType"]);
            long qShareTotal = currentRow["qShareTotal"] == DBNull.Value ? 0 : Convert.ToInt64(currentRow["qShareTotal"]);
            int peopleCountTotal = currentRow["peopleCountTotal"] == DBNull.Value ? 0 : Convert.ToInt32(currentRow["peopleCountTotal"]);

            // อัปเดต label ที่เกี่ยวข้อง
            label15.Text = qShareTotal.ToString("N0");
            label17.Text = peopleCountTotal.ToString();

            // ปรับตำแหน่ง label ตาม AgendaType
            AdjustControlsForAgendaType(agendaType);

            // เรียกคำนวณและแสดงผลคะแนน
            CalculateAndDisplayVoteSummary();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // หยุด Timer เมื่อปิดฟอร์ม
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
