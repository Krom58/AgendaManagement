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
    public partial class MultiInputForm: Form
    {
        public MultiInputForm()
        {
            InitializeComponent();
            txtPeopleCount.Text = "1";
            txtNote.Text = "";
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
    }
}
