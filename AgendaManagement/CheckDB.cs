﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using ThaiNationalIDCard;
using PCSC;
using PCSC.Iso7816;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using System.IO;
using System.Data.Common;
namespace Work1
{
    public partial class CheckDB : Form
    {
        private ThaiIDCard idcard;
        private bool isProcessingCard = false;
        private MultiInputForm multiInputForm;
        public CheckDB()
        {
            InitializeComponent();
            // Set DropDownStyle to DropDownList to make ComboBox non-editable
            cbxReaderList.DropDownStyle = ComboBoxStyle.DropDownList;
            // Set DrawMode to OwnerDrawFixed to handle custom drawing
            cbxReaderList.DrawMode = DrawMode.OwnerDrawFixed;
            cbxReaderList.DrawItem += new DrawItemEventHandler(cbxReaderList_DrawItem);
        }
        private void CheckDB_Load(object sender, EventArgs e)
        {
            idcard = new ThaiIDCard();
            btnRefreshReaderList_Click(sender, e); // Add this line to search for card readers on form load
            // Check if any card readers are available
            if (cbxReaderList.Items.Count == 0)
            {
                // Disable controls if no card readers are found
                cbxReaderList.Enabled = false;
                btnRefreshReaderList.Enabled = false;
                chkBoxMonitor.Enabled = false;
                btnRead.Enabled = false;
                PhotoProgressBar1.Enabled = false;
            }
            else
            {
                chkBoxMonitor.Checked = true; // Add this line to check the checkbox on form load
            }
        }

        private Main _Main;

        // Constructor ที่รับ Form1 เข้ามา
        public CheckDB(Main main)
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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string n_first = textBox1.Text.Trim();
            string n_last = textBox2.Text.Trim();
            string i_ref = textBox3.Text.Trim();

            try
            {
                // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                var dbcfg = new DBConfig(iniPath);

                // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                using (var conn = dbcfg.CreateConnection())
                {
                    conn.Open();

                    StringBuilder queryBuilder = new StringBuilder("SELECT * FROM PersonData ORDER BY Id");

                    if (!string.IsNullOrEmpty(n_first))
                    {
                        queryBuilder.Append(" AND n_first LIKE @n_first");
                    }
                    if (!string.IsNullOrEmpty(n_last))
                    {
                        queryBuilder.Append(" AND n_last LIKE @n_last");
                    }
                    if (!string.IsNullOrEmpty(i_ref))
                    {
                        queryBuilder.Append(" AND i_ref LIKE @i_ref");
                    }

                    using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        cmd.CommandText = queryBuilder.ToString();

                        if (!string.IsNullOrEmpty(n_first))
                        {
                            var param = cmd.CreateParameter();
                            param.ParameterName = "@n_first";
                            param.Value = "%" + n_first + "%";
                            cmd.Parameters.Add(param);
                        }
                        if (!string.IsNullOrEmpty(n_last))
                        {
                            var param = cmd.CreateParameter();
                            param.ParameterName = "@n_last";
                            param.Value = "%" + n_last + "%";
                            cmd.Parameters.Add(param);
                        }
                        if (!string.IsNullOrEmpty(i_ref))
                        {
                            var param = cmd.CreateParameter();
                            param.ParameterName = "@i_ref";
                            param.Value = "%" + i_ref + "%";
                            cmd.Parameters.Add(param);
                        }

                        using (var adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd; // ใช้ DbCommand โดยตรง
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Use Invoke to update the dataGridView on the UI thread
                            dataGridView.Invoke((MethodInvoker)delegate
                            {
                                dataGridView.DataSource = dt;
                                dataGridView.Columns["n_title"].HeaderText = "คำนำหน้า";
                                dataGridView.Columns["n_first"].HeaderText = "ชื่อ";
                                dataGridView.Columns["n_last"].HeaderText = "นามสกุล";
                                dataGridView.Columns["q_share"].HeaderText = "หุ้น";
                                dataGridView.Columns["i_ref"].HeaderText = "รหัสบัตรประชาชน";
                                dataGridView.Columns["SelfCount"].HeaderText = "มาเอง";
                                dataGridView.Columns["ProxyCount"].HeaderText = "มอบฉันทะ";
                                dataGridView.Columns["Note"].HeaderText = "หมายเหตุ";
                                dataGridView.Columns["q_share"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dataGridView.Columns["i_ref"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView.Columns["SelfCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView.Columns["ProxyCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                                // ซ่อนคอลัมน์ Id
                                dataGridView.Columns["Id"].Visible = false;
                                dataGridView.Columns["RegStatus"].Visible = false;
                                foreach (DataGridViewColumn col in dataGridView.Columns)
                                {
                                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            });

                            // ล้างข้อมูลในช่อง TextBox หลังจากการค้นหาเสร็จสิ้น
                            ClearSearchFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
        // ฟังก์ชันช่วยเหลือสำหรับล้างข้อมูลใน TextBox
        private void ClearSearchFields()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            // Initialize the MultiInputForm instance before using it
            multiInputForm = new MultiInputForm();

            // ตรวจสอบว่า DataGridView มีแถวที่เลือกหรือไม่
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการลงทะเบียน");
                return;
            }

            // สมมุติว่าเลือกแถวแรกที่ถูกเลือก
            int Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            string currentRegStatus = dataGridView.SelectedRows[0].Cells["RegStatus"].Value.ToString();

            // ตรวจสอบว่ามีสถานะ "ลงทะเบียนแล้ว" หรือไม่
            if (currentRegStatus == "ลงทะเบียนแล้ว")
            {
                MessageBox.Show("ชื่อนี้ลงทะเบียนแล้ว ไม่สามารถลงทะเบียนซ้ำได้");
                return;
            }

            // เปิด FormRegistrationChoice เพื่อเลือกประเภทการเข้าร่วม
            FormRegistrationChoice choiceForm = new FormRegistrationChoice();
            if (choiceForm.ShowDialog() == DialogResult.OK)
            {
                string attendChoice = choiceForm.SelectedChoice;  // "มาเอง" หรือ "มอบฉันทะ"
                if (multiInputForm.ShowDialog() == DialogResult.OK)
                {
                    string peopleCountInput = multiInputForm.PeopleCountInput;
                    string noteInput = multiInputForm.NoteInput;
                    // อัปเดทข้อมูลในตาราง PersonData โดยเซ็ตค่าในคอลัมน์ที่เกี่ยวข้องเป็น 1
                    // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
                    var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
                    var dbcfg = new DBConfig(iniPath);

                    // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
                    using (var conn = dbcfg.CreateConnection())
                    {
                        conn.Open();
                        string updateQuery = @"
                        UPDATE PersonData
                        SET RegStatus = N'ลงทะเบียนแล้ว',
                            SelfCount = CASE
                                                WHEN @AttendType = N'มาเอง' THEN 1 
                                                WHEN @AttendType = N'มอบฉันทะ' THEN NULL
                                            END,
                            ProxyCount = CASE 
                                                 WHEN @AttendType = N'มอบฉันทะ' THEN 1 
                                                 WHEN @AttendType = N'มาเอง' THEN NULL 
                                             END,
                            Note = @Note
                        WHERE Id = @Id";
                        using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                        {
                            cmd.CommandText = updateQuery;

                            var param1 = cmd.CreateParameter();
                            param1.ParameterName = "@AttendType";
                            param1.Value = attendChoice;
                            cmd.Parameters.Add(param1);

                            var param2 = cmd.CreateParameter();
                            param2.ParameterName = "@Note";
                            param2.Value = noteInput;
                            cmd.Parameters.Add(param2);

                            var param3 = cmd.CreateParameter();
                            param3.ParameterName = "@Id";
                            param3.Value = Id;
                            cmd.Parameters.Add(param3);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("ลงทะเบียนเรียบร้อยแล้ว");
                    LoadData(); // รีเฟรช DataGridView

                    // ดำเนินการแทรกข้อมูลลงในตารางลงทะเบียนเพิ่มเติม (ถ้ามี)
                    InsertRegistrationRecord(Id, attendChoice, noteInput, peopleCountInput);

                    // หลังจากลงทะเบียนแล้ว ให้เข้าสู่หน้าปริ้น
                    PrintAgenda printForm = new PrintAgenda(Id, attendChoice);
                    printForm.Show();
                }
            }
        }
        private void LoadData()
        {
            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string query = "SELECT * FROM PersonData ORDER BY Id";
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (DbDataAdapter adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView.DataSource = dt;
                        dataGridView.Columns["n_title"].HeaderText = "คำนำหน้า";
                        dataGridView.Columns["n_first"].HeaderText = "ชื่อ";
                        dataGridView.Columns["n_last"].HeaderText = "นามสกุล";
                        dataGridView.Columns["q_share"].HeaderText = "หุ้น";
                        dataGridView.Columns["i_ref"].HeaderText = "รหัสบัตรประชาชน";
                        dataGridView.Columns["SelfCount"].HeaderText = "มาเอง";
                        dataGridView.Columns["ProxyCount"].HeaderText = "มอบฉันทะ";
                        dataGridView.Columns["Note"].HeaderText = "หมายเหตุ";
                        dataGridView.Columns["q_share"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView.Columns["i_ref"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns["SelfCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns["ProxyCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        // ซ่อนคอลัมน์ Id
                        dataGridView.Columns["Id"].Visible = false;
                        dataGridView.Columns["RegStatus"].Visible = false;
                        foreach (DataGridViewColumn col in dataGridView.Columns)
                        {
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }
        private void InsertRegistrationRecord(int Id, string attendChoice, string noteInput, string peopleCountInput)
        {
            // ดึงข้อมูลของบุคคลจาก PersonData (FullName และ q_share)
            string fullName = "";
            string shareCount = "";

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();
                string queryPerson = "SELECT CONCAT(n_title, n_first, ' ', n_last) AS FullName, q_share, Note, Id FROM PersonData WHERE Id = @Id";
                using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = queryPerson;

                    var param = cmd.CreateParameter();
                    param.ParameterName = "@Id";
                    param.Value = Id;
                    cmd.Parameters.Add(param);

                    using (DbDataReader reader = cmd.ExecuteReader()) // Use DbDataReader instead of SqlDataReader
                    {
                        if (reader.Read())
                        {
                            fullName = reader["FullName"].ToString();
                            shareCount = reader["q_share"].ToString();
                            noteInput = reader["Note"].ToString();
                        }
                    }
                }

                // กำหนดค่า PeopleCount เป็น 1 สำหรับทั้งสองกรณี
                int peopleCount;
                if (!int.TryParse(peopleCountInput, out peopleCount))
                {
                    peopleCount = 1; // กรณีไม่สามารถแปลงได้ ให้ฟิกเป็น 1
                }

                if (attendChoice == "มาเอง")
                {
                    // ใช้ Person Id และ prefix "B" (หรือปรับตามที่คุณต้องการ)
                    string identifier = GetIdentifierFromPersonId(Id, "B");

                    string insertQuery = @"
                        INSERT INTO SelfRegistration (Identifier, PeopleCount, FullName, ShareCount, Note, Id)
                        VALUES (@Identifier, @PeopleCount, @FullName, @ShareCount, @Note, @Id)";
                    using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        cmd.CommandText = insertQuery;

                        var param1 = cmd.CreateParameter();
                        param1.ParameterName = "@Identifier";
                        param1.Value = identifier;
                        cmd.Parameters.Add(param1);

                        var param2 = cmd.CreateParameter();
                        param2.ParameterName = "@PeopleCount";
                        param2.Value = peopleCount;
                        cmd.Parameters.Add(param2);

                        var param3 = cmd.CreateParameter();
                        param3.ParameterName = "@FullName";
                        param3.Value = fullName;
                        cmd.Parameters.Add(param3);

                        var param4 = cmd.CreateParameter();
                        param4.ParameterName = "@ShareCount";
                        param4.Value = shareCount;
                        cmd.Parameters.Add(param4);

                        var param5 = cmd.CreateParameter();
                        param5.ParameterName = "@Note";
                        param5.Value = noteInput;
                        cmd.Parameters.Add(param5);

                        var param6 = cmd.CreateParameter();
                        param6.ParameterName = "@Id";
                        param6.Value = Id;
                        cmd.Parameters.Add(param6);

                        cmd.ExecuteNonQuery();
                    }
                }
                else if (attendChoice == "มอบฉันทะ")
                {
                    // ใช้ Person Id และ prefix "P"
                    string identifier = GetIdentifierFromPersonId(Id, "P");

                    string insertQuery = @"
                        INSERT INTO ProxyRegistration (Identifier, PeopleCount, FullName, ShareCount, Note, Id)
                        VALUES (@Identifier, @PeopleCount, @FullName, @ShareCount, @Note, @Id)";
                    using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                    {
                        cmd.CommandText = insertQuery;

                        var param1 = cmd.CreateParameter();
                        param1.ParameterName = "@Identifier";
                        param1.Value = identifier;
                        cmd.Parameters.Add(param1);

                        var param2 = cmd.CreateParameter();
                        param2.ParameterName = "@PeopleCount";
                        param2.Value = peopleCount;
                        cmd.Parameters.Add(param2);

                        var param3 = cmd.CreateParameter();
                        param3.ParameterName = "@FullName";
                        param3.Value = fullName;
                        cmd.Parameters.Add(param3);

                        var param4 = cmd.CreateParameter();
                        param4.ParameterName = "@ShareCount";
                        param4.Value = shareCount;
                        cmd.Parameters.Add(param4);

                        var param5 = cmd.CreateParameter();
                        param5.ParameterName = "@Note";
                        param5.Value = noteInput;
                        cmd.Parameters.Add(param5);

                        var param6 = cmd.CreateParameter();
                        param6.ParameterName = "@Id";
                        param6.Value = Id;
                        cmd.Parameters.Add(param6);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }
        private string GetIdentifierFromPersonId(int personId, string prefix)
        {
            return prefix + personId.ToString();
        }

        private void Reprint_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า DataGridView มีแถวที่เลือกหรือไม่
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการ reprint");
                return;
            }

            // สมมุติว่าเลือกแถวแรกที่ถูกเลือก
            int Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            string currentRegStatus = dataGridView.SelectedRows[0].Cells["RegStatus"].Value.ToString();

            // ตรวจสอบว่าบุคคลนี้ลงทะเบียนแล้วหรือไม่
            if (currentRegStatus != "ลงทะเบียนแล้ว")
            {
                MessageBox.Show("บุคคลนี้ยังไม่ได้ลงทะเบียน ไม่สามารถ reprint ได้");
                return;
            }

            // ตรวจสอบค่าในคอลัมน์ SelfCount และ ProxyCount
            int selfCount = 0;
            int proxyCount = 0;
            if (dataGridView.SelectedRows[0].Cells["SelfCount"].Value != null)
                int.TryParse(dataGridView.SelectedRows[0].Cells["SelfCount"].Value.ToString(), out selfCount);
            if (dataGridView.SelectedRows[0].Cells["ProxyCount"].Value != null)
                int.TryParse(dataGridView.SelectedRows[0].Cells["ProxyCount"].Value.ToString(), out proxyCount);

            string attendChoice = "";
            if (selfCount == 1)
                attendChoice = "มาเอง";
            else if (proxyCount == 1)
                attendChoice = "มอบฉันทะ";
            else
            {
                MessageBox.Show("ไม่พบข้อมูลการลงทะเบียนที่ถูกต้อง");
                return;
            }

            // เปิดหน้าปริ้นโดยส่ง personId และ attendChoice ไปด้วย
            PrintAgenda printForm = new PrintAgenda(Id, attendChoice);
            printForm.Show();
        }

        private void btnReregister_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า DataGridView มีแถวที่เลือกหรือไม่
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการ re-register");
                return;
            }

            // สมมุติว่าเลือกแถวแรกที่ถูกเลือก
            int personId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            string currentRegStatus = dataGridView.SelectedRows[0].Cells["RegStatus"].Value.ToString();

            // ตรวจสอบว่าลงทะเบียนแล้วหรือไม่ ถ้ายังไม่ลงทะเบียน ให้แจ้งให้ลงทะเบียนก่อน
            if (currentRegStatus != "ลงทะเบียนแล้ว")
            {
                MessageBox.Show("บุคคลนี้ยังไม่ได้ลงทะเบียน กรุณาลงทะเบียนก่อน re-register");
                return;
            }

            // ถามยืนยันการ re-register
            DialogResult result = MessageBox.Show(
                "คุณแน่ใจหรือไม่ที่จะ re-register บุคคลนี้? ข้อมูลการลงทะเบียนเดิมจะถูกลบออก",
                "ยืนยันการ re-register",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            // 1) สร้าง DBConfig instance (ปรับ path ให้ถูกต้อง)
            var iniPath = Path.Combine(Application.StartupPath, "database_config.ini");
            var dbcfg = new DBConfig(iniPath);

            // 2) ใช้ DbConnection แทน SqlConnection เพื่อรองรับฐานข้อมูลหลายประเภท
            using (var conn = dbcfg.CreateConnection())
            {
                conn.Open();

                string deleteSelf = "DELETE FROM SelfRegistration WHERE Id = @Id";
                using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = deleteSelf;

                    var param = cmd.CreateParameter();
                    param.ParameterName = "@Id";
                    param.Value = personId;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }

                string deleteProxy = "DELETE FROM ProxyRegistration WHERE Id = @Id";
                using (DbCommand cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                {
                    cmd.CommandText = deleteProxy;

                    var param = cmd.CreateParameter();
                    param.ParameterName = "@Id";
                    param.Value = personId;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }
            }

            // เปิด FormRegistrationChoice เพื่อเลือกประเภทการเข้าร่วม (มาเอง หรือ มอบฉันทะ)
            FormRegistrationChoice choiceForm = new FormRegistrationChoice();
            if (choiceForm.ShowDialog() == DialogResult.OK)
            {
                string attendChoice = choiceForm.SelectedChoice;  // "มาเอง" หรือ "มอบฉันทะ"

                // เปิด MultiInputForm เพื่อรับข้อมูลเพิ่มเติม (จำนวนคนและหมายเหตุ)
                MultiInputForm multiInputForm = new MultiInputForm();
                if (multiInputForm.ShowDialog() == DialogResult.OK)
                {
                    string peopleCountInput = multiInputForm.PeopleCountInput;
                    string noteInput = multiInputForm.NoteInput;

                    // อัปเดตสถานะใน PersonData ให้เป็น "ลงทะเบียนแล้ว" และอัปเดตคอลัมน์ SelfCount/ProxyCount ให้เป็น 1 ตามที่เลือก
                    using (var conn = dbcfg.CreateConnection()) // Use DbConnection instead of SqlConnection
                    {
                        conn.Open();
                        string updatePersonQuery = @"
                    UPDATE PersonData
                    SET RegStatus = N'ลงทะเบียนแล้ว',
                            SelfCount = CASE
                                                WHEN @AttendType = N'มาเอง' THEN 1 
                                                WHEN @AttendType = N'มอบฉันทะ' THEN NULL 
                                                ELSE NULL 
                                            END,
                            ProxyCount = CASE 
                                                 WHEN @AttendType = N'มอบฉันทะ' THEN 1 
                                                 WHEN @AttendType = N'มาเอง' THEN NULL 
                                                 ELSE NULL 
                                             END,
                        Note = @Note
                    WHERE Id = @Id";
                        using (var cmd = conn.CreateCommand()) // Use DbCommand instead of SqlCommand
                        {
                            cmd.CommandText = updatePersonQuery;

                            var param1 = cmd.CreateParameter();
                            param1.ParameterName = "@AttendType";
                            param1.Value = attendChoice;
                            cmd.Parameters.Add(param1);

                            var param2 = cmd.CreateParameter();
                            param2.ParameterName = "@Note";
                            param2.Value = noteInput;
                            cmd.Parameters.Add(param2);

                            var param3 = cmd.CreateParameter();
                            param3.ParameterName = "@Id";
                            param3.Value = personId;
                            cmd.Parameters.Add(param3);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Re-registration สำเร็จแล้ว");

                    // รีเฟรช DataGridView
                    LoadData();

                    // ดำเนินการแทรกข้อมูลลงในตารางการลงทะเบียนเพิ่มเติม (ถ้ามี)
                    InsertRegistrationRecord(personId, attendChoice, noteInput, peopleCountInput);

                    // เปิดหน้าปริ้น (PrintAgenda)
                    PrintAgenda printForm = new PrintAgenda(personId, attendChoice);
                    printForm.Show();
                }
            }
        }

        private void chkBoxMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxMonitor.Checked)
            {
                if (cbxReaderList.SelectedItem == null)
                {
                    MessageBox.Show("No reader select to monitoring.");
                    chkBoxMonitor.Checked = false;
                    return;
                }
                idcard.MonitorStart(cbxReaderList.SelectedItem.ToString());
                idcard.eventCardInsertedWithPhoto += new handleCardInserted(CardInserted);
                idcard.eventPhotoProgress += new handlePhotoProgress(photoProgress);
            }
            else
            {
                if (cbxReaderList.SelectedItem != null)
                {
                    idcard.MonitorStop(cbxReaderList.SelectedItem.ToString());
                }
                // ยกเลิกการสมัคร Event
                idcard.eventCardInsertedWithPhoto -= new handleCardInserted(CardInserted);
            }
        }
        public void CardInserted(Personal personal)
        {
            if (isProcessingCard) return; // หากกำลังประมวลผลอยู่ ให้ข้ามการทำงาน
            isProcessingCard = true;

            try
            {
                if (personal == null)
                {
                    HandleCardReadError();
                    return;
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    // ตรวจสอบว่า MultiInputForm เปิดอยู่หรือไม่
                    if (multiInputForm != null && !multiInputForm.IsDisposed)
                    {
                        // อัปเดต txtNote ใน MultiInputForm
                        multiInputForm.UpdateNote($"{personal.Th_Prefix}{personal.Th_Firstname} {personal.Th_Lastname}");
                    }
                    else
                    {
                        // Clear old data
                        ClearOldData();

                        // อัปเดตข้อมูลใน TextBox
                        UpdatePersonalInfo(personal);

                        // เรียกการค้นหาอัตโนมัติ
                        SearchButton_Click(this, EventArgs.Empty);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดใน CardInserted: " + ex.Message);
            }
            finally
            {
                isProcessingCard = false; // รีเซ็ตสถานะเมื่อประมวลผลเสร็จ
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบว่าเครื่องอ่านบัตรถูกเลือกหรือไม่
                if (cbxReaderList.SelectedItem == null)
                {
                    MessageBox.Show("กรุณาเลือกเครื่องอ่านบัตรก่อนดำเนินการ");
                    return;
                }

                // Clear old data
                ClearOldData();

                // อ่านข้อมูลจากบัตร
                Personal personal = idcard.readAll();
                if (personal != null)
                {
                    // อัปเดตข้อมูลใน TextBox
                    UpdatePersonalInfo(personal);

                    // เรียกการค้นหาอัตโนมัติ
                    SearchButton_Click(sender, e);
                }
                else
                {
                    HandleCardReadError();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการอ่านบัตร: " + ex.Message);
            }
        }
        private void UpdateUI(Action updateAction)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(updateAction);
            }
            else
            {
                updateAction();
            }
        }

        private void UpdatePersonalInfo(Personal personal)
        {
            UpdateUI(() =>
            {
                textBox3.Text = personal.Citizenid;
                textBox1.Text = personal.Th_Firstname;
                textBox2.Text = personal.Th_Lastname;
            });
        }

        // ฟังก์ชันช่วยเหลือสำหรับจัดการข้อผิดพลาดในการอ่านบัตร
        private void HandleCardReadError()
        {
            if (idcard.ErrorCode() > 0)
            {
                MessageBox.Show("ข้อผิดพลาดจากเครื่องอ่านบัตร: " + idcard.Error());
            }
            else
            {
                MessageBox.Show("ไม่พบบัตร กรุณาเสียบบัตรและลองใหม่อีกครั้ง");
            }
        }

        private void btnRefreshReaderList_Click(object sender, EventArgs e)
        {
            cbxReaderList.Items.Clear();
            cbxReaderList.SelectedIndex = -1;
            cbxReaderList.SelectedText = String.Empty;
            cbxReaderList.Text = string.Empty;
            cbxReaderList.Refresh();

            try
            {
                ThaiIDCard idcard = new ThaiIDCard();
                string[] readers = idcard.GetReaders();

                if (readers == null) return;
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                foreach (string r in readers)
                {
                    cbxReaderList.Items.Add(r);
                }

                // Automatically select the first item if there are any items
                if (cbxReaderList.Items.Count > 0)
                {
                    cbxReaderList.SelectedIndex = 0;
                }

                cbxReaderList.DroppedDown = true;
            }
            catch
            {
                
            }
        }
        private void photoProgress(int value, int maximum)
        {
            if (textBox1.InvokeRequired)
            {
                if (PhotoProgressBar1.Maximum != maximum)
                    PhotoProgressBar1.BeginInvoke(new MethodInvoker(delegate { PhotoProgressBar1.Maximum = maximum; }));

                // fix progress bar sync.
                if (PhotoProgressBar1.Maximum > value)
                    PhotoProgressBar1.BeginInvoke(new MethodInvoker(delegate { PhotoProgressBar1.Value = value + 1; }));

                PhotoProgressBar1.BeginInvoke(new MethodInvoker(delegate { PhotoProgressBar1.Value = value; }));
            }
            else
            {
                if (PhotoProgressBar1.Maximum != maximum)
                    PhotoProgressBar1.Maximum = maximum;

                // fix progress bar sync.
                if (PhotoProgressBar1.Maximum > value)
                    PhotoProgressBar1.Value = value + 1;

                PhotoProgressBar1.Value = value;
            }
        }

        private void cbxReaderList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox comboBox = (ComboBox)sender;
            string text = comboBox.Items[e.Index].ToString();

            // Draw the background of the item.
            e.DrawBackground();

            // Draw the text of the item.
            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            }

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        private void ClearOldData()
        {
            // Clear TextBoxes
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            // Clear DataGridView
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
        }
    }
}
