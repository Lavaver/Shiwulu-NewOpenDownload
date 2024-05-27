using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDownload.NET
{
    partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }



        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelProductName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Release 2.0 (Public Build 20000) 更新日志：\n- 添加多区块下载选项（后续可能会做自定义分隔区块量）\n- 任务栏小图标气球通知经典回归（但和原 Java 版少了有关文件大小的正文内容）\n有关该软件发行版的详细信息，请参阅 https://github.com/Lavaver/Shiwulu-NewOpenDownload/releases 。", "Release 2.0 (Public Build 20000) 更新日志",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
