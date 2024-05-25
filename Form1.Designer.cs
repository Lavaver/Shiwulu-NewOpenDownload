namespace OpenDownload.NET
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            新建新任务ToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            请求头ToolStripMenuItem = new ToolStripMenuItem();
            默认代理请求头配置按钮 = new ToolStripMenuItem();
            百度网盘专用代理请求头配置按钮 = new ToolStripMenuItem();
            用户自定义代理配置请求头按钮 = new ToolStripMenuItem();
            listView1 = new ListView();
            源地址columnHeader = new ColumnHeader();
            状态columnHeader = new ColumnHeader();
            保存路径columnHeader = new ColumnHeader();
            Status = new ToolStripStatusLabel();
            statusStrip1 = new StatusStrip();
            toolStripButton1 = new ToolStripButton();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton3, toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1353, 27);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { 新建新任务ToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(83, 24);
            toolStripDropDownButton1.Text = "下载操作";
            toolStripDropDownButton1.ToolTipText = "修改下载时行为";
            // 
            // 新建新任务ToolStripMenuItem
            // 
            新建新任务ToolStripMenuItem.Image = (Image)resources.GetObject("新建新任务ToolStripMenuItem.Image");
            新建新任务ToolStripMenuItem.Name = "新建新任务ToolStripMenuItem";
            新建新任务ToolStripMenuItem.Size = new Size(167, 26);
            新建新任务ToolStripMenuItem.Text = "新建新任务";
            新建新任务ToolStripMenuItem.Click += 新建新任务ToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { 请求头ToolStripMenuItem });
            toolStripDropDownButton3.Image = (Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Size = new Size(53, 24);
            toolStripDropDownButton3.Text = "配置";
            // 
            // 请求头ToolStripMenuItem
            // 
            请求头ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 默认代理请求头配置按钮, 百度网盘专用代理请求头配置按钮, 用户自定义代理配置请求头按钮 });
            请求头ToolStripMenuItem.Image = (Image)resources.GetObject("请求头ToolStripMenuItem.Image");
            请求头ToolStripMenuItem.Name = "请求头ToolStripMenuItem";
            请求头ToolStripMenuItem.Size = new Size(197, 26);
            请求头ToolStripMenuItem.Text = "用户代理请求头";
            请求头ToolStripMenuItem.ToolTipText = "配置用户代理请求头（User-Agent），以避免可能的拒绝访问";
            // 
            // 默认代理请求头配置按钮
            // 
            默认代理请求头配置按钮.Checked = true;
            默认代理请求头配置按钮.CheckState = CheckState.Checked;
            默认代理请求头配置按钮.Name = "默认代理请求头配置按钮";
            默认代理请求头配置按钮.Size = new Size(227, 26);
            默认代理请求头配置按钮.Text = "默认";
            默认代理请求头配置按钮.ToolTipText = "使用默认请求头下载（注意：使用该代理在百度网盘下载会抛出 403 禁止访问异常）\r\n";
            默认代理请求头配置按钮.Click += mozilla50WindowsNT100Win64X64AppleWebKit53736KHTMLLikeGeckoChrome107000Safari53736ToolStripMenuItem_Click;
            // 
            // 百度网盘专用代理请求头配置按钮
            // 
            百度网盘专用代理请求头配置按钮.Enabled = false;
            百度网盘专用代理请求头配置按钮.Name = "百度网盘专用代理请求头配置按钮";
            百度网盘专用代理请求头配置按钮.Size = new Size(227, 26);
            百度网盘专用代理请求头配置按钮.Text = "百度网盘专用请求头";
            百度网盘专用代理请求头配置按钮.ToolTipText = "（正在开发）面向百度网盘特殊需求下载的专用用户代理请求头";
            // 
            // 用户自定义代理配置请求头按钮
            // 
            用户自定义代理配置请求头按钮.Name = "用户自定义代理配置请求头按钮";
            用户自定义代理配置请求头按钮.Size = new Size(227, 26);
            用户自定义代理配置请求头按钮.Text = "用户自定义";
            用户自定义代理配置请求头按钮.ToolTipText = "配置自定义代理请求头以在特殊下载时避免异常";
            用户自定义代理配置请求头按钮.Click += 用户自定义代理配置请求头按钮_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { 源地址columnHeader, 状态columnHeader, 保存路径columnHeader });
            listView1.Location = new Point(12, 30);
            listView1.Name = "listView1";
            listView1.Size = new Size(1329, 595);
            listView1.TabIndex = 2;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // 源地址columnHeader
            // 
            源地址columnHeader.Text = "源地址";
            源地址columnHeader.Width = 300;
            // 
            // 状态columnHeader
            // 
            状态columnHeader.Text = "下载状态";
            状态columnHeader.Width = 100;
            // 
            // 保存路径columnHeader
            // 
            保存路径columnHeader.Text = "保存路径";
            保存路径columnHeader.Width = 315;
            // 
            // Status
            // 
            Status.Name = "Status";
            Status.Size = new Size(0, 16);
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { Status });
            statusStrip1.Location = new Point(0, 628);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1353, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(43, 24);
            toolStripButton1.Text = "关于";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 650);
            Controls.Add(listView1);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shiwulu OpenDownload 2";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem 新建新任务ToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripMenuItem 请求头ToolStripMenuItem;
        private ToolStripMenuItem 默认代理请求头配置按钮;
        private ToolStripMenuItem 用户自定义代理配置请求头按钮;
        private ListView listView1;
        private ToolStripStatusLabel Status;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem 百度网盘专用代理请求头配置按钮;
        private ColumnHeader 源地址columnHeader;
        private ColumnHeader 状态columnHeader;
        private ColumnHeader 保存路径columnHeader;
        private ToolStripButton toolStripButton1;
    }
}
