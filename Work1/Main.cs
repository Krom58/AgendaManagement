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

            // ซ่อน Form1 ไว้เพื่อให้เห็นเฉพาะ Form2
            this.Hide();
        }

        private void CheckDB_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            CheckDB CheckDB = new CheckDB(this);

            // แสดง Form2
            CheckDB.Show();

            // ซ่อน Form1 ไว้เพื่อให้เห็นเฉพาะ Form2
            this.Hide();
        }

        private void Agenda_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            Agenda Agenda = new Agenda(this);

            // แสดง Form2
            Agenda.Show();

            // ซ่อน Form1 ไว้เพื่อให้เห็นเฉพาะ Form2
            this.Hide();
        }

        private void PrintAgenda_Click(object sender, EventArgs e)
        {
            // สร้างอินสแตนซ์ของ Form2 พร้อมส่ง this (Form1) ไปใน constructor
            PrintAgenda PrintAgenda = new PrintAgenda(this);

            // แสดง Form2
            PrintAgenda.Show();

            // ซ่อน Form1 ไว้เพื่อให้เห็นเฉพาะ Form2
            this.Hide();
        }
    }
}
