using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard;
using PCSC;
using PCSC.Iso7816;

namespace Work1
{
    public partial class MultiInputForm: Form
    {
        private string note;
        public MultiInputForm(string data)
        {
            InitializeComponent();
            note = data;
            txtPeopleCount.Text = "1"; // ค่าเริ่มต้น
            txtNote.Text = note; // ตั้งค่าข้อมูลใน txtNote
        }
        public MultiInputForm()
        {
            InitializeComponent();
            note = string.Empty;
            txtPeopleCount.Text = "1";
            txtNote.Text = note;
        }

        public string PeopleCountInput
        {
            get
            {
                string val = txtPeopleCount.Text.Trim();
                return string.IsNullOrEmpty(val) ? "1" : val;
            }
        }

        public string NoteInput
        {
            get { return txtNote.Text.Trim(); }
        }

        private void btnSelf_Click(object sender, EventArgs e)
        {
            // ถ้า TextBox สำหรับจำนวนคนว่าง ให้เซ็ตเป็น "1"
            if (string.IsNullOrEmpty(txtPeopleCount.Text.Trim()))
            {
                txtPeopleCount.Text = "1";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void MultiInputForm_Load(object sender, EventArgs e)
        {
           
        }
        public void UpdateNote(string note)
        {
            if (txtNote.InvokeRequired)
            {
                txtNote.Invoke(new MethodInvoker(() => txtNote.Text = note));
            }
            else
            {
                txtNote.Text = note;
            }
        }
    }
}
