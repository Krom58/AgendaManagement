namespace Work1
{
    partial class Agenda
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
            this.Back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMeetingNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAgendaNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAgendaTitle = new System.Windows.Forms.TextBox();
            this.btnSaveHeaderTemplate = new System.Windows.Forms.Button();
            this.dataGridViewTemplate = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // Back
            // 
            this.Back.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(1347, 12);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(125, 50);
            this.Back.TabIndex = 2;
            this.Back.Text = "กลับหน้าหลัก";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(416, 43);
            this.label1.TabIndex = 4;
            this.label1.Text = "บัตรลงคะแนนการประชุมสามัญผู้ถือหุ้น ครั้งที่";
            // 
            // txtMeetingNumber
            // 
            this.txtMeetingNumber.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMeetingNumber.Location = new System.Drawing.Point(464, 36);
            this.txtMeetingNumber.Name = "txtMeetingNumber";
            this.txtMeetingNumber.Size = new System.Drawing.Size(75, 51);
            this.txtMeetingNumber.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 43);
            this.label2.TabIndex = 11;
            this.label2.Text = "วาระที่";
            // 
            // txtAgendaNumber
            // 
            this.txtAgendaNumber.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAgendaNumber.Location = new System.Drawing.Point(123, 141);
            this.txtAgendaNumber.Name = "txtAgendaNumber";
            this.txtAgendaNumber.Size = new System.Drawing.Size(75, 51);
            this.txtAgendaNumber.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(261, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 43);
            this.label3.TabIndex = 13;
            this.label3.Text = "หัวข้อวาระ";
            // 
            // txtAgendaTitle
            // 
            this.txtAgendaTitle.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAgendaTitle.Location = new System.Drawing.Point(380, 141);
            this.txtAgendaTitle.Name = "txtAgendaTitle";
            this.txtAgendaTitle.Size = new System.Drawing.Size(750, 51);
            this.txtAgendaTitle.TabIndex = 14;
            // 
            // btnSaveHeaderTemplate
            // 
            this.btnSaveHeaderTemplate.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveHeaderTemplate.Location = new System.Drawing.Point(1347, 140);
            this.btnSaveHeaderTemplate.Name = "btnSaveHeaderTemplate";
            this.btnSaveHeaderTemplate.Size = new System.Drawing.Size(125, 50);
            this.btnSaveHeaderTemplate.TabIndex = 15;
            this.btnSaveHeaderTemplate.Text = "สร้าง";
            this.btnSaveHeaderTemplate.UseVisualStyleBackColor = true;
            this.btnSaveHeaderTemplate.Click += new System.EventHandler(this.btnSaveHeaderTemplate_Click);
            // 
            // dataGridViewTemplate
            // 
            this.dataGridViewTemplate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTemplate.Location = new System.Drawing.Point(3, 252);
            this.dataGridViewTemplate.Name = "dataGridViewTemplate";
            this.dataGridViewTemplate.Size = new System.Drawing.Size(1479, 457);
            this.dataGridViewTemplate.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1347, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 50);
            this.button1.TabIndex = 17;
            this.button1.Text = "ลบ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(721, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(350, 47);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "เอางดออกเสียงไปรวมในการคำนวณ";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(721, 76);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(374, 47);
            this.checkBox2.TabIndex = 20;
            this.checkBox2.Text = "ไม่เอางดออกเสียงไปรวมในการคำนวณ";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // Agenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 711);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridViewTemplate);
            this.Controls.Add(this.btnSaveHeaderTemplate);
            this.Controls.Add(this.txtAgendaTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAgendaNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMeetingNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Back);
            this.Name = "Agenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agenda";
            this.Load += new System.EventHandler(this.Agenda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemplate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMeetingNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAgendaNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAgendaTitle;
        private System.Windows.Forms.Button btnSaveHeaderTemplate;
        private System.Windows.Forms.DataGridView dataGridViewTemplate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}