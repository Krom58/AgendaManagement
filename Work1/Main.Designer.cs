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
            this.Agenda = new System.Windows.Forms.Button();
            this.RegistrationViewer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ImportExcel
            // 
            this.ImportExcel.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportExcel.Location = new System.Drawing.Point(20, 250);
            this.ImportExcel.Name = "ImportExcel";
            this.ImportExcel.Size = new System.Drawing.Size(150, 75);
            this.ImportExcel.TabIndex = 0;
            this.ImportExcel.Text = "Import Excel";
            this.ImportExcel.UseVisualStyleBackColor = true;
            this.ImportExcel.Click += new System.EventHandler(this.ImportExcel_Click);
            // 
            // CheckDB
            // 
            this.CheckDB.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckDB.Location = new System.Drawing.Point(204, 250);
            this.CheckDB.Name = "CheckDB";
            this.CheckDB.Size = new System.Drawing.Size(150, 75);
            this.CheckDB.TabIndex = 1;
            this.CheckDB.Text = "ลงทะเบียน";
            this.CheckDB.UseVisualStyleBackColor = true;
            this.CheckDB.Click += new System.EventHandler(this.CheckDB_Click);
            // 
            // Agenda
            // 
            this.Agenda.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Agenda.Location = new System.Drawing.Point(572, 250);
            this.Agenda.Name = "Agenda";
            this.Agenda.Size = new System.Drawing.Size(150, 75);
            this.Agenda.TabIndex = 2;
            this.Agenda.Text = "เพิ่มวาระการประชุม";
            this.Agenda.UseVisualStyleBackColor = true;
            this.Agenda.Click += new System.EventHandler(this.Agenda_Click);
            // 
            // RegistrationViewer
            // 
            this.RegistrationViewer.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistrationViewer.Location = new System.Drawing.Point(393, 250);
            this.RegistrationViewer.Name = "RegistrationViewer";
            this.RegistrationViewer.Size = new System.Drawing.Size(150, 75);
            this.RegistrationViewer.TabIndex = 3;
            this.RegistrationViewer.Text = "ตรวจสอบผู้ลงทะเบียน";
            this.RegistrationViewer.UseVisualStyleBackColor = true;
            this.RegistrationViewer.Click += new System.EventHandler(this.RegistrationViewer_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.RegistrationViewer);
            this.Controls.Add(this.Agenda);
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
        private System.Windows.Forms.Button Agenda;
        private System.Windows.Forms.Button RegistrationViewer;
    }
}

