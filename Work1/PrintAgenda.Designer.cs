namespace Work1
{
    partial class PrintAgenda
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
            this.btnLoadPersonData = new System.Windows.Forms.Button();
            this.Back = new System.Windows.Forms.Button();
            this.richTextBoxTemplate = new System.Windows.Forms.RichTextBox();
            this.PrintDoc = new System.Windows.Forms.Button();
            this.comboBoxPerson = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnLoadPersonData
            // 
            this.btnLoadPersonData.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadPersonData.Location = new System.Drawing.Point(318, 11);
            this.btnLoadPersonData.Name = "btnLoadPersonData";
            this.btnLoadPersonData.Size = new System.Drawing.Size(165, 50);
            this.btnLoadPersonData.TabIndex = 16;
            this.btnLoadPersonData.Text = "ตกลง";
            this.btnLoadPersonData.UseVisualStyleBackColor = true;
            this.btnLoadPersonData.Click += new System.EventHandler(this.btnLoadTemplate_Click);
            // 
            // Back
            // 
            this.Back.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(887, 9);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(125, 50);
            this.Back.TabIndex = 17;
            this.Back.Text = "กลับหน้าหลัก";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // richTextBoxTemplate
            // 
            this.richTextBoxTemplate.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxTemplate.Location = new System.Drawing.Point(12, 65);
            this.richTextBoxTemplate.Name = "richTextBoxTemplate";
            this.richTextBoxTemplate.Size = new System.Drawing.Size(1000, 640);
            this.richTextBoxTemplate.TabIndex = 19;
            this.richTextBoxTemplate.Text = "";
            // 
            // PrintDoc
            // 
            this.PrintDoc.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintDoc.Location = new System.Drawing.Point(611, 9);
            this.PrintDoc.Name = "PrintDoc";
            this.PrintDoc.Size = new System.Drawing.Size(165, 50);
            this.PrintDoc.TabIndex = 20;
            this.PrintDoc.Text = "ปริ้น";
            this.PrintDoc.UseVisualStyleBackColor = true;
            this.PrintDoc.Click += new System.EventHandler(this.PrintDoc_Click);
            // 
            // comboBoxPerson
            // 
            this.comboBoxPerson.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPerson.FormattingEnabled = true;
            this.comboBoxPerson.Location = new System.Drawing.Point(12, 10);
            this.comboBoxPerson.Name = "comboBoxPerson";
            this.comboBoxPerson.Size = new System.Drawing.Size(300, 51);
            this.comboBoxPerson.TabIndex = 21;
            this.comboBoxPerson.TextChanged += new System.EventHandler(this.comboBoxPerson_TextChanged);
            // 
            // PrintAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 711);
            this.Controls.Add(this.comboBoxPerson);
            this.Controls.Add(this.PrintDoc);
            this.Controls.Add(this.richTextBoxTemplate);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.btnLoadPersonData);
            this.Name = "PrintAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintAgenda";
            this.Load += new System.EventHandler(this.PrintAgenda_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadPersonData;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.RichTextBox richTextBoxTemplate;
        private System.Windows.Forms.Button PrintDoc;
        private System.Windows.Forms.ComboBox comboBoxPerson;
    }
}