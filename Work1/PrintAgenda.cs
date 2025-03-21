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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Work1
{
    public partial class PrintAgenda: Form
    {
        private int personId;
        private string attendChoice; // เก็บ attendChoice จากฟอร์มอื่น
        public PrintAgenda(int Id, string attendChoice)
        {
            InitializeComponent();
            this.personId = Id;
            this.attendChoice = attendChoice; // assign ค่าที่ส่งมาจากฟอร์มอื่น
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
        private int currentIndex = 0; // ตัวแปรเก็บข้อความที่พิมพ์ถึงอันที่เท่าไร
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;

            float columnWidth = 107.45f;
            float rowHeight = 69.7f;
            int rows = 4;
            int cols = 2;

            // แบ่งข้อความใน RichTextBox
            string[] cellTexts = richTextBoxTemplate.Text.Split(
                new string[] { "-----------------------------------------------------" },
                StringSplitOptions.RemoveEmptyEntries
            );

            // วาดข้อความ 8 ช่องต่อหน้า
            int itemsPerPage = 8; // 2 คอลัมน์ × 4 แถว = 8 ช่องต่อหน้า
            int cellIndex = currentIndex; // เริ่มต้นจาก currentIndex
            int maxIndex = Math.Min(currentIndex + itemsPerPage, cellTexts.Length);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;        // ชิดซ้าย
            sf.LineAlignment = StringAlignment.Center;  // กึ่งกลางแนวตั้ง

            float indent = 10f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (cellIndex >= maxIndex) break;

                    float x = col * columnWidth;
                    float y = row * rowHeight;
                    RectangleF rect = new RectangleF(x, y, columnWidth, rowHeight);

                    RectangleF textRect = new RectangleF(rect.X + indent, rect.Y, rect.Width - indent, rect.Height);

                    using (System.Drawing.Font font = new System.Drawing.Font("Angsana New", 12))
                    {
                        e.Graphics.DrawString(cellTexts[cellIndex], font, Brushes.Black, textRect, sf);
                    }

                    cellIndex++;
                }

                if (cellIndex >= maxIndex) break;
            }

            // ตรวจสอบว่ามีข้อความเหลือหรือไม่
            if (cellIndex < cellTexts.Length)
            {
                currentIndex = cellIndex; // อัปเดต currentIndex เพื่อพิมพ์หน้าถัดไป
                e.HasMorePages = true;   // แจ้งให้ระบบพิมพ์หน้าถัดไป
            }
            else
            {
                currentIndex = 0;       // รีเซ็ต currentIndex หลังจากพิมพ์ครบทุกหน้า
                e.HasMorePages = false; // แจ้งว่าพิมพ์เสร็จแล้ว
            }
        }

        private void PrintAgenda_Load(object sender, EventArgs e)
        {
            LoadPrintTemplate();
        }

        private void LoadPrintTemplate()
        {
            string fullName = "";
            string q_share = "";
            string identifier = "";

            using (SqlConnection conn = new SqlConnection(DBConfig.connectionString))
            {
                conn.Open();

                // ดึงข้อมูลจาก PersonData สำหรับ personId
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

                // ใช้ attendChoice ที่ส่งมาจากหน้าลงทะเบียน (printAgenda รับ attendChoice แล้วเก็บในตัวแปร attendChoice)
                if (attendChoice == "มาเอง")
                {
                    // ดึง Identifier จากตาราง SelfRegistration สำหรับ personId
                    // (สมมุติว่าตาราง SelfRegistration มีคอลัมน์ PersonID ด้วย)
                    string queryReg = "SELECT Identifier FROM SelfRegistration WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(queryReg, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", personId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            identifier = result.ToString();
                    }
                }
                else if (attendChoice == "มอบฉันทะ")
                {
                    // ดึง Identifier จากตาราง ProxyRegistration สำหรับ personId
                    string queryReg = "SELECT Identifier FROM ProxyRegistration WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(queryReg, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", personId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            identifier = result.ToString();
                    }
                }

                // ดึงข้อมูล Template จากตาราง HeaderTemplate ทั้งหมด
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
                            documentTemplate.AppendLine($"บัตรลงคะแนนการประชุมสามัญผู้ถือหุ้น ครั้งที่ {meetingNumber}              {identifier}");
                            documentTemplate.AppendLine($"วาระที่ {agendaNumber}    {agendaTitle}");
                            documentTemplate.AppendLine($"ชื่อ: {fullName}");
                            documentTemplate.AppendLine($"จำนวนหุ้น: {q_share}");
                            documentTemplate.AppendLine("   [  ] เห็นด้วย           [  ] ไม่เห็นด้วย         [  ] งดออกเสียง");
                            documentTemplate.AppendLine("     (Approved)         (Disapproved)     (Abstained)");
                            documentTemplate.AppendLine("ลงชื่อ __________________ ผู้ถือหุ้น");
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