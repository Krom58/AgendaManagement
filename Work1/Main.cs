using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Work1
{
    public partial class Main: Form
    {
        public Main()
        {
            InitializeComponent();
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
    }
}
