namespace DoNetDrive.Protocol.Door.Test
{
    partial class frmUploadSoftware
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
            this.label1 = new System.Windows.Forms.Label();
            this.Txt_Ver = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Txt_CRC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Txt_FliePath = new System.Windows.Forms.TextBox();
            this.Btn_Select = new System.Windows.Forms.Button();
            this.Btn_Upload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "固件版本号";
            // 
            // Txt_Ver
            // 
            this.Txt_Ver.Enabled = false;
            this.Txt_Ver.Location = new System.Drawing.Point(78, 21);
            this.Txt_Ver.Name = "Txt_Ver";
            this.Txt_Ver.Size = new System.Drawing.Size(100, 21);
            this.Txt_Ver.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "固件CRC";
            // 
            // Txt_CRC
            // 
            this.Txt_CRC.Enabled = false;
            this.Txt_CRC.Location = new System.Drawing.Point(268, 22);
            this.Txt_CRC.Name = "Txt_CRC";
            this.Txt_CRC.Size = new System.Drawing.Size(100, 21);
            this.Txt_CRC.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "文件地址：";
            // 
            // Txt_FliePath
            // 
            this.Txt_FliePath.Enabled = false;
            this.Txt_FliePath.Location = new System.Drawing.Point(78, 63);
            this.Txt_FliePath.Multiline = true;
            this.Txt_FliePath.Name = "Txt_FliePath";
            this.Txt_FliePath.Size = new System.Drawing.Size(290, 66);
            this.Txt_FliePath.TabIndex = 1;
            // 
            // Btn_Select
            // 
            this.Btn_Select.Location = new System.Drawing.Point(78, 147);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(75, 23);
            this.Btn_Select.TabIndex = 2;
            this.Btn_Select.Text = "选择固件";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // Btn_Upload
            // 
            this.Btn_Upload.Location = new System.Drawing.Point(293, 147);
            this.Btn_Upload.Name = "Btn_Upload";
            this.Btn_Upload.Size = new System.Drawing.Size(75, 23);
            this.Btn_Upload.TabIndex = 3;
            this.Btn_Upload.Text = "上传固件";
            this.Btn_Upload.UseVisualStyleBackColor = true;
            this.Btn_Upload.Click += new System.EventHandler(this.Btn_Upload_Click);
            // 
            // frmUploadSoftware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 199);
            this.Controls.Add(this.Btn_Upload);
            this.Controls.Add(this.Btn_Select);
            this.Controls.Add(this.Txt_FliePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Txt_CRC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Txt_Ver);
            this.Controls.Add(this.label1);
            this.Name = "frmUploadSoftware";
            this.Text = "固件更新";
            this.Load += new System.EventHandler(this.frmUploadSoftware_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Txt_Ver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Txt_CRC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Txt_FliePath;
        private System.Windows.Forms.Button Btn_Select;
        private System.Windows.Forms.Button Btn_Upload;
    }
}