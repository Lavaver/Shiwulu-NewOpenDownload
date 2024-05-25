namespace OpenDownload.NET
{
    partial class New_Task
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
            label2 = new Label();
            textBox2 = new TextBox();
            button1 = new Button();
            启动下载按钮 = new Button();
            label3 = new Label();
            Spacelabel = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 27);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 0;
            label1.Text = "下载地址";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(117, 24);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "例如 https://example.com/example.txt";
            textBox1.Size = new Size(679, 27);
            textBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 75);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 2;
            label2.Text = "存储路径";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(117, 72);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(583, 27);
            textBox2.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(706, 71);
            button1.Name = "button1";
            button1.Size = new Size(90, 29);
            button1.TabIndex = 4;
            button1.Text = "选择...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // 启动下载按钮
            // 
            启动下载按钮.Enabled = false;
            启动下载按钮.Location = new Point(608, 116);
            启动下载按钮.Name = "启动下载按钮";
            启动下载按钮.Size = new Size(188, 36);
            启动下载按钮.TabIndex = 5;
            启动下载按钮.Text = "启动下载";
            启动下载按钮.UseVisualStyleBackColor = true;
            启动下载按钮.Click += 启动下载按钮_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 124);
            label3.Name = "label3";
            label3.Size = new Size(99, 20);
            label3.TabIndex = 6;
            label3.Text = "剩余存储空间";
            // 
            // Spacelabel
            // 
            Spacelabel.AutoSize = true;
            Spacelabel.Location = new Point(131, 124);
            Spacelabel.Name = "Spacelabel";
            Spacelabel.Size = new Size(0, 20);
            Spacelabel.TabIndex = 7;
            // 
            // New_Task
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(836, 164);
            Controls.Add(Spacelabel);
            Controls.Add(label3);
            Controls.Add(启动下载按钮);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "New_Task";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "创建新任务";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Button button1;
        private Button 启动下载按钮;
        private Label label3;
        private Label Spacelabel;
    }
}