using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Work1;

namespace AgendaDetail
{
    public partial class RegisterationDetail : Form
    {
        private Timer refreshTimer; // Add a Timer
        public RegisterationDetail()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void RegisterationDetail_Load(object sender, EventArgs e)
        {
            LoadRegistrationData();
        }
        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 3000; // Set the interval to 3 seconds
            refreshTimer.Tick += new EventHandler(RefreshTimer_Tick);
            refreshTimer.Start(); // Start the timer
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadRegistrationData(); // Refresh the data
        }
        private void LoadRegistrationData()
        {
            try
            {
                int peopleCountSelf = 0, peopleCountProxy = 0;
                long shareCountSelf = 0, shareCountProxy = 0, totalShares = 0;

                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    // Load data from SelfRegistration
                    string querySelf = "SELECT COUNT(*) AS PeopleCount_Self, SUM(ShareCount::BIGINT) AS QShare_Self FROM SelfRegistration";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = querySelf;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountSelf = reader["PeopleCount_Self"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Self"]);
                                shareCountSelf = reader["QShare_Self"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Self"]);
                            }
                        }
                    }

                    // Debug output
                    Debug.WriteLine($"SelfRegistration - PeopleCount: {peopleCountSelf}, ShareCount: {shareCountSelf}");

                    // Load data from ProxyRegistration
                    string queryProxy = "SELECT COUNT(*) AS PeopleCount_Proxy, SUM(ShareCount::BIGINT) AS QShare_Proxy FROM ProxyRegistration";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = queryProxy;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountProxy = reader["PeopleCount_Proxy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount_Proxy"]);
                                shareCountProxy = reader["QShare_Proxy"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QShare_Proxy"]);
                            }
                        }
                    }

                    // Debug output
                    Debug.WriteLine($"ProxyRegistration - PeopleCount: {peopleCountProxy}, ShareCount: {shareCountProxy}");

                    // Load total shares from PersonData
                    string queryTotalShares = "SELECT SUM(q_share::BIGINT) AS TotalQShare FROM PersonData";
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = queryTotalShares;
                        totalShares = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                    }

                    // Debug output
                    Debug.WriteLine($"PersonData - TotalShares: {totalShares}");
                }

                // Set label texts
                label7.Text = peopleCountSelf.ToString();
                label8.Text = peopleCountProxy.ToString();
                label9.Text = shareCountSelf.ToString("N0");
                label10.Text = shareCountProxy.ToString("N0");
                label13.Text = (peopleCountSelf + peopleCountProxy).ToString();
                label14.Text = (shareCountSelf + shareCountProxy).ToString("N0");
                label17.Text = totalShares.ToString("N0");

                // Calculate percentages
                double percentSelf = totalShares > 0 ? (double)shareCountSelf / totalShares * 100 : 0;
                double percentProxy = totalShares > 0 ? (double)shareCountProxy / totalShares * 100 : 0;
                double percentTotal = percentSelf + percentProxy;

                label11.Text = percentSelf.ToString("F2") + "%";
                label12.Text = percentProxy.ToString("F2") + "%";
                label15.Text = percentTotal.ToString("F2") + "%";

                double qShareTotalValue = double.Parse(label14.Text, System.Globalization.NumberStyles.AllowThousands);
                double qShareGlobal = double.Parse(label17.Text, System.Globalization.NumberStyles.AllowThousands);
                double percentQShare = qShareGlobal > 0 ? (qShareTotalValue / qShareGlobal) * 100 : 0;
                label18.Text = percentQShare.ToString("F2") + "%";

                double percentDifference = percentTotal - percentQShare;
                label19.Text = percentDifference.ToString("F2") + "%";

                // Debug output
                Debug.WriteLine($"Percentages - Self: {percentSelf}%, Proxy: {percentProxy}%, Total: {percentTotal}%");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading registration data: " + ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            AgendaDetail agendaDetailForm = new AgendaDetail();
            agendaDetailForm.Show();
            this.Hide();
        }
    }
}
