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

                // 1) สร้าง DBConfig instance
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfigDetail(iniPath);

                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    // 2) หาชนิดฐานข้อมูลจาก config
                    string dbType = dbcfg.Config.Type.ToLower();

                    // 3) กำหนด expression สำหรับ CAST ShareCount
                    string castShareCount;
                    switch (dbType)
                    {
                        case "postgresql":
                            castShareCount = "CAST(ShareCount AS BIGINT)";
                            break;
                        case "mysql":
                        case "mariadb":
                            castShareCount = "CAST(ShareCount AS SIGNED)";
                            break;
                        case "mssql":
                            castShareCount = "CAST(ShareCount AS BIGINT)";
                            break;
                        default:
                            throw new NotSupportedException($"Database type '{dbType}' not supported");
                    }

                    // 4) กำหนด expression สำหรับ CAST q_share
                    string castQShare;
                    switch (dbType)
                    {
                        case "postgresql":
                            castQShare = "CAST(q_share AS BIGINT)";
                            break;
                        case "mysql":
                        case "mariadb":
                            castQShare = "CAST(q_share AS SIGNED)";
                            break;
                        case "mssql":
                            castQShare = "CAST(q_share AS BIGINT)";
                            break;
                        default:
                            throw new NotSupportedException($"Database type '{dbType}' not supported");
                    }

                    // --- Load SelfRegistration ---
                    string querySelf = $@"
                SELECT 
                    COUNT(*) AS PeopleCount_Self,
                    SUM({castShareCount}) AS QShare_Self
                FROM SelfRegistration";

                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = querySelf;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountSelf = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                shareCountSelf = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                            }
                        }
                    }

                    // --- Load ProxyRegistration ---
                    string queryProxy = $@"
                SELECT 
                    COUNT(*) AS PeopleCount_Proxy,
                    SUM({castShareCount}) AS QShare_Proxy
                FROM ProxyRegistration";

                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = queryProxy;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountProxy = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                shareCountProxy = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);
                            }
                        }
                    }

                    // --- Load PersonData total shares ---
                    string queryTotalShares = $@"
                SELECT SUM({castQShare}) AS TotalQShare
                FROM PersonData";

                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = queryTotalShares;
                        object result = cmd.ExecuteScalar();
                        totalShares = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt64(result);
                    }
                }

                // --- อัปเดต UI ---
                label7.Text = peopleCountSelf.ToString();
                label8.Text = peopleCountProxy.ToString();
                label9.Text = shareCountSelf.ToString("N0");
                label10.Text = shareCountProxy.ToString("N0");
                label13.Text = (peopleCountSelf + peopleCountProxy).ToString();
                label14.Text = (shareCountSelf + shareCountProxy).ToString("N0");
                label17.Text = totalShares.ToString("N0");

                // คำนวณเปอร์เซ็นต์
                double percentSelf = totalShares > 0 ? (double)shareCountSelf / totalShares * 100 : 0;
                double percentProxy = totalShares > 0 ? (double)shareCountProxy / totalShares * 100 : 0;
                double percentTotal = percentSelf + percentProxy;

                label11.Text = $"{percentSelf:F2}%";
                label12.Text = $"{percentProxy:F2}%";
                label15.Text = $"{percentTotal:F2}%";

                double qShareTotalValue = double.Parse(label14.Text, System.Globalization.NumberStyles.AllowThousands);
                double qShareGlobal = double.Parse(label17.Text, System.Globalization.NumberStyles.AllowThousands);
                double percentQShare = qShareGlobal > 0 ? (qShareTotalValue / qShareGlobal) * 100 : 0;
                label18.Text = $"{percentQShare:F2}%";

                double percentDifference = percentTotal - percentQShare;
                label19.Text = $"{percentDifference:F2}%";

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
