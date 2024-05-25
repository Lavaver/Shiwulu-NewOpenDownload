namespace OpenDownload.NET
{
    partial class User_Agent_Config_FormPage
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
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.DarkGoldenrod;
            label1.Location = new Point(21, 21);
            label1.Name = "label1";
            label1.Size = new Size(629, 60);
            label1.TabIndex = 0;
            label1.Text = "注意：\r\n错误配置用户代理请求头可能导致 403 拒绝访问错误，若你不知道该项作用，请使用默认配置\r\n所有的 User-Agent 更改将在新的下载任务中生效";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(21, 94);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "键入用户代理请求头（User-Agent）";
            textBox1.Size = new Size(507, 27);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(534, 94);
            button1.Name = "button1";
            button1.Size = new Size(124, 27);
            button1.TabIndex = 2;
            button1.Text = "应用配置";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // User_Agent_Config_FormPage
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(675, 144);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "User_Agent_Config_FormPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "配置用户代理请求头（User-Agent）";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
    }
}