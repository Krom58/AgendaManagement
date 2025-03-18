namespace Work1
{
    partial class RegistrationViewer
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
            this.dataGridViewRegistration = new System.Windows.Forms.DataGridView();
            this.SearchButton = new System.Windows.Forms.Button();
            this.txtSearchFullName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSelfCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblProxyCount = new System.Windows.Forms.Label();
            this.btnLoadSelf = new System.Windows.Forms.Button();
            this.btnLoadProxy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegistration)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRegistration
            // 
            this.dataGridViewRegistration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRegistration.Location = new System.Drawing.Point(1, 201);
            this.dataGridViewRegistration.Name = "dataGridViewRegistration";
            this.dataGridViewRegistration.Size = new System.Drawing.Size(1482, 509);
            this.dataGridViewRegistration.TabIndex = 24;
            // 
            // SearchButton
            // 
            this.SearchButton.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchButton.Location = new System.Drawing.Point(663, 104);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(125, 50);
            this.SearchButton.TabIndex = 22;
            this.SearchButton.Text = "ค้นหา";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // txtSearchFullName
            // 
            this.txtSearchFullName.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchFullName.Location = new System.Drawing.Point(157, 103);
            this.txtSearchFullName.Name = "txtSearchFullName";
            this.txtSearchFullName.Size = new System.Drawing.Size(500, 51);
            this.txtSearchFullName.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 43);
            this.label2.TabIndex = 16;
            this.label2.Text = "ชื่อ - นามสกุล";
            // 
            // Back
            // 
            this.Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(1322, 12);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(150, 75);
            this.Back.TabIndex = 15;
            this.Back.Text = "กลับหน้าหลัก";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 43);
            this.label1.TabIndex = 25;
            this.label1.Text = "จำนวนผู้ลงทะเบียนทั้งหมด";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(269, 27);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 43);
            this.lblTotal.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(413, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 43);
            this.label6.TabIndex = 27;
            this.label6.Text = "จำนวนมาเอง";
            // 
            // lblSelfCount
            // 
            this.lblSelfCount.AutoSize = true;
            this.lblSelfCount.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelfCount.Location = new System.Drawing.Point(549, 27);
            this.lblSelfCount.Name = "lblSelfCount";
            this.lblSelfCount.Size = new System.Drawing.Size(0, 43);
            this.lblSelfCount.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(681, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(174, 43);
            this.label8.TabIndex = 29;
            this.label8.Text = "จำนวนมอบฉันทะ";
            // 
            // lblProxyCount
            // 
            this.lblProxyCount.AutoSize = true;
            this.lblProxyCount.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProxyCount.Location = new System.Drawing.Point(861, 27);
            this.lblProxyCount.Name = "lblProxyCount";
            this.lblProxyCount.Size = new System.Drawing.Size(0, 43);
            this.lblProxyCount.TabIndex = 30;
            // 
            // btnLoadSelf
            // 
            this.btnLoadSelf.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSelf.Location = new System.Drawing.Point(987, 27);
            this.btnLoadSelf.Name = "btnLoadSelf";
            this.btnLoadSelf.Size = new System.Drawing.Size(125, 50);
            this.btnLoadSelf.TabIndex = 31;
            this.btnLoadSelf.Text = "มาเอง";
            this.btnLoadSelf.UseVisualStyleBackColor = true;
            this.btnLoadSelf.Click += new System.EventHandler(this.btnLoadSelf_Click);
            // 
            // btnLoadProxy
            // 
            this.btnLoadProxy.Font = new System.Drawing.Font("Angsana New", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadProxy.Location = new System.Drawing.Point(1144, 27);
            this.btnLoadProxy.Name = "btnLoadProxy";
            this.btnLoadProxy.Size = new System.Drawing.Size(125, 50);
            this.btnLoadProxy.TabIndex = 32;
            this.btnLoadProxy.Text = "มอบฉันทะ";
            this.btnLoadProxy.UseVisualStyleBackColor = true;
            this.btnLoadProxy.Click += new System.EventHandler(this.btnLoadProxy_Click);
            // 
            // RegistrationViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 711);
            this.Controls.Add(this.btnLoadProxy);
            this.Controls.Add(this.btnLoadSelf);
            this.Controls.Add(this.lblProxyCount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblSelfCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewRegistration);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.txtSearchFullName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Back);
            this.Name = "RegistrationViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RegistrationViewer";
            this.Load += new System.EventHandler(this.RegistrationViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegistration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRegistration;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox txtSearchFullName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSelfCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblProxyCount;
        private System.Windows.Forms.Button btnLoadSelf;
        private System.Windows.Forms.Button btnLoadProxy;
    }
}