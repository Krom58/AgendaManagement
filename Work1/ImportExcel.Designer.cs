namespace Work1
{
    partial class ImportExcel
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
            this.SelectFile = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.ImportToDatabase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Back
            // 
            this.Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(1322, 12);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(150, 75);
            this.Back.TabIndex = 1;
            this.Back.Text = "กลับหน้าหลัก";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // SelectFile
            // 
            this.SelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectFile.Location = new System.Drawing.Point(12, 95);
            this.SelectFile.Name = "SelectFile";
            this.SelectFile.Size = new System.Drawing.Size(725, 100);
            this.SelectFile.TabIndex = 2;
            this.SelectFile.Text = "เลือกไฟล์ Excel";
            this.SelectFile.UseVisualStyleBackColor = true;
            this.SelectFile.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(1, 201);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1482, 509);
            this.dataGridView.TabIndex = 3;
            // 
            // ImportToDatabase
            // 
            this.ImportToDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportToDatabase.Location = new System.Drawing.Point(747, 95);
            this.ImportToDatabase.Name = "ImportToDatabase";
            this.ImportToDatabase.Size = new System.Drawing.Size(725, 100);
            this.ImportToDatabase.TabIndex = 4;
            this.ImportToDatabase.Text = "Import";
            this.ImportToDatabase.UseVisualStyleBackColor = true;
            this.ImportToDatabase.Click += new System.EventHandler(this.ImportToDatabase_Click);
            // 
            // ImportExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 711);
            this.Controls.Add(this.ImportToDatabase);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.SelectFile);
            this.Controls.Add(this.Back);
            this.Name = "ImportExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImportExcel";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Button SelectFile;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button ImportToDatabase;
    }
}