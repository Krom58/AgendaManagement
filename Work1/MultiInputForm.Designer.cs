namespace Work1
{
    partial class MultiInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtPeopleCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnSelf = new System.Windows.Forms.Button();
            this.chkBoxMonitor = new System.Windows.Forms.CheckBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.cbxReaderList = new System.Windows.Forms.ComboBox();
            this.btnRefreshReaderList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "จำนวนผู้เข้าร่วม";
            // 
            // txtPeopleCount
            // 
            this.txtPeopleCount.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeopleCount.Location = new System.Drawing.Point(173, 72);
            this.txtPeopleCount.Name = "txtPeopleCount";
            this.txtPeopleCount.Size = new System.Drawing.Size(52, 51);
            this.txtPeopleCount.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 43);
            this.label2.TabIndex = 11;
            this.label2.Text = "หมายเหตุ";
            // 
            // txtNote
            // 
            this.txtNote.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNote.Location = new System.Drawing.Point(118, 142);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(454, 51);
            this.txtNote.TabIndex = 12;
            // 
            // btnSelf
            // 
            this.btnSelf.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelf.Location = new System.Drawing.Point(232, 199);
            this.btnSelf.Name = "btnSelf";
            this.btnSelf.Size = new System.Drawing.Size(125, 50);
            this.btnSelf.TabIndex = 13;
            this.btnSelf.Text = "ยืนยัน";
            this.btnSelf.UseVisualStyleBackColor = true;
            this.btnSelf.Click += new System.EventHandler(this.btnSelf_Click);
            // 
            // chkBoxMonitor
            // 
            this.chkBoxMonitor.AutoSize = true;
            this.chkBoxMonitor.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxMonitor.Location = new System.Drawing.Point(268, 84);
            this.chkBoxMonitor.Name = "chkBoxMonitor";
            this.chkBoxMonitor.Size = new System.Drawing.Size(129, 33);
            this.chkBoxMonitor.TabIndex = 19;
            this.chkBoxMonitor.Text = "อ่านบัตรอัตโนมัติ";
            this.chkBoxMonitor.UseVisualStyleBackColor = true;
            this.chkBoxMonitor.CheckedChanged += new System.EventHandler(this.chkBoxMonitor_CheckedChanged);
            // 
            // btnRead
            // 
            this.btnRead.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Location = new System.Drawing.Point(429, 81);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(143, 37);
            this.btnRead.TabIndex = 21;
            this.btnRead.Text = "อ่านบัตร";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // cbxReaderList
            // 
            this.cbxReaderList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxReaderList.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxReaderList.FormattingEnabled = true;
            this.cbxReaderList.Location = new System.Drawing.Point(12, 12);
            this.cbxReaderList.Name = "cbxReaderList";
            this.cbxReaderList.Size = new System.Drawing.Size(395, 37);
            this.cbxReaderList.TabIndex = 22;
            this.cbxReaderList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbxReaderList_DrawItem);
            // 
            // btnRefreshReaderList
            // 
            this.btnRefreshReaderList.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshReaderList.Location = new System.Drawing.Point(429, 11);
            this.btnRefreshReaderList.Name = "btnRefreshReaderList";
            this.btnRefreshReaderList.Size = new System.Drawing.Size(143, 37);
            this.btnRefreshReaderList.TabIndex = 23;
            this.btnRefreshReaderList.Text = "ค้นหาเครื่องอ่านบัตร";
            this.btnRefreshReaderList.UseVisualStyleBackColor = true;
            this.btnRefreshReaderList.Click += new System.EventHandler(this.btnRefreshReaderList_Click);
            // 
            // MultiInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.btnRefreshReaderList);
            this.Controls.Add(this.cbxReaderList);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.chkBoxMonitor);
            this.Controls.Add(this.btnSelf);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPeopleCount);
            this.Controls.Add(this.label1);
            this.Name = "MultiInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "กรอกจำนวนผู้เข้าร่วมกับหมายเหตุ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPeopleCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSelf;
        private System.Windows.Forms.CheckBox chkBoxMonitor;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ComboBox cbxReaderList;
        private System.Windows.Forms.Button btnRefreshReaderList;
    }
}