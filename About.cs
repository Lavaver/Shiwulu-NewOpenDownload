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
            MessageBox.Show("Release 1.1 (Public Build 10015) 更新日志：\n- 对下载前异常行为进行更改，现已改为生成带有堆栈的日志文件，可帮助开发人员快速定位问题\n- 修改了主页面 ListView 列宽度的占比，改善视觉体验\n- 实装针对于百度网盘下载的 UA ，在下载前可选择避免 403 拒绝访问\n有关该软件发行版的详细信息，请参阅 https://github.com/Lavaver/Shiwulu-NewOpenDownload/releases 。", "Release 1.1 (Public Build 10015) 更新日志",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
