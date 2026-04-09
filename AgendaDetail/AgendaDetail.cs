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
            
            System.Diagnostics.Debug.WriteLine("[Timer] Auto-refresh started (every 3 seconds)");
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
                System.Diagnostics.Debug.WriteLine($"[LoadPeopleCountTotal] Error: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"[LoadHeaderDataForNavigation] Error: {ex.Message}");
            }
        }

        private void AdjustControlsForAgendaType(int agendaType)
        {
            // ถ้า AgendaType ไม่เปลี่ยน ไม่ต้องทำอะไร
            if (agendaType == lastAgendaType)
                return;

            // รีเซ็ตตำแหน่งก่อนเสมอ
            ResetLabelsToDefault();

            // ถ้า AgendaType = 1 ให้สลับตำแหน่ง
            if (agendaType == 1)
            {
                // สลับตำแหน่ง label ที่แสดงค่า
                SwapLabels(label5, label9);   // งดออกเสียง (label5) <-> รวม (label9)
                SwapLabels(label11, label13); // % งดออกเสียง (label11) <-> % รวม (label13)
                
                // สลับตำแหน่ง label หัวข้อ
                SwapLabels(label23, label21); // หัวข้อ "งดออกเสียง" <-> หัวข้อ "รวม"
                
                // ซ่อนเปอร์เซ็นต์งดออกเสียงสำหรับ AgendaType 1
                label11.Text = "-";
                
                System.Diagnostics.Debug.WriteLine("[AdjustControls] Swapped to AgendaType 1 layout");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[AdjustControls] Reset to AgendaType 2 layout");
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
            string agendaItem = HeaderLabel.Text; // ใช้ TextBox1 เป็นหัวข้อวาระ
            if (string.IsNullOrEmpty(agendaItem))
            {
                return; // ไม่แสดง MessageBox เพื่อไม่รบกวนการ refresh
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

                long adjustedApprove;
                long totalVotesSum;
                // ถ้า AgendaType = 1 (แบบที่ไม่รวมงดออกเสียงในการคำนวณ)
                if (dtHeaders.Rows[currentAgendaIndex]["AgendaType"].ToString() == "1")
                {
                    adjustedApprove = qShareTotal - (disapprove + invalidBallot);
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot;
                    
                    // ตั้งค่า label ตามตำแหน่งปัจจุบัน (หลังการสลับแล้ว)
                    label6.Text = disapprove.ToString("N0");
                    label5.Text = totalVotesSum.ToString("N0");  // label5 จะอยู่ที่ตำแหน่ง "รวม" หลังสลับ
                    label8.Text = invalidBallot.ToString("N0");
                    label7.Text = adjustedApprove.ToString("N0");
                    label9.Text = abstain.ToString("N0");        // label9 จะอยู่ที่ตำแหน่ง "งดออกเสียง" หลังสลับ
                    
                    label10.Text = CalculatePercentage(disapprove, qShareTotal);
                    label11.Text = "-";                          // label11 จะอยู่ที่ตำแหน่ง "% รวม" หลังสลับ (แสดง -)
                    label12.Text = CalculatePercentage(invalidBallot, qShareTotal);
                    label18.Text = CalculatePercentage(adjustedApprove, qShareTotal);
                    label13.Text = CalculatePercentage(abstain, qShareTotal); // label13 จะอยู่ที่ตำแหน่ง "% งดออกเสียง" หลังสลับ
                }
                else // AgendaType = 2 (แบบปกติ รวมงดออกเสียง)
                {
                    adjustedApprove = qShareTotal - (disapprove + abstain + invalidBallot);
                    totalVotesSum = adjustedApprove + disapprove + invalidBallot + abstain;
                    
                    label6.Text = disapprove.ToString("N0");
                    label5.Text = abstain.ToString("N0");
                    label8.Text = invalidBallot.ToString("N0");
                    label7.Text = adjustedApprove.ToString("N0");
                    label9.Text = totalVotesSum.ToString("N0");

                    label10.Text = CalculatePercentage(disapprove, qShareTotal);
                    label11.Text = CalculatePercentage(abstain, qShareTotal);
                    label12.Text = CalculatePercentage(invalidBallot, qShareTotal);
                    label18.Text = CalculatePercentage(adjustedApprove, qShareTotal);
                    
                    double totalPercentage = ParsePercentage(label18.Text) + ParsePercentage(label10.Text) + 
                                           ParsePercentage(label12.Text) + ParsePercentage(label11.Text);
                    label13.Text = $"{totalPercentage:F2}%";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CalculateVote] Error: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine("[Timer] Stopped before navigation");
                
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
                System.Diagnostics.Debug.WriteLine($"[Navigation] Back to agenda {currentAgendaIndex + 1}");
            }
            else
            {
                // หยุด Timer ก่อนปิดฟอร์ม
                refreshTimer?.Stop();
                System.Diagnostics.Debug.WriteLine("[Timer] Stopped before navigation");
                
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
                System.Diagnostics.Debug.WriteLine($"[Navigation] Forward to agenda {currentAgendaIndex + 1}");
            }
            else
            {
                MessageBox.Show("นี่คือวาระสุดท้ายแล้ว");
            }
        }

        // เพิ่ม method สำหรับรีเฟรชหน้าปัจจุบัน (ปรับปรุงแล้ว)
        private void RefreshCurrentPage()
        {
            try
            {
                // โหลดข้อมูลวาระใหม่
                LoadHeaderDataForNavigation();
                
                // โหลดจำนวนคนรวมใหม่
                LoadPeopleCountTotal();
                
                // ตรวจสอบว่า currentAgendaIndex ยังอยู่ในช่วงที่ถูกต้องหรือไม่
                if (dtHeaders != null && dtHeaders.Rows.Count > 0)
                {
                    // ถ้า index เกินขนาด ให้ปรับเป็น index สุดท้าย
                    if (currentAgendaIndex >= dtHeaders.Rows.Count)
                    {
                        currentAgendaIndex = dtHeaders.Rows.Count - 1;
                        lastAgendaType = -1; // รีเซ็ตเพื่อปรับตำแหน่ง label ใหม่
                        System.Diagnostics.Debug.WriteLine($"[Refresh] Index adjusted to {currentAgendaIndex}");
                    }
                    
                    // อัพเดทการแสดงผลวาระ (ไม่รีเซ็ต lastAgendaType เพื่อไม่ให้สลับซ้ำ)
                    UpdateAgendaDisplay();
                    
                    // แสดง debug info
                    System.Diagnostics.Debug.WriteLine(
                        $"[Refresh] {DateTime.Now:HH:mm:ss} | " +
                        $"Agenda: {currentAgendaIndex + 1}/{dtHeaders.Rows.Count} | " +
                        $"Type: {dtHeaders.Rows[currentAgendaIndex]["AgendaType"]} | " +
                        $"Display: {HeaderLabel.Text}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[Refresh] Warning: No agenda data available");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Refresh] Error: {ex.Message}");
                // ไม่แสดง MessageBox เพื่อไม่ให้รบกวนผู้ใช้ทุก 3 วินาที
            }
        }

        private void UpdateAgendaDisplay()
        {
            if (dtHeaders == null || dtHeaders.Rows.Count == 0)
                return;
                
            if (currentAgendaIndex >= dtHeaders.Rows.Count)
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

            // ปรับตำแหน่ง label ตาม AgendaType (จะทำงานต่อเมื่อ AgendaType เปลี่ยน)
            AdjustControlsForAgendaType(agendaType);

            // เรียกคำนวณและแสดงผลคะแนน (ใช้ฟังก์ชันเดิม)
            CalculateAndDisplayVoteSummary();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // หยุด Timer เมื่อปิดฟอร์ม
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            System.Diagnostics.Debug.WriteLine("[Timer] Stopped and disposed");
            base.OnFormClosed(e);
        }
    }
}
