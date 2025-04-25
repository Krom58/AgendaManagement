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
            this.RegistrationViewer = new System.Windows.Forms.Button();
            this.Agenda = new System.Windows.Forms.Button();
            this.ImportExcel = new System.Windows.Forms.Button();
            this.CheckDB = new System.Windows.Forms.Button();
            this.RegistrationSummary = new System.Windows.Forms.Button();
            this.Setting = new System.Windows.Forms.Button();
            this.AgendaSummary = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegistrationViewer
            // 
            this.RegistrationViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegistrationViewer.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistrationViewer.Location = new System.Drawing.Point(188, 235);
            this.RegistrationViewer.Margin = new System.Windows.Forms.Padding(5);
            this.RegistrationViewer.Name = "RegistrationViewer";
            this.RegistrationViewer.Size = new System.Drawing.Size(173, 105);
            this.RegistrationViewer.TabIndex = 3;
            this.RegistrationViewer.Text = "ตรวจสอบผู้ลงทะเบียน";
            this.RegistrationViewer.UseVisualStyleBackColor = true;
            this.RegistrationViewer.Click += new System.EventHandler(this.RegistrationViewer_Click);
            // 
            // Agenda
            // 
            this.Agenda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Agenda.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Agenda.Location = new System.Drawing.Point(188, 120);
            this.Agenda.Margin = new System.Windows.Forms.Padding(5);
            this.Agenda.Name = "Agenda";
            this.Agenda.Size = new System.Drawing.Size(173, 105);
            this.Agenda.TabIndex = 2;
            this.Agenda.Text = "เพิ่มวาระการประชุม";
            this.Agenda.UseVisualStyleBackColor = true;
            this.Agenda.Visible = false;
            this.Agenda.Click += new System.EventHandler(this.Agenda_Click);
            // 
            // ImportExcel
            // 
            this.ImportExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImportExcel.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportExcel.Location = new System.Drawing.Point(188, 5);
            this.ImportExcel.Margin = new System.Windows.Forms.Padding(5);
            this.ImportExcel.Name = "ImportExcel";
            this.ImportExcel.Size = new System.Drawing.Size(173, 105);
            this.ImportExcel.TabIndex = 0;
            this.ImportExcel.Text = "Import Excel";
            this.ImportExcel.UseVisualStyleBackColor = true;
            this.ImportExcel.Visible = false;
            this.ImportExcel.Click += new System.EventHandler(this.ImportExcel_Click);
            // 
            // CheckDB
            // 
            this.CheckDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckDB.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckDB.Location = new System.Drawing.Point(5, 235);
            this.CheckDB.Margin = new System.Windows.Forms.Padding(5);
            this.CheckDB.Name = "CheckDB";
            this.CheckDB.Size = new System.Drawing.Size(173, 105);
            this.CheckDB.TabIndex = 1;
            this.CheckDB.Text = "ลงทะเบียน";
            this.CheckDB.UseVisualStyleBackColor = true;
            this.CheckDB.Click += new System.EventHandler(this.CheckDB_Click);
            // 
            // RegistrationSummary
            // 
            this.RegistrationSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegistrationSummary.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistrationSummary.Location = new System.Drawing.Point(371, 235);
            this.RegistrationSummary.Margin = new System.Windows.Forms.Padding(5);
            this.RegistrationSummary.Name = "RegistrationSummary";
            this.RegistrationSummary.Size = new System.Drawing.Size(173, 105);
            this.RegistrationSummary.TabIndex = 4;
            this.RegistrationSummary.Text = "สรุปผู้ลงทะเบียน";
            this.RegistrationSummary.UseVisualStyleBackColor = true;
            this.RegistrationSummary.Click += new System.EventHandler(this.RegistrationSummary_Click);
            // 
            // Setting
            // 
            this.Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Setting.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Setting.Location = new System.Drawing.Point(5, 5);
            this.Setting.Margin = new System.Windows.Forms.Padding(5);
            this.Setting.Name = "Setting";
            this.Setting.Size = new System.Drawing.Size(173, 105);
            this.Setting.TabIndex = 6;
            this.Setting.Text = "Setting";
            this.Setting.UseVisualStyleBackColor = true;
            this.Setting.Click += new System.EventHandler(this.Setting_Click);
            // 
            // AgendaSummary
            // 
            this.AgendaSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AgendaSummary.Font = new System.Drawing.Font("Angsana New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgendaSummary.Location = new System.Drawing.Point(554, 235);
            this.AgendaSummary.Margin = new System.Windows.Forms.Padding(5);
            this.AgendaSummary.Name = "AgendaSummary";
            this.AgendaSummary.Size = new System.Drawing.Size(175, 105);
            this.AgendaSummary.TabIndex = 5;
            this.AgendaSummary.Text = "สรุปวาระ";
            this.AgendaSummary.UseVisualStyleBackColor = true;
            this.AgendaSummary.Click += new System.EventHandler(this.AgendaSummary_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.AgendaSummary, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.Setting, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.RegistrationSummary, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.ImportExcel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckDB, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.RegistrationViewer, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Agenda, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 461);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RegistrationViewer;
        private System.Windows.Forms.Button Agenda;
        private System.Windows.Forms.Button ImportExcel;
        private System.Windows.Forms.Button CheckDB;
        private System.Windows.Forms.Button RegistrationSummary;
        private System.Windows.Forms.Button Setting;
        private System.Windows.Forms.Button AgendaSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

