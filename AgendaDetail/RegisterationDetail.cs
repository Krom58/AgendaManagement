using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AgendaDetail
{
    public partial class RegisterationDetail : Form
    {
        private Timer refreshTimer; // Add a Timer
        public RegisterationDetail()
        {
            InitializeComponent();
        }

        private void RegisterationDetail_Load(object sender, EventArgs e)
        {
            LoadRegistrationData();
            InitializeTimer();
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

                using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
                {
                    conn.Open();

                    // Load data from SelfRegistration
                    string querySelf = "SELECT COUNT(*) AS PeopleCount, SUM(CONVERT(BIGINT, ShareCount)) AS ShareCount FROM SelfRegistration";
                    using (SqlCommand cmd = new SqlCommand(querySelf, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountSelf = reader["PeopleCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount"]);
                                shareCountSelf = reader["ShareCount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ShareCount"]);
                            }
                        }
                    }

                    // Load data from ProxyRegistration
                    string queryProxy = "SELECT COUNT(*) AS PeopleCount, SUM(CONVERT(BIGINT, ShareCount)) AS ShareCount FROM ProxyRegistration";
                    using (SqlCommand cmd = new SqlCommand(queryProxy, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                peopleCountProxy = reader["PeopleCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PeopleCount"]);
                                shareCountProxy = reader["ShareCount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ShareCount"]);
                            }
                        }
                    }

                    // Load total shares from PersonData
                    string queryTotalShares = "SELECT SUM(CONVERT(BIGINT, q_share)) AS TotalShares FROM PersonData";
                    using (SqlCommand cmd = new SqlCommand(queryTotalShares, conn))
                    {
                        totalShares = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                    }
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

                // Calculate additional percentages
                double percentTotalShares = totalShares > 0 ? (double)(shareCountSelf + shareCountProxy) / totalShares * 100 : 0;
                double percentDifference = percentTotal - percentTotalShares;

                label18.Text = percentTotalShares.ToString("F2") + "%";
                label19.Text = percentDifference.ToString("F2") + "%";
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
