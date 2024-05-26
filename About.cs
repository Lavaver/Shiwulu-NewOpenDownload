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
            MessageBox.Show("Release 1.2 (Public Build 10025) 更新日志：\n- 添加下载速度和瞬时速度显示，下载进度精细到小数点后两位\n- 修改了主页面 ListView 列宽度的占比，改善视觉体验\n有关该软件发行版的详细信息，请参阅 https://github.com/Lavaver/Shiwulu-NewOpenDownload/releases 。", "Release 1.2 (Public Build 10025) 更新日志",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
