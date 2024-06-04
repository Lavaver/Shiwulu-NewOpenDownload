namespace OpenDownload.NET
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            button1 = new Button();
            label5 = new Label();
            label4 = new Label();
            groupBox2 = new GroupBox();
            DotNetCore_VersionStatus = new Label();
            label6 = new Label();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(209, 183);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 20F);
            label1.Location = new Point(243, 38);
            label1.Name = "label1";
            label1.Size = new Size(416, 45);
            label1.TabIndex = 1;
            label1.Text = "Shiwulu OpenDownload";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(253, 137);
            label2.Name = "label2";
            label2.Size = new Size(306, 23);
            label2.TabIndex = 2;
            label2.Text = "Release 2.5（单击此处查看更新日志）";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 12F);
            label3.Location = new Point(253, 97);
            label3.Name = "label3";
            label3.Size = new Size(391, 27);
            label3.TabIndex = 3;
            label3.Text = "A .NET Core-based Free Download Tool";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label5);
            groupBox1.Location = new Point(12, 214);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(950, 77);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "开发者";
            // 
            // button1
            // 
            button1.Location = new Point(765, 32);
            button1.Name = "button1";
            button1.Size = new Size(155, 29);
            button1.TabIndex = 7;
            button1.Text = "MIT 许可证信息";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 10F);
            label5.Location = new Point(24, 34);
            label5.Name = "label5";
            label5.Size = new Size(608, 23);
            label5.TabIndex = 6;
            label5.Text = "Lavaver（规划、软件设计、程序、原 Java 版作者，也是这款软件唯一的作者）";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 10F);
            label4.Location = new Point(253, 172);
            label4.Name = "label4";
            label4.Size = new Size(162, 23);
            label4.TabIndex = 5;
            label4.Text = "Public Build 20055";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(DotNetCore_VersionStatus);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new Point(12, 297);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(950, 73);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = ".NET Core 版本信息";
            // 
            // DotNetCore_VersionStatus
            // 
            DotNetCore_VersionStatus.AutoSize = true;
            DotNetCore_VersionStatus.Font = new Font("Microsoft YaHei UI", 10F);
            DotNetCore_VersionStatus.Location = new Point(485, 34);
            DotNetCore_VersionStatus.Name = "DotNetCore_VersionStatus";
            DotNetCore_VersionStatus.Size = new Size(0, 23);
            DotNetCore_VersionStatus.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 10F);
            label6.Location = new Point(24, 34);
            label6.Name = "label6";
            label6.Size = new Size(455, 23);
            label6.TabIndex = 7;
            label6.Text = "该软件需求 .NET Core 8 及以上版本，你正在运行的版本为";
            // 
            // button2
            // 
            button2.Location = new Point(777, 390);
            button2.Name = "button2";
            button2.Size = new Size(155, 29);
            button2.TabIndex = 8;
            button2.Text = "关闭";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 441);
            Controls.Add(button2);
            Controls.Add(groupBox2);
            Controls.Add(label4);
            Controls.Add(groupBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "About";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "关于 OpenDownload";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private GroupBox groupBox2;
        private Label DotNetCore_VersionStatus;
        private Label label6;
        private Button button1;
        private Button button2;
    }
}