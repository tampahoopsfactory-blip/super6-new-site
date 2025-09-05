namespace DoNetDrive.Protocol.Door.Test
{
    partial class frmPassword
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
            this.butAddPassword = new System.Windows.Forms.Button();
            this.labelX1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new System.Windows.Forms.GroupBox();
            this.butReadPasswordDetail = new System.Windows.Forms.Button();
            this.butClearPassword = new System.Windows.Forms.Button();
            this.butReadAllPassword = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Doors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpenTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Expiry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbReverse = new System.Windows.Forms.CheckBox();
            this.btnClearList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbOpenTimes = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cbbit0 = new System.Windows.Forms.CheckBox();
            this.cbbit1 = new System.Windows.Forms.CheckBox();
            this.cbbit2 = new System.Windows.Forms.CheckBox();
            this.cbbit3 = new System.Windows.Forms.CheckBox();
            this.butInsertList = new System.Windows.Forms.Button();
            this.butDelList = new System.Windows.Forms.Button();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.btnDelDevice = new System.Windows.Forms.Button();
            this.btnDelSelect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRandom = new System.Windows.Forms.Button();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // butAddPassword
            // 
            this.butAddPassword.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butAddPassword.Location = new System.Drawing.Point(320, 36);
            this.butAddPassword.Name = "butAddPassword";
            this.butAddPassword.Size = new System.Drawing.Size(109, 52);
            this.butAddPassword.TabIndex = 25;
            this.butAddPassword.Text = "添加列表密码";
            this.butAddPassword.Click += new System.EventHandler(this.ButAddPassword_Click);
            // 
            // labelX1
            // 
            // 
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 100);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(263, 23);
            this.labelX1.TabIndex = 24;
            this.labelX1.Text = "密码列表：";
            // 
            // groupPanel1
            this.groupPanel1.Controls.Add(this.butReadPasswordDetail);
            this.groupPanel1.Controls.Add(this.butClearPassword);
            this.groupPanel1.Controls.Add(this.butReadAllPassword);
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(558, 82);

            this.groupPanel1.TabIndex = 23;
            this.groupPanel1.Text = "控制板中的密码表";
            // 
            // butReadPasswordDetail
            // 
            this.butReadPasswordDetail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butReadPasswordDetail.Location = new System.Drawing.Point(12, 3);
            this.butReadPasswordDetail.Name = "butReadPasswordDetail";
            this.butReadPasswordDetail.Size = new System.Drawing.Size(121, 52);
            this.butReadPasswordDetail.TabIndex = 1;
            this.butReadPasswordDetail.Text = "读取密码库存储详情";
            this.butReadPasswordDetail.Click += new System.EventHandler(this.ButReadPasswordDetail_Click);
            // 
            // butClearPassword
            // 
            this.butClearPassword.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butClearPassword.Location = new System.Drawing.Point(428, 3);
            this.butClearPassword.Name = "butClearPassword";
            this.butClearPassword.Size = new System.Drawing.Size(109, 52);
            this.butClearPassword.TabIndex = 3;
            this.butClearPassword.Text = "删除所有密码表";
            this.butClearPassword.Click += new System.EventHandler(this.ButClearPassword_Click);
            // 
            // butReadAllPassword
            // 
            this.butReadAllPassword.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butReadAllPassword.Location = new System.Drawing.Point(148, 3);
            this.butReadAllPassword.Name = "butReadAllPassword";
            this.butReadAllPassword.Size = new System.Drawing.Size(142, 52);
            this.butReadAllPassword.TabIndex = 2;
            this.butReadAllPassword.Text = "从控制板读取所有密码表";
            this.butReadAllPassword.Click += new System.EventHandler(this.ButReadAllPassword_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Password,
            this.Doors,
            this.OpenTimes,
            this.Expiry});
            this.dataGridView1.Location = new System.Drawing.Point(12, 129);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(590, 232);
            this.dataGridView1.TabIndex = 26;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView1_CellMouseClick);
            // 
            // Selected
            // 
            this.Selected.DataPropertyName = "Selected";
            this.Selected.HeaderText = "选择";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Width = 50;
            // 
            // Password
            // 
            this.Password.DataPropertyName = "Password";
            this.Password.HeaderText = "密码";
            this.Password.Name = "Password";
            this.Password.ReadOnly = true;
            // 
            // Doors
            // 
            this.Doors.DataPropertyName = "Doors";
            this.Doors.HeaderText = "有效门";
            this.Doors.Name = "Doors";
            this.Doors.ReadOnly = true;
            this.Doors.Width = 130;
            // 
            // OpenTimes
            // 
            this.OpenTimes.DataPropertyName = "OpenTimes";
            this.OpenTimes.HeaderText = "有效期";
            this.OpenTimes.Name = "OpenTimes";
            this.OpenTimes.ReadOnly = true;
            // 
            // Expiry
            // 
            this.Expiry.DataPropertyName = "Expiry";
            this.Expiry.HeaderText = "有效期";
            this.Expiry.Name = "Expiry";
            this.Expiry.ReadOnly = true;
            // 
            // cbReverse
            // 
            this.cbReverse.AutoSize = true;
            this.cbReverse.Location = new System.Drawing.Point(12, 367);
            this.cbReverse.Name = "cbReverse";
            this.cbReverse.Size = new System.Drawing.Size(48, 16);
            this.cbReverse.TabIndex = 27;
            this.cbReverse.Text = "反选";
            this.cbReverse.UseVisualStyleBackColor = true;
            this.cbReverse.CheckedChanged += new System.EventHandler(this.CbReverse_CheckedChanged);
            // 
            // btnClearList
            // 
            this.btnClearList.Location = new System.Drawing.Point(527, 367);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(75, 23);
            this.btnClearList.TabIndex = 28;
            this.btnClearList.Text = "清空表格";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.BtnClearList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 396);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 29;
            this.label1.Text = "密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 435);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 30;
            this.label2.Text = "有效测试：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 411);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 21);
            this.txtPassword.TabIndex = 31;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // cmbOpenTimes
            // 
            this.cmbOpenTimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOpenTimes.FormattingEnabled = true;
            this.cmbOpenTimes.Location = new System.Drawing.Point(12, 450);
            this.cmbOpenTimes.Name = "cmbOpenTimes";
            this.cmbOpenTimes.Size = new System.Drawing.Size(100, 20);
            this.cmbOpenTimes.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "权限：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 435);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "有效期：";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "yyyy/MM/dd";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(224, 450);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(99, 21);
            this.dtpDate.TabIndex = 35;
            // 
            // cbbit0
            // 
            this.cbbit0.AutoSize = true;
            this.cbbit0.Location = new System.Drawing.Point(225, 416);
            this.cbbit0.Name = "cbbit0";
            this.cbbit0.Size = new System.Drawing.Size(42, 16);
            this.cbbit0.TabIndex = 36;
            this.cbbit0.Text = "门1";
            this.cbbit0.UseVisualStyleBackColor = true;
            // 
            // cbbit1
            // 
            this.cbbit1.AutoSize = true;
            this.cbbit1.Location = new System.Drawing.Point(287, 416);
            this.cbbit1.Name = "cbbit1";
            this.cbbit1.Size = new System.Drawing.Size(42, 16);
            this.cbbit1.TabIndex = 37;
            this.cbbit1.Text = "门2";
            this.cbbit1.UseVisualStyleBackColor = true;
            // 
            // cbbit2
            // 
            this.cbbit2.AutoSize = true;
            this.cbbit2.Location = new System.Drawing.Point(348, 416);
            this.cbbit2.Name = "cbbit2";
            this.cbbit2.Size = new System.Drawing.Size(42, 16);
            this.cbbit2.TabIndex = 38;
            this.cbbit2.Text = "门3";
            this.cbbit2.UseVisualStyleBackColor = true;
            // 
            // cbbit3
            // 
            this.cbbit3.AutoSize = true;
            this.cbbit3.Location = new System.Drawing.Point(410, 416);
            this.cbbit3.Name = "cbbit3";
            this.cbbit3.Size = new System.Drawing.Size(42, 16);
            this.cbbit3.TabIndex = 39;
            this.cbbit3.Text = "门4";
            this.cbbit3.UseVisualStyleBackColor = true;
            // 
            // butInsertList
            // 
            this.butInsertList.Location = new System.Drawing.Point(14, 487);
            this.butInsertList.Name = "butInsertList";
            this.butInsertList.Size = new System.Drawing.Size(147, 23);
            this.butInsertList.TabIndex = 40;
            this.butInsertList.Text = "新增至列表";
            this.butInsertList.UseVisualStyleBackColor = true;
            this.butInsertList.Click += new System.EventHandler(this.ButInsertList_Click);
            // 
            // butDelList
            // 
            this.butDelList.Location = new System.Drawing.Point(169, 487);
            this.butDelList.Name = "butDelList";
            this.butDelList.Size = new System.Drawing.Size(147, 23);
            this.butDelList.TabIndex = 41;
            this.butDelList.Text = "从列表删除";
            this.butDelList.UseVisualStyleBackColor = true;
            this.butDelList.Click += new System.EventHandler(this.ButDelList_Click);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.Location = new System.Drawing.Point(324, 487);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(147, 23);
            this.btnAddDevice.TabIndex = 42;
            this.btnAddDevice.Text = "新增至设备";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.BtnAddDevice_Click);
            // 
            // btnDelDevice
            // 
            this.btnDelDevice.Location = new System.Drawing.Point(14, 516);
            this.btnDelDevice.Name = "btnDelDevice";
            this.btnDelDevice.Size = new System.Drawing.Size(224, 23);
            this.btnDelDevice.TabIndex = 43;
            this.btnDelDevice.Text = "从设备删除单个密码";
            this.btnDelDevice.UseVisualStyleBackColor = true;
            this.btnDelDevice.Click += new System.EventHandler(this.BtnDelDevice_Click);
            // 
            // btnDelSelect
            // 
            this.btnDelSelect.Location = new System.Drawing.Point(247, 516);
            this.btnDelSelect.Name = "btnDelSelect";
            this.btnDelSelect.Size = new System.Drawing.Size(224, 23);
            this.btnDelSelect.TabIndex = 44;
            this.btnDelSelect.Text = "从设备删除多个密码";
            this.btnDelSelect.UseVisualStyleBackColor = true;
            this.btnDelSelect.Click += new System.EventHandler(this.BtnDelSelect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRandom);
            this.groupBox1.Controls.Add(this.txtCount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(3, 545);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 70);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自动生成";
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(128, 41);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(75, 23);
            this.btnRandom.TabIndex = 46;
            this.btnRandom.Text = "生成";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.BtnRandom_Click);
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(6, 43);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(100, 21);
            this.txtCount.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "自动生成密码：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(9, 621);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(586, 70);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试密码";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "门1",
            "门2",
            "门3",
            "门4"});
            this.comboBox2.Location = new System.Drawing.Point(307, 37);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(100, 20);
            this.comboBox2.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 47;
            this.label7.Text = "端口号：";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(6, 35);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(107, 23);
            this.button8.TabIndex = 46;
            this.button8.Text = "测试密码";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(151, 36);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 21);
            this.textBox3.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "密码：";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(324, 450);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(55, 21);
            this.dtpTime.TabIndex = 48;
            // 
            // frmPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 694);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelSelect);
            this.Controls.Add(this.btnDelDevice);
            this.Controls.Add(this.btnAddDevice);
            this.Controls.Add(this.butDelList);
            this.Controls.Add(this.butInsertList);
            this.Controls.Add(this.cbbit3);
            this.Controls.Add(this.cbbit2);
            this.Controls.Add(this.cbbit1);
            this.Controls.Add(this.cbbit0);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbOpenTimes);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClearList);
            this.Controls.Add(this.cbReverse);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.butAddPassword);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.Name = "frmPassword";
            this.Text = "开门密码";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPassword_FormClosed);
            this.Load += new System.EventHandler(this.frmPassword_Load);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button butAddPassword;
        private System.Windows.Forms.Label labelX1;
        private System.Windows.Forms.GroupBox groupPanel1;
        private System.Windows.Forms.Button butReadPasswordDetail;
        private System.Windows.Forms.Button butClearPassword;
        private System.Windows.Forms.Button butReadAllPassword;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox cbReverse;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cmbOpenTimes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.CheckBox cbbit0;
        private System.Windows.Forms.CheckBox cbbit1;
        private System.Windows.Forms.CheckBox cbbit2;
        private System.Windows.Forms.CheckBox cbbit3;
        private System.Windows.Forms.Button butInsertList;
        private System.Windows.Forms.Button butDelList;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.Button btnDelDevice;
        private System.Windows.Forms.Button btnDelSelect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Doors;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenTimes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Expiry;
    }
}