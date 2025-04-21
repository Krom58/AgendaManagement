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
        private ThaiIDCard idcard;
        public MultiInputForm()
        {
            InitializeComponent();
            txtPeopleCount.Text = "1";
            txtNote.Text = "";
            // Set DropDownStyle to DropDownList to make ComboBox non-editable
            cbxReaderList.DropDownStyle = ComboBoxStyle.DropDownList;
            // Set DrawMode to OwnerDrawFixed to handle custom drawing
            cbxReaderList.DrawMode = DrawMode.OwnerDrawFixed;
            cbxReaderList.DrawItem += new DrawItemEventHandler(cbxReaderList_DrawItem);
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

            }
            else
            {
                if (cbxReaderList.SelectedItem != null)
                    idcard.MonitorStop(cbxReaderList.SelectedItem.ToString());
            }
        }
        public void CardInserted(Personal personal)
        {
            try
            {
                if (personal == null)
                {
                    if (idcard.ErrorCode() > 0)
                    {
                        MessageBox.Show(idcard.Error());
                    }
                    else
                    {
                        MessageBox.Show("No card detected.");
                    }
                    return;
                }

                txtNote.Text = personal.Th_Firstname + ' ' + personal.Th_Lastname;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in CardInserted: " + ex.Message);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {

                Personal personal = idcard.readAll();
                if (personal != null)
                {
                    txtNote.Text = personal.Th_Firstname + ' ' + personal.Th_Lastname;
                }
                else if (idcard.ErrorCode() > 0)
                {
                    MessageBox.Show(idcard.Error());
                }
                else
                {
                    MessageBox.Show("No card detected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
