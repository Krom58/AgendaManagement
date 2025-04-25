namespace Work1
{
    partial class CheckDB
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
            this.btnRegister = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Back = new System.Windows.Forms.Button();
            this.chkBoxMonitor = new System.Windows.Forms.CheckBox();
            this.btnRefreshReaderList = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.cbxReaderList = new System.Windows.Forms.ComboBox();
            this.PhotoProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.Reprint = new System.Windows.Forms.Button();
            this.btnReregister = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRegister
            // 
            this.btnRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegister.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.Location = new System.Drawing.Point(1322, 150);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(150, 50);
            this.btnRegister.TabIndex = 13;
            this.btnRegister.Text = "ลงทะเบียน";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click_1);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(0, 206);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1484, 507);
            this.dataGridView.TabIndex = 14;
            // 
            // Back
            // 
            this.Back.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(1322, 12);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(150, 75);
            this.Back.TabIndex = 2;
            this.Back.Text = "กลับหน้าหลัก";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // chkBoxMonitor
            // 
            this.chkBoxMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxMonitor.AutoSize = true;
            this.chkBoxMonitor.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxMonitor.Location = new System.Drawing.Point(615, 14);
            this.chkBoxMonitor.Name = "chkBoxMonitor";
            this.chkBoxMonitor.Size = new System.Drawing.Size(129, 33);
            this.chkBoxMonitor.TabIndex = 18;
            this.chkBoxMonitor.Text = "อ่านบัตรอัตโนมัติ";
            this.chkBoxMonitor.UseVisualStyleBackColor = true;
            this.chkBoxMonitor.CheckedChanged += new System.EventHandler(this.chkBoxMonitor_CheckedChanged);
            // 
            // btnRefreshReaderList
            // 
            this.btnRefreshReaderList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshReaderList.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshReaderList.Location = new System.Drawing.Point(432, 12);
            this.btnRefreshReaderList.Name = "btnRefreshReaderList";
            this.btnRefreshReaderList.Size = new System.Drawing.Size(143, 37);
            this.btnRefreshReaderList.TabIndex = 17;
            this.btnRefreshReaderList.Text = "ค้นหาเครื่องอ่านบัตร";
            this.btnRefreshReaderList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRefreshReaderList.UseVisualStyleBackColor = true;
            this.btnRefreshReaderList.Click += new System.EventHandler(this.btnRefreshReaderList_Click);
            // 
            // btnRead
            // 
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRead.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Location = new System.Drawing.Point(786, 11);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(143, 37);
            this.btnRead.TabIndex = 20;
            this.btnRead.Text = "อ่านบัตร";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // cbxReaderList
            // 
            this.cbxReaderList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxReaderList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxReaderList.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxReaderList.FormattingEnabled = true;
            this.cbxReaderList.Location = new System.Drawing.Point(31, 12);
            this.cbxReaderList.Name = "cbxReaderList";
            this.cbxReaderList.Size = new System.Drawing.Size(395, 37);
            this.cbxReaderList.TabIndex = 19;
            this.cbxReaderList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbxReaderList_DrawItem);
            // 
            // PhotoProgressBar1
            // 
            this.PhotoProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PhotoProgressBar1.Location = new System.Drawing.Point(935, 12);
            this.PhotoProgressBar1.MarqueeAnimationSpeed = 0;
            this.PhotoProgressBar1.Name = "PhotoProgressBar1";
            this.PhotoProgressBar1.Size = new System.Drawing.Size(381, 34);
            this.PhotoProgressBar1.TabIndex = 53;
            // 
            // Reprint
            // 
            this.Reprint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Reprint.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Reprint.Location = new System.Drawing.Point(198, 150);
            this.Reprint.Name = "Reprint";
            this.Reprint.Size = new System.Drawing.Size(150, 50);
            this.Reprint.TabIndex = 15;
            this.Reprint.Text = "พิมพ์ใหม่";
            this.Reprint.UseVisualStyleBackColor = true;
            this.Reprint.Click += new System.EventHandler(this.Reprint_Click);
            // 
            // btnReregister
            // 
            this.btnReregister.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReregister.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReregister.Location = new System.Drawing.Point(31, 150);
            this.btnReregister.Name = "btnReregister";
            this.btnReregister.Size = new System.Drawing.Size(161, 50);
            this.btnReregister.TabIndex = 16;
            this.btnReregister.Text = "ลงทะเบียนใหม่";
            this.btnReregister.UseVisualStyleBackColor = true;
            this.btnReregister.Click += new System.EventHandler(this.btnReregister_Click);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchButton.Location = new System.Drawing.Point(1191, 79);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(125, 53);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "ค้นหา";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 43);
            this.label2.TabIndex = 6;
            this.label2.Text = "ชื่อ";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(74, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(250, 51);
            this.textBox1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(330, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 43);
            this.label4.TabIndex = 8;
            this.label4.Text = "นามสกุล";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(432, 79);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(250, 51);
            this.textBox2.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(697, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 43);
            this.label3.TabIndex = 7;
            this.label3.Text = "รหัสบัตรประชาชน";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(886, 79);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(300, 51);
            this.textBox3.TabIndex = 11;
            // 
            // CheckDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 711);
            this.Controls.Add(this.cbxReaderList);
            this.Controls.Add(this.PhotoProgressBar1);
            this.Controls.Add(this.btnRefreshReaderList);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.chkBoxMonitor);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnReregister);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Reprint);
            this.Name = "CheckDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "หน้าลงทะเบียน";
            this.Load += new System.EventHandler(this.CheckDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.CheckBox chkBoxMonitor;
        private System.Windows.Forms.Button btnRefreshReaderList;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ComboBox cbxReaderList;
        private System.Windows.Forms.ProgressBar PhotoProgressBar1;
        private System.Windows.Forms.Button Reprint;
        private System.Windows.Forms.Button btnReregister;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
    }
}