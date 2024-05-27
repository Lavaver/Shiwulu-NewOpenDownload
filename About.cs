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
            MessageBox.Show("Release 2.1 (Public Build 20015) 更新日志：\n- 添加若有下载任务时尝试关闭软件会弹窗警告是否继续关闭\n有关该软件发行版的详细信息，请参阅 https://github.com/Lavaver/Shiwulu-NewOpenDownload/releases 。", "Release 2.1 (Public Build 20015) 更新日志",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
