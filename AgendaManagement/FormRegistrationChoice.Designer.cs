namespace Work1
{
    partial class FormRegistrationChoice
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
            this.btnSelf = new System.Windows.Forms.Button();
            this.btnProxy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelf
            // 
            this.btnSelf.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelf.Location = new System.Drawing.Point(12, 19);
            this.btnSelf.Name = "btnSelf";
            this.btnSelf.Size = new System.Drawing.Size(125, 50);
            this.btnSelf.TabIndex = 2;
            this.btnSelf.Text = "มาเอง";
            this.btnSelf.UseVisualStyleBackColor = true;
            this.btnSelf.Click += new System.EventHandler(this.btnSelf_Click);
            // 
            // btnProxy
            // 
            this.btnProxy.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProxy.Location = new System.Drawing.Point(177, 19);
            this.btnProxy.Name = "btnProxy";
            this.btnProxy.Size = new System.Drawing.Size(125, 50);
            this.btnProxy.TabIndex = 3;
            this.btnProxy.Text = "มอบฉันทะ";
            this.btnProxy.UseVisualStyleBackColor = true;
            this.btnProxy.Click += new System.EventHandler(this.btnProxy_Click);
            // 
            // FormRegistrationChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 81);
            this.Controls.Add(this.btnProxy);
            this.Controls.Add(this.btnSelf);
            this.Name = "FormRegistrationChoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "เลือก มาเอง หรือ มอบฉันทะ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelf;
        private System.Windows.Forms.Button btnProxy;
    }
}