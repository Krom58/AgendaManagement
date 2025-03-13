using DocumentFormat.OpenXml.Presentation;
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
using System.Drawing.Printing;

namespace Work1
{
    public partial class PrintAgenda: Form
    {
        private string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";
        public PrintAgenda()
        {
            InitializeComponent();
        }

        private Main _Main;

        // Constructor ที่รับ Form1 เข้ามา
        public PrintAgenda(Main main)
        {
            InitializeComponent();
            _Main = main;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            // เรียกให้ Form1 กลับมาแสดง
            _Main.Show();

            // ปิด Form2 เพื่อกลับไปใช้งาน Form1
            this.Close();
        }

        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            if (comboBoxPerson.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกบุคคลจากรายการ");
                return;
            }

            // ดึงข้อมูลจาก ComboBox (ใช้ DataRowView ในกรณีที่ผูก DataSource ด้วย DataTable)
            DataRowView drv = comboBoxPerson.SelectedItem as DataRowView;
            if (drv != null)
            {
                string personName = drv["n_first"].ToString();
                string shareCount = drv["q_share"].ToString();

                // เรียก LoadTemplates พร้อมกับส่งค่าพารามิเตอร์
                LoadTemplates(personName, shareCount);
            }
        }
        private void PrintDoc_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDoc;
            previewDialog.ShowDialog();
        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // วาดข้อความจาก richTextBoxPreview ลงบนเอกสาร A4
            e.Graphics.DrawString(richTextBoxTemplate.Text, new System.Drawing.Font("Arial", 12), Brushes.Black, new PointF(50, 50));
        }

        private void PrintAgenda_Load(object sender, EventArgs e)
        {
            comboBoxPerson.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxPerson.AutoCompleteSource = AutoCompleteSource.CustomSource;
            LoadPersons();
        }
        private void LoadPersons()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, n_first, n_last, q_share FROM PersonData";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dt.Columns.Add("FullName", typeof(string), "n_first + ' ' + n_last");

                        comboBoxPerson.DataSource = dt;
                        comboBoxPerson.DisplayMember = "FullName";
                        comboBoxPerson.ValueMember = "Id";

                        // สร้าง AutoCompleteSource
                        AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            autoCompleteCollection.Add(row["FullName"].ToString());
                        }
                        comboBoxPerson.AutoCompleteCustomSource = autoCompleteCollection;
                    }
                }
            }
        }
        private void LoadTemplates(string personName, string shareCount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // ดึงข้อมูลที่ต้องการ (ไม่รวม FixedContent หากต้องการปรับแต่งเพิ่มเติม)
                string query = "SELECT MeetingNumber, AgendaNumber, AgendaTitle, FixedContent FROM HeaderTemplate ORDER BY HeaderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string documentTemplate = "";
                        while (reader.Read())
                        {
                            string meetingNumber = reader["MeetingNumber"].ToString();
                            string agendaNumber = reader["AgendaNumber"].ToString();
                            string agendaTitle = reader["AgendaTitle"].ToString();
                            string fixedContent = reader["FixedContent"].ToString();

                            // แทนที่ Placeholder {n_first} และ {q_share} ใน FixedContent
                            fixedContent = fixedContent.Replace("{n_first}", personName).Replace("{q_share}", shareCount);

                            documentTemplate += $@"
บัตรลงคะแนนการประชุมสามัญผู้ถือหุ้น ครั้งที่ {meetingNumber}

วาระที่ {agendaNumber}    {agendaTitle}

{fixedContent}
-----------------------------------------------------
";
                        }
                        richTextBoxTemplate.Text = documentTemplate;
                    }
                }
            }
        }


        private void comboBoxPerson_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
