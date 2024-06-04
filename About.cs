using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDownload.NET
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            DotNetCore_VersionStatus.Text = $"{Environment.Version}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MIT_License mIT_License = new();
            mIT_License.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2.5 更新日志\n- 关于页面重构\n- 修复常规 bug", "2.5 更新日志",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
