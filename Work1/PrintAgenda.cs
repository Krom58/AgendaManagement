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
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Drawing.Configuration;

namespace Work1
{
    public partial class PrintAgenda: Form
    {
        private string connectionString = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";
        private int personId;
        public PrintAgenda(int Id)
        {
            InitializeComponent();
            personId = Id;
        }
        // Constructor ที่รับ Form1 เข้ามา
        public PrintAgenda(Main main)
        {
            InitializeComponent();
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
            // กำหนดขนาดและตำแหน่งของคอลัมน์
            float columnWidth = e.MarginBounds.Width / 2;
            float columnHeight = e.MarginBounds.Height;
            float leftColumnX = e.MarginBounds.Left;
            float rightColumnX = e.MarginBounds.Left + columnWidth;
            float yPosition = e.MarginBounds.Top;
            float templateHeight = 200; // กำหนดความสูงของแต่ละ template
            float lineSpacing = 0; // กำหนดระยะห่างระหว่างบรรทัด

            // แบ่งข้อความใน RichTextBox เป็นสองคอลัมน์
            string[] templates = richTextBoxTemplate.Text.Split(new[] { "-----------------------------------------------------" }, StringSplitOptions.None);
            List<string> leftColumnTemplates = new List<string>();
            List<string> rightColumnTemplates = new List<string>();

            for (int i = 0; i < templates.Length; i++)
            {
                if (i % 2 == 0)
                {
                    leftColumnTemplates.Add(templates[i]);
                }
                else
                {
                    rightColumnTemplates.Add(templates[i]);
                }
            }

            // วาดข้อความในคอลัมน์ซ้าย
            foreach (string template in leftColumnTemplates)
            {
                RectangleF rect = new RectangleF(leftColumnX, yPosition, columnWidth, templateHeight);
                e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

                // วัดขนาดของข้อความ
                SizeF textSize = e.Graphics.MeasureString(template, new System.Drawing.Font("Angsana New", 12));
                // คำนวณตำแหน่งการวาดเพื่อให้อยู่กึ่งกลาง
                float textX = rect.X + (rect.Width - textSize.Width) / 2;
                float textY = rect.Y + (rect.Height - textSize.Height) / 2;

                e.Graphics.DrawString(template, new System.Drawing.Font("Angsana New", 12), Brushes.Black, new PointF(textX, textY));
                yPosition += templateHeight + lineSpacing; // เพิ่มระยะห่างระหว่าง template
            }

            // รีเซ็ตตำแหน่ง y สำหรับคอลัมน์ขวา
            yPosition = e.MarginBounds.Top;

            // วาดข้อความในคอลัมน์ขวา
            foreach (string template in rightColumnTemplates)
            {
                RectangleF rect = new RectangleF(rightColumnX, yPosition, columnWidth, templateHeight);
                e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

                // วัดขนาดของข้อความ
                SizeF textSize = e.Graphics.MeasureString(template, new System.Drawing.Font("Angsana New", 12));
                // คำนวณตำแหน่งการวาดเพื่อให้อยู่กึ่งกลาง
                float textX = rect.X + (rect.Width - textSize.Width) / 2;
                float textY = rect.Y + (rect.Height - textSize.Height) / 2;

                e.Graphics.DrawString(template, new System.Drawing.Font("Angsana New", 12), Brushes.Black, new PointF(textX, textY));
                yPosition += templateHeight + lineSpacing; // เพิ่มระยะห่างระหว่าง template
            }
        }

        private void PrintAgenda_Load(object sender, EventArgs e)
        {
            LoadPrintTemplate();
        }

        private void LoadPrintTemplate()
        {
            // กำหนดตัวแปรสำหรับข้อมูลผู้ลงทะเบียนที่เลือก
            string fullName = "";
            string q_share = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ดึงข้อมูลจาก PersonData สำหรับ PersonID ที่เลือก
                string queryPerson = @"
            SELECT CONCAT(n_first, ' ', n_last) AS FullName, q_share
            FROM PersonData
            WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(queryPerson, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", personId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fullName = reader["FullName"].ToString();
                            q_share = reader["q_share"].ToString();
                        }
                    }
                }

                // ดึงข้อมูล Template จาก HeaderTemplate ทั้งหมด
                string queryTemplate = @"
            SELECT MeetingNumber, AgendaNumber, AgendaTitle, FixedContent
            FROM HeaderTemplate
            ORDER BY HeaderID";
                using (SqlCommand cmd = new SqlCommand(queryTemplate, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder documentTemplate = new StringBuilder();
                        while (reader.Read())
                        {
                            string meetingNumber = reader["MeetingNumber"].ToString();
                            string agendaNumber = reader["AgendaNumber"].ToString();
                            string agendaTitle = reader["AgendaTitle"].ToString();
                            string fixedContent = reader["FixedContent"].ToString();

                            // ประกอบ Block Template สำหรับแต่ละแถวใน HeaderTemplate
                            documentTemplate.AppendLine($"บัตรลงคะแนนการประชุมสามัญผู้ถือหุ้น ครั้งที่ {meetingNumber}");
                            documentTemplate.AppendLine($"วาระที่ {agendaNumber}    {agendaTitle}");
                            documentTemplate.AppendLine($"ชื่อ: {fullName}");
                            documentTemplate.AppendLine($"จำนวนหุ้น: {q_share}");
                            documentTemplate.AppendLine("   [  ]เห็นด้วย           [  ]ไม่เห็นด้วย         [  ]งดออกเสียง");
                            documentTemplate.AppendLine("     (Approved)         (Disapproved)     (Abstained)");
                            documentTemplate.AppendLine(" ลงชื่อ __________________ ผู้ถือหุ้น");
                            documentTemplate.Append("-----------------------------------------------------");
                        }
                        richTextBoxTemplate.Text = documentTemplate.ToString().Trim();
                    }
                }
            }
        }





        private void comboBoxPerson_TextChanged(object sender, EventArgs e)
        {
            
        }

        
    }
}