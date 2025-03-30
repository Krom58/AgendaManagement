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
    public partial class PrintAgenda : Form
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
            // รีเซ็ต currentIndex ที่นี่เท่านั้น
            currentIndex = 0;

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);

            // ตั้งค่าให้ใช้เครื่องพิมพ์เริ่มต้นของระบบ
            printDoc.PrinterSettings.PrinterName = new PrinterSettings().PrinterName;

            // ตรวจสอบความพร้อมของเครื่องพิมพ์
            if (!printDoc.PrinterSettings.IsValid)
            {
                MessageBox.Show("ไม่พบเครื่องพิมพ์เริ่มต้น กรุณาตรวจสอบการตั้งค่าเครื่องพิมพ์ในระบบ");
                return;
            }

            // สั่งพิมพ์โดยตรง
            try
            {
                printDoc.Print();
                MessageBox.Show("พิมพ์สำเร็จ");
                this.Close(); // ปิดฟอร์ม PrintAgenda
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการพิมพ์: {ex.Message}");
            }
        }
        private int currentIndex = 0; // ตัวแปรเก็บข้อความที่พิมพ์ถึงอันที่เท่าไร
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;

            float columnWidth = 107.45f;
            float rowHeight = 71f;
            int rows = 4;
            int cols = 2;

            // แบ่งข้อความใน RichTextBox โดยลบข้อความว่างออกด้วย RemoveEmptyEntries
            string[] allCells = richTextBoxTemplate.Text.Split(
                new string[] { "-----------------------------------------------------" },
                StringSplitOptions.RemoveEmptyEntries
            );

            // หากต้องการตรวจสอบว่ามี element ว่างหรือไม่ สามารถกรองออกอีกที:
            List<string> cellTexts = new List<string>();
            foreach (var cell in allCells)
            {
                if (!string.IsNullOrWhiteSpace(cell))
                    cellTexts.Add(cell.Trim());
            }

            int maxIndex = cellTexts.Count; // จำนวนข้อความที่แท้จริง

            // กำหนดตัวแปร StringFormat สำหรับการจัดวางข้อความ
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            sf.FormatFlags |= StringFormatFlags.NoClip;
            float indent = 7.5f;

            // บันทึกค่า currentIndex เริ่มต้นของหน้าปัจจุบัน เพื่อใช้ตรวจสอบว่าหน้านี้พิมพ์อะไรไปหรือไม่
            int startIndex = currentIndex;

            // วาดข้อความในรูปแบบตาราง (2 คอลัมน์ x 4 แถว = 8 ช่อง)
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (currentIndex >= maxIndex)
                        break;

                    float x = col * columnWidth;
                    float y = row * rowHeight;
                    RectangleF rect = new RectangleF(x, y, columnWidth, rowHeight);

                    using (System.Drawing.Font font = new System.Drawing.Font("Angsana New", 12))
                    {
                        SizeF textSize = e.Graphics.MeasureString(cellTexts[currentIndex], font, (int)rect.Width, sf);
                        // ตรวจสอบว่าความสูงที่วัดได้เกิน rowHeight หรือไม่
                        float usedRowHeight = Math.Max(rowHeight, textSize.Height);
                        RectangleF textRect = new RectangleF(rect.X + indent, rect.Y, rect.Width - indent, usedRowHeight);

                        e.Graphics.DrawString(cellTexts[currentIndex], font, Brushes.Black, textRect, sf);
                    }

                    currentIndex++;
                }

                if (currentIndex >= maxIndex)
                    break;
            }

            // ถ้าในหน้าปัจจุบันไม่พิมพ์อะไร (เช่น element ว่าง) ให้หยุดการพิมพ์เพื่อป้องกันการวนลูปไม่รู้จบ
            if (currentIndex == startIndex)
            {
                e.HasMorePages = false;
                return;
            }

            // ตรวจสอบว่ามีข้อความเหลือให้พิมพ์ในหน้าถัดไปหรือไม่
            if (currentIndex < maxIndex)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
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
                SELECT CONCAT(n_title, n_first, ' ', n_last) AS FullName, q_share
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
                SELECT MeetingNumber, AgendaNumber, AgendaTitle
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

                            using (System.Drawing.Font font = new System.Drawing.Font("Angsana New", 12))
                            {
                                // กำหนดความกว้างสูงสุด (ปรับค่าให้เหมาะสมกับ layout ของคุณ)
                                float maxTextWidth = 265f;
                                using (Graphics g = richTextBoxTemplate.CreateGraphics())
                                {
                                    agendaTitle = WrapText(g, agendaTitle, font, maxTextWidth);
                                }
                            }

                            // ประกอบ Block Template สำหรับแต่ละแถวใน HeaderTemplate
                            documentTemplate.AppendLine($"บัตรลงคะแนนการประชุมสามัญผู้ถือหุ้น ครั้งที่ {meetingNumber}              {identifier}");
                            documentTemplate.Append($"วาระที่ {agendaNumber}    {agendaTitle}");
                            documentTemplate.AppendLine($"ชื่อ: {fullName}");
                            documentTemplate.AppendLine($"จำนวนหุ้น: {q_share}");
                            documentTemplate.AppendLine("   [  ] เห็นด้วย           [  ] ไม่เห็นด้วย         [  ] งดออกเสียง");
                            documentTemplate.AppendLine("     (Approved)         (Disapproved)     (Abstained)");
                            documentTemplate.AppendLine("");
                            documentTemplate.AppendLine("ลงชื่อ ____________________________ ผู้ถือหุ้น/มอบฉันทะ");
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

        private int FindMaxFit(string text, int start, System.Drawing.Font font, Graphics g, float maxWidth)
        {
            int lastSpace = -1;
            int fit = 0;
            for (int i = start; i < text.Length; i++)
            {
                string substr = text.Substring(start, i - start + 1);
                SizeF size = g.MeasureString(substr, font);
                if (size.Width > maxWidth)
                {
                    // หากเจอขนาดเกิน ให้ตัดที่เว้นวรรคล่าสุด (ถ้ามี)
                    if (lastSpace != -1)
                    {
                        return lastSpace - start + 1;
                    }
                    // ถ้าไม่มีเว้นวรรคเลย ให้ตัดก่อนตัวอักษรที่ทำให้เกิน
                    return i - start;
                }
                if (char.IsWhiteSpace(text[i]))
                {
                    lastSpace = i;
                }
                fit = i - start + 1;
            }
            return fit;
        }

        // ฟังก์ชัน WrapText ใหม่
        private string WrapText(Graphics g, string text, System.Drawing.Font font, float maxWidth)
        {
            // ถ้ามีบรรทัดเว้นไว้ใน text ให้รวมให้เป็นข้อความต่อเนื่อง
            text = text.Replace("\r", " ").Replace("\n", " ");

            StringBuilder wrapped = new StringBuilder();
            int start = 0;
            while (start < text.Length)
            {
                // หาจำนวนตัวอักษรที่สามารถพิมพ์ในบรรทัดนี้ได้
                int count = FindMaxFit(text, start, font, g, maxWidth);
                if (count == 0)
                    break;

                string line = text.Substring(start, count).TrimEnd();
                wrapped.AppendLine(line);
                start += count;

                // ข้ามช่องว่างที่อาจเกิดขึ้นหลังบรรทัดที่ตัดแล้ว
                while (start < text.Length && char.IsWhiteSpace(text[start]))
                {
                    start++;
                }
            }
            return wrapped.ToString();
        }
    }
}