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
    public partial class User_Agent_Config_FormPage : Form
    {
        public Form1 MainForm { get; set; }

        public User_Agent_Config_FormPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 估计没有人会使用 Java 版下载吧...？Java 版最后一次更新已经是三月份的事情了（主要是 Java 版下载一堆问题已经快被它搞疯了），如果真的有人拆 jar 包改 class 的 User-Agent 配置可能不会用这个版本了（）
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (MainForm != null)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    MainForm.UpdateUserAgent(textBox1.Text);
                    this.Close();
                    MessageBox.Show("已更改 User-Agent 配置。所有的配置将在新的下载开始时生效。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("User-Agent 配置未被更改。因为你未输入新的配置。", "配置未被更改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
