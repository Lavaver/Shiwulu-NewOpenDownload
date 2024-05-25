using System.Net;
using System.Net.NetworkInformation;

namespace OpenDownload.NET
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 用户代理请求头（User-Agent）配置。默认为 Chrome 浏览器的默认用户代理。这里表明代理请求头是为了部分下载抛出 403 错误状态码导致程序抛出异常。对于百度网盘后续专门集中精力去尝试获得请求头加入到软件预设代理中（关于百度网盘搞特殊请求头我只能说“百度，你有点极端了”）。
        /// </summary>
        public string UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

        private New_Task newTaskForm;
        public Form1()
        {
            InitializeComponent();
            Status.Text = "准备就绪";
            newTaskForm = new New_Task();
            newTaskForm.DownloadStarted += OnDownloadStarted;
        }

        private void mozilla50WindowsNT100Win64X64AppleWebKit53736KHTMLLikeGeckoChrome107000Safari53736ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!默认代理请求头配置按钮.Checked)
            {
                UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
                MessageBox.Show("已恢复 User-Agent 配置。所有的配置将在新的下载开始时生效。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                用户自定义代理配置请求头按钮.Text = "用户自定义";
                默认代理请求头配置按钮.Checked = true;
                用户自定义代理配置请求头按钮.Checked = false;
            }
            else
            {
                MessageBox.Show("User-Agent 配置未被更改。因为已勾选预设的配置。", "配置未被更改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void 用户自定义代理配置请求头按钮_Click(object sender, EventArgs e)
        {
            User_Agent_Config_FormPage ConfigPage = new();
            ConfigPage.MainForm = this;
            ConfigPage.ShowDialog();
        }

        public void UpdateUserAgent(string newAgent)
        {
            UserAgentConfig = newAgent;
            默认代理请求头配置按钮.Checked = false;
            用户自定义代理配置请求头按钮.Checked = true;
            用户自定义代理配置请求头按钮.Text = $"已自定义配置为 {newAgent} ，点击以修改配置";
        }

        private void 新建新任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newTaskForm.ShowDialog();
        }
        private async void OnDownloadStarted(object sender, DownloadEventArgs e)
        {
            string sourceUrl = e.SourceUrl;
            string savePath = e.SavePath;

            await DownloadFileAsync(sourceUrl, savePath);
        }

        private async Task DownloadFileAsync(string sourceUrl, string savePath)
        {
            ListViewItem newItem = null; // 在 try 代码块之外定义 newItem

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"下载失败：{response.StatusCode}");
                    }

                    // 创建一个新的 ListViewItem 对象
                    newItem = new ListViewItem(sourceUrl);
                    // 添加子项
                    newItem.SubItems.Add("0%"); // 添加用于显示下载进度的子项
                    newItem.SubItems.Add(savePath);

                    // 将新项添加到 ListView 控件中
                    listView1.Items.Add(newItem);

                    // 下载文件并更新进度
                    await DownloadFileWithProgressAsync(sourceUrl, savePath, response, newItem);

                    // 标记下载完成
                    newItem.SubItems[1].Text = "完成";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"文件下载失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (newItem != null)
                    {
                        newItem.SubItems[1].Text = "错误";
                    }
                }
            }
        }

        private async Task DownloadFileWithProgressAsync(string sourceUrl, string savePath, HttpResponseMessage response, ListViewItem item)
        {
            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
            {
                long? contentLength = response.Content.Headers.ContentLength;

                using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    long bytesDownloaded = 0;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);

                        bytesDownloaded += bytesRead;
                        if (contentLength.HasValue)
                        {
                            int progressPercentage = (int)(bytesDownloaded * 100 / contentLength.Value);

                            // 确保在UI线程上执行更新操作
                            listView1.Invoke((MethodInvoker)delegate
                            {
                                item.SubItems[1].Text = $"{progressPercentage}%";
                            });
                        }
                    }
                }
            }
        }

        private void UpdateProgress(string sourceUrl, int progressPercentage)
        {
            // 查找相应的项并更新进度
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == sourceUrl)
                {
                    item.SubItems[1].Text = $"{progressPercentage}%";
                    break;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            About about = new();
            about.ShowDialog();
        }
    }
}
