namespace Work1
{
    partial class Main
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
            this.ImportExcel = new System.Windows.Forms.Button();
            this.CheckDB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ImportExcel
            // 
            this.ImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportExcel.Location = new System.Drawing.Point(20, 250);
            this.ImportExcel.Name = "ImportExcel";
            this.ImportExcel.Size = new System.Drawing.Size(150, 75);
            this.ImportExcel.TabIndex = 0;
            this.ImportExcel.Text = "ไปหน้า Import Excel";
            this.ImportExcel.UseVisualStyleBackColor = true;
            this.ImportExcel.Click += new System.EventHandler(this.ImportExcel_Click);
            // 
            // CheckDB
            // 
            this.CheckDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckDB.Location = new System.Drawing.Point(190, 250);
            this.CheckDB.Name = "CheckDB";
            this.CheckDB.Size = new System.Drawing.Size(150, 75);
            this.CheckDB.TabIndex = 1;
            this.CheckDB.Text = "ไปหน้าตรวจสอบข้อมูล";
            this.CheckDB.UseVisualStyleBackColor = true;
            this.CheckDB.Click += new System.EventHandler(this.CheckDB_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckDB);
            this.Controls.Add(this.ImportExcel);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ImportExcel;
        private System.Windows.Forms.Button CheckDB;
    }
}

