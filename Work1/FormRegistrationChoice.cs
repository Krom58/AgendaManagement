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
    public partial class FormRegistrationChoice: Form
    {
        public string SelectedChoice { get; set; } = "";
        public FormRegistrationChoice()
        {
            InitializeComponent();
        }

        private void btnSelf_Click(object sender, EventArgs e)
        {
            SelectedChoice = "มาเอง";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnProxy_Click(object sender, EventArgs e)
        {
            SelectedChoice = "ตัวแทน";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
