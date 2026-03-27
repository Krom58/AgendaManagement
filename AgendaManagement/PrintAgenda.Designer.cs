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
            this.richTextBoxTemplate = new System.Windows.Forms.RichTextBox();
            this.PrintDoc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxTemplate
            // 
            this.richTextBoxTemplate.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxTemplate.Location = new System.Drawing.Point(12, 65);
            this.richTextBoxTemplate.Name = "richTextBoxTemplate";
            this.richTextBoxTemplate.Size = new System.Drawing.Size(1000, 640);
            this.richTextBoxTemplate.TabIndex = 19;
            this.richTextBoxTemplate.Text = "";
            // 
            // PrintDoc
            // 
            this.PrintDoc.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintDoc.Location = new System.Drawing.Point(847, 9);
            this.PrintDoc.Name = "PrintDoc";
            this.PrintDoc.Size = new System.Drawing.Size(165, 50);
            this.PrintDoc.TabIndex = 20;
            this.PrintDoc.Text = "สั่งพิมพ์";
            this.PrintDoc.UseVisualStyleBackColor = true;
            this.PrintDoc.Click += new System.EventHandler(this.PrintDoc_Click);
            // 
            // PrintAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 711);
            this.Controls.Add(this.PrintDoc);
            this.Controls.Add(this.richTextBoxTemplate);
            this.Name = "PrintAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintAgenda";
            this.Load += new System.EventHandler(this.PrintAgenda_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxTemplate;
        private System.Windows.Forms.Button PrintDoc;
    }
}