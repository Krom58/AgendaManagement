using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Work1
{
    public partial class Main: Form
    {
        private readonly string _iniPath;
        public Main()
        {
            InitializeComponent();
            _iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
        }
        private void ImportExcel_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            ImportExcel ImportExcel = new ImportExcel(this);

            // แสดง Form2
            ImportExcel.Show();
        }

        private void CheckDB_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            CheckDB CheckDB = new CheckDB(this);

            // แสดง Form2
            CheckDB.Show();
        }

        private void Agenda_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            Agenda Agenda = new Agenda(this);

            // แสดง Form2
            Agenda.Show();
        }

        private void RegistrationViewer_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            RegistrationViewer RegistrationViewer = new RegistrationViewer(this);

            // แสดง Form2
            RegistrationViewer.Show();
        }

        private void RegistrationSummary_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            RegistrationSummary RegistrationSummary = new RegistrationSummary(this);

            // แสดง Form2
            RegistrationSummary.Show();
        }

        private void AgendaSummary_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            AgendaSummary AgendaSummary = new AgendaSummary(this);

            // แสดง Form2
            AgendaSummary.Show();
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            ImportExcel.Visible = !ImportExcel.Visible;
            Agenda.Visible = !Agenda.Visible;
            Config.Visible = !Config.Visible;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void Config_Click(object sender, EventArgs e)
        {
            // ขอ path ของโฟลเดอร์ที่ exe รันอยู่
            string exeFolder = AppDomain.CurrentDomain.BaseDirectory;

            // ขึ้นไปสามระดับ (bin\Debug\net7.0-windows → AgendaManagement)
            string projectFolder = Path.GetFullPath(Path.Combine(exeFolder, @"..\..\..\"));

            // ผนวกชื่อไฟล์ ini ในโฟลเดอร์โปรเจกต์
            string iniFilePath = Path.Combine(projectFolder, "database_config.ini");

            if (!File.Exists(_iniPath))
            {
                MessageBox.Show(
                    $"ไม่พบไฟล์ database_config.ini ที่\r\n{_iniPath}",
                    "ข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                // เปิด Notepad ให้แก้ไฟล์
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = $"\"{_iniPath}\"",
                    UseShellExecute = true
                };
                var proc = System.Diagnostics.Process.Start(psi);

                // รอให้ Notepad ปิดก่อน แล้ว restart แอป
                proc.WaitForExit();

                // รีสตาร์ททั้งโปรแกรม
                Application.Restart();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"เกิดข้อผิดพลาดในการเปิดไฟล์: {ex.Message}",
                    "ข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
