namespace DotNetDrive.Protocol.POS.Test
{
    partial class FrmReservation
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dtpReservationDate = new System.Windows.Forms.DateTimePicker();
            this.cbTimeGroup8 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup7 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup6 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup5 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup4 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup3 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup2 = new System.Windows.Forms.CheckBox();
            this.cbTimeGroup1 = new System.Windows.Forms.CheckBox();
            this.txtCardData = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butDeleteFromList = new System.Windows.Forms.Button();
            this.butAddAll = new System.Windows.Forms.Button();
            this.butAddToDevice = new System.Windows.Forms.Button();
            this.butAddToList = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtStartCode = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCreateCount = new System.Windows.Forms.NumericUpDown();
            this.butCreateByRandom = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butClearList = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butClearDataBase = new System.Windows.Forms.Button();
            this.butReadAllMenu = new System.Windows.Forms.Button();
            this.butReadDataBase = new System.Windows.Forms.Button();
            this.dgvReservation = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardData)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservation)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 445);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 202);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dtpReservationDate);
            this.tabPage1.Controls.Add(this.cbTimeGroup8);
            this.tabPage1.Controls.Add(this.cbTimeGroup7);
            this.tabPage1.Controls.Add(this.cbTimeGroup6);
            this.tabPage1.Controls.Add(this.cbTimeGroup5);
            this.tabPage1.Controls.Add(this.cbTimeGroup4);
            this.tabPage1.Controls.Add(this.cbTimeGroup3);
            this.tabPage1.Controls.Add(this.cbTimeGroup2);
            this.tabPage1.Controls.Add(this.cbTimeGroup1);
            this.tabPage1.Controls.Add(this.txtCardData);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.butDeleteFromList);
            this.tabPage1.Controls.Add(this.butAddAll);
            this.tabPage1.Controls.Add(this.butAddToDevice);
            this.tabPage1.Controls.Add(this.butAddToList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 176);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "订餐操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dtpReservationDate
            // 
            this.dtpReservationDate.Location = new System.Drawing.Point(281, 11);
            this.dtpReservationDate.Name = "dtpReservationDate";
            this.dtpReservationDate.Size = new System.Drawing.Size(126, 21);
            this.dtpReservationDate.TabIndex = 24;
            // 
            // cbTimeGroup8
            // 
            this.cbTimeGroup8.AutoSize = true;
            this.cbTimeGroup8.Location = new System.Drawing.Point(577, 45);
            this.cbTimeGroup8.Name = "cbTimeGroup8";
            this.cbTimeGroup8.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup8.TabIndex = 23;
            this.cbTimeGroup8.Text = "时段8";
            this.cbTimeGroup8.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup7
            // 
            this.cbTimeGroup7.AutoSize = true;
            this.cbTimeGroup7.Location = new System.Drawing.Point(507, 45);
            this.cbTimeGroup7.Name = "cbTimeGroup7";
            this.cbTimeGroup7.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup7.TabIndex = 22;
            this.cbTimeGroup7.Text = "时段7";
            this.cbTimeGroup7.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup6
            // 
            this.cbTimeGroup6.AutoSize = true;
            this.cbTimeGroup6.Location = new System.Drawing.Point(436, 45);
            this.cbTimeGroup6.Name = "cbTimeGroup6";
            this.cbTimeGroup6.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup6.TabIndex = 21;
            this.cbTimeGroup6.Text = "时段6";
            this.cbTimeGroup6.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup5
            // 
            this.cbTimeGroup5.AutoSize = true;
            this.cbTimeGroup5.Location = new System.Drawing.Point(368, 45);
            this.cbTimeGroup5.Name = "cbTimeGroup5";
            this.cbTimeGroup5.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup5.TabIndex = 20;
            this.cbTimeGroup5.Text = "时段5";
            this.cbTimeGroup5.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup4
            // 
            this.cbTimeGroup4.AutoSize = true;
            this.cbTimeGroup4.Location = new System.Drawing.Point(300, 45);
            this.cbTimeGroup4.Name = "cbTimeGroup4";
            this.cbTimeGroup4.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup4.TabIndex = 19;
            this.cbTimeGroup4.Text = "时段4";
            this.cbTimeGroup4.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup3
            // 
            this.cbTimeGroup3.AutoSize = true;
            this.cbTimeGroup3.Location = new System.Drawing.Point(227, 45);
            this.cbTimeGroup3.Name = "cbTimeGroup3";
            this.cbTimeGroup3.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup3.TabIndex = 18;
            this.cbTimeGroup3.Text = "时段3";
            this.cbTimeGroup3.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup2
            // 
            this.cbTimeGroup2.AutoSize = true;
            this.cbTimeGroup2.Location = new System.Drawing.Point(152, 45);
            this.cbTimeGroup2.Name = "cbTimeGroup2";
            this.cbTimeGroup2.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup2.TabIndex = 17;
            this.cbTimeGroup2.Text = "时段2";
            this.cbTimeGroup2.UseVisualStyleBackColor = true;
            // 
            // cbTimeGroup1
            // 
            this.cbTimeGroup1.AutoSize = true;
            this.cbTimeGroup1.Location = new System.Drawing.Point(83, 45);
            this.cbTimeGroup1.Name = "cbTimeGroup1";
            this.cbTimeGroup1.Size = new System.Drawing.Size(54, 16);
            this.cbTimeGroup1.TabIndex = 16;
            this.cbTimeGroup1.Text = "时段1";
            this.cbTimeGroup1.UseVisualStyleBackColor = true;
            // 
            // txtCardData
            // 
            this.txtCardData.Location = new System.Drawing.Point(83, 11);
            this.txtCardData.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtCardData.Name = "txtCardData";
            this.txtCardData.Size = new System.Drawing.Size(92, 21);
            this.txtCardData.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "餐段权限：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "订餐日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "卡号：";
            // 
            // butDeleteFromList
            // 
            this.butDeleteFromList.Location = new System.Drawing.Point(473, 104);
            this.butDeleteFromList.Name = "butDeleteFromList";
            this.butDeleteFromList.Size = new System.Drawing.Size(109, 23);
            this.butDeleteFromList.TabIndex = 4;
            this.butDeleteFromList.Text = "从列表删除";
            this.butDeleteFromList.UseVisualStyleBackColor = true;
            this.butDeleteFromList.Click += new System.EventHandler(this.butDeleteFromList_Click);
            // 
            // butAddAll
            // 
            this.butAddAll.Location = new System.Drawing.Point(323, 104);
            this.butAddAll.Name = "butAddAll";
            this.butAddAll.Size = new System.Drawing.Size(109, 23);
            this.butAddAll.TabIndex = 2;
            this.butAddAll.Text = "上传所有订餐";
            this.butAddAll.UseVisualStyleBackColor = true;
            this.butAddAll.Click += new System.EventHandler(this.butAddAll_Click);
            // 
            // butAddToDevice
            // 
            this.butAddToDevice.Location = new System.Drawing.Point(166, 104);
            this.butAddToDevice.Name = "butAddToDevice";
            this.butAddToDevice.Size = new System.Drawing.Size(109, 23);
            this.butAddToDevice.TabIndex = 1;
            this.butAddToDevice.Text = "添加至控制器";
            this.butAddToDevice.UseVisualStyleBackColor = true;
            this.butAddToDevice.Click += new System.EventHandler(this.butAddToDevice_Click);
            // 
            // butAddToList
            // 
            this.butAddToList.Location = new System.Drawing.Point(14, 104);
            this.butAddToList.Name = "butAddToList";
            this.butAddToList.Size = new System.Drawing.Size(109, 23);
            this.butAddToList.TabIndex = 0;
            this.butAddToList.Text = "添加至列表";
            this.butAddToList.UseVisualStyleBackColor = true;
            this.butAddToList.Click += new System.EventHandler(this.butAddToList_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtStartCode);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtCreateCount);
            this.tabPage2.Controls.Add(this.butCreateByRandom);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 176);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "批量操作";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtStartCode
            // 
            this.txtStartCode.Location = new System.Drawing.Point(109, 43);
            this.txtStartCode.Name = "txtStartCode";
            this.txtStartCode.Size = new System.Drawing.Size(120, 21);
            this.txtStartCode.TabIndex = 58;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 57;
            this.label5.Text = "代码起始数值：";
            // 
            // txtCreateCount
            // 
            this.txtCreateCount.Location = new System.Drawing.Point(109, 11);
            this.txtCreateCount.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtCreateCount.Name = "txtCreateCount";
            this.txtCreateCount.Size = new System.Drawing.Size(120, 21);
            this.txtCreateCount.TabIndex = 56;
            // 
            // butCreateByRandom
            // 
            this.butCreateByRandom.Location = new System.Drawing.Point(24, 79);
            this.butCreateByRandom.Name = "butCreateByRandom";
            this.butCreateByRandom.Size = new System.Drawing.Size(99, 23);
            this.butCreateByRandom.TabIndex = 53;
            this.butCreateByRandom.Text = "生成顺序商品";
            this.butCreateByRandom.UseVisualStyleBackColor = true;
            this.butCreateByRandom.Click += new System.EventHandler(this.butCreateByRandom_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(30, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 55;
            this.label18.Text = "生成数量：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butClearList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 391);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 54);
            this.panel1.TabIndex = 5;
            // 
            // butClearList
            // 
            this.butClearList.Location = new System.Drawing.Point(713, 12);
            this.butClearList.Name = "butClearList";
            this.butClearList.Size = new System.Drawing.Size(75, 23);
            this.butClearList.TabIndex = 0;
            this.butClearList.Text = "清空表格";
            this.butClearList.UseVisualStyleBackColor = true;
            this.butClearList.Click += new System.EventHandler(this.butClearList_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butClearDataBase);
            this.groupBox1.Controls.Add(this.butReadAllMenu);
            this.groupBox1.Controls.Add(this.butReadDataBase);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 52);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "订餐操作";
            // 
            // butClearDataBase
            // 
            this.butClearDataBase.Location = new System.Drawing.Point(693, 20);
            this.butClearDataBase.Name = "butClearDataBase";
            this.butClearDataBase.Size = new System.Drawing.Size(75, 23);
            this.butClearDataBase.TabIndex = 2;
            this.butClearDataBase.Text = "清空订餐";
            this.butClearDataBase.UseVisualStyleBackColor = true;
            this.butClearDataBase.Click += new System.EventHandler(this.butClearDataBase_Click);
            // 
            // butReadAllMenu
            // 
            this.butReadAllMenu.Location = new System.Drawing.Point(361, 21);
            this.butReadAllMenu.Name = "butReadAllMenu";
            this.butReadAllMenu.Size = new System.Drawing.Size(75, 23);
            this.butReadAllMenu.TabIndex = 1;
            this.butReadAllMenu.Text = "读取订餐";
            this.butReadAllMenu.UseVisualStyleBackColor = true;
            this.butReadAllMenu.Click += new System.EventHandler(this.butReadAllMenu_Click);
            // 
            // butReadDataBase
            // 
            this.butReadDataBase.Location = new System.Drawing.Point(18, 20);
            this.butReadDataBase.Name = "butReadDataBase";
            this.butReadDataBase.Size = new System.Drawing.Size(94, 23);
            this.butReadDataBase.TabIndex = 0;
            this.butReadDataBase.Text = "读取订餐信息";
            this.butReadDataBase.UseVisualStyleBackColor = true;
            this.butReadDataBase.Click += new System.EventHandler(this.butReadDataBase_Click);
            // 
            // dgvReservation
            // 
            this.dgvReservation.AllowUserToAddRows = false;
            this.dgvReservation.AllowUserToDeleteRows = false;
            this.dgvReservation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5});
            this.dgvReservation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReservation.Location = new System.Drawing.Point(0, 52);
            this.dgvReservation.Name = "dgvReservation";
            this.dgvReservation.ReadOnly = true;
            this.dgvReservation.RowHeadersVisible = false;
            this.dgvReservation.RowTemplate.Height = 23;
            this.dgvReservation.Size = new System.Drawing.Size(800, 339);
            this.dgvReservation.TabIndex = 7;
            this.dgvReservation.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReservation_CellClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "选择";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "CardData";
            this.Column2.HeaderText = "卡号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "ReservationDate";
            this.Column3.HeaderText = "订餐日期";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ShowTimeGroup";
            this.Column5.HeaderText = "定额餐段权限";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 300;
            // 
            // FrmReservation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 647);
            this.Controls.Add(this.dgvReservation);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmReservation";
            this.Text = "订餐";
            this.Load += new System.EventHandler(this.FrmReservation_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardData)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox cbTimeGroup8;
        private System.Windows.Forms.CheckBox cbTimeGroup7;
        private System.Windows.Forms.CheckBox cbTimeGroup6;
        private System.Windows.Forms.CheckBox cbTimeGroup5;
        private System.Windows.Forms.CheckBox cbTimeGroup4;
        private System.Windows.Forms.CheckBox cbTimeGroup3;
        private System.Windows.Forms.CheckBox cbTimeGroup2;
        private System.Windows.Forms.CheckBox cbTimeGroup1;
        private System.Windows.Forms.NumericUpDown txtCardData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butDeleteFromList;
        private System.Windows.Forms.Button butAddAll;
        private System.Windows.Forms.Button butAddToDevice;
        private System.Windows.Forms.Button butAddToList;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown txtStartCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown txtCreateCount;
        private System.Windows.Forms.Button butCreateByRandom;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butClearList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butClearDataBase;
        private System.Windows.Forms.Button butReadAllMenu;
        private System.Windows.Forms.Button butReadDataBase;
        private System.Windows.Forms.DataGridView dgvReservation;
        private System.Windows.Forms.DateTimePicker dtpReservationDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}