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
                百度网盘专用代理请求头配置按钮.Checked = false;
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
            百度网盘专用代理请求头配置按钮.Checked = false;
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
            ListViewItem newItem = null;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"下载失败：{response.StatusCode}");
                    }

                    newItem = new ListViewItem(sourceUrl);
                    newItem.SubItems.Add(savePath);
                    newItem.SubItems.Add("0%"); // 下载进度子项
                    newItem.SubItems.Add("0 KB/s"); // 下载速度子项
                    newItem.SubItems.Add("0 KB/s"); // 瞬时速度子项
                    listView1.Items.Add(newItem);

                    await DownloadFileWithProgressAsync(sourceUrl, savePath, response, newItem);

                    newItem.SubItems[2].Text = "完成";
                    newItem.SubItems[3].Text = ""; // 清空下载速度子项
                }
                catch (Exception ex)
                {
                    MessageBox.Show("文件下载失败，日志已保存至 /Exception/{time}.log 中", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CrashLogSpawn(ex);
                    if (newItem != null)
                    {
                        newItem.SubItems[1].Text = "错误";
                        newItem.SubItems[3].Text = ""; // 清空下载速度子项
                    }
                }
            }
        }


        private string CalculateDownloadSpeed(double speed)
        {
            string[] suffixes = { "B/s", "KB/s", "MB/s", "GB/s", "TB/s" };

            int suffixIndex = 0;
            while (speed >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                speed /= 1024;
                suffixIndex++;
            }

            return $"{speed:0.##} {suffixes[suffixIndex]}";
        }

        /// <summary>
        /// 出现异常生成堆栈日志的逻辑。保存路径为该软件目录下的 /Exception/{time}.log 中。不用异步是因为生成过程不需要占用过多时间，往往都是一瞬间记录好随时可溯源的，不搞 JSON 格式是因为拿给开发人员看也得查个半天（被 VS Code 错误消息复制后那一长串 JSON 害惨了）
        /// </summary>
        /// <param name="ex">任何异常</param>
        static void CrashLogSpawn(Exception ex)
        {
            // 记录异常发生时间
            string time = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            // 生成带随机字符串的日志文件路径
            string randomString = GenerateRandomString(8);
            string logFileName = $"{time}_{randomString}_Exception.log";
            string logFilePath = $"./Exception/{logFileName}";

            try
            {
                // 如果异常目录不存在，则创建
                Directory.CreateDirectory("./Exception/");

                // 将异常信息写入日志文件
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine("Shiwulu Opendownload .NET 错误与堆栈日志");
                    sw.WriteLine("---------------------------------");
                    sw.WriteLine($"异常发生时间：{time}");
                    sw.WriteLine($"异常信息：{ex.Message}");
                    sw.WriteLine($"异常堆栈：{ex.StackTrace}");
                    sw.WriteLine("---------------------------------");
                    sw.WriteLine("*** 引发异常的上一位置中堆栈跟踪的末尾 ***");
                    sw.WriteLine(ex);
                    sw.WriteLine("---------------------------------");
                }
            }
            catch (Exception)
            {

            }
        }

        // 生成指定长度的随机字符串
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task DownloadFileWithProgressAsync(string sourceUrl, string savePath, HttpResponseMessage response, ListViewItem listItem)
        {
            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
            {
                using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;

                    long totalDownloadedBytes = 0;
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    double progress = 0;
                    DateTime startTime = DateTime.Now;
                    DateTime previousTime = DateTime.Now;
                    long previousDownloadedBytes = 0;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalDownloadedBytes += bytesRead;

                        progress = (double)totalDownloadedBytes / totalBytes * 100;
                        listItem.SubItems[2].Text = $"{progress:F2}%";

                        TimeSpan elapsedTime = DateTime.Now - startTime;
                        double speed = totalDownloadedBytes / elapsedTime.TotalSeconds;
                        string speedStr = CalculateDownloadSpeed(speed);
                        listItem.SubItems[3].Text = speedStr;

                        TimeSpan elapsedSincePrevious = DateTime.Now - previousTime;
                        long downloadedBytesSincePrevious = totalDownloadedBytes - previousDownloadedBytes;
                        double instantSpeed = downloadedBytesSincePrevious / elapsedSincePrevious.TotalSeconds;
                        string instantSpeedStr = CalculateDownloadSpeed(instantSpeed);
                        listItem.SubItems[4].Text = instantSpeedStr;

                        previousTime = DateTime.Now;
                        previousDownloadedBytes = totalDownloadedBytes;
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
                    item.SubItems[2].Text = $"{progressPercentage}%";
                    break;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            About about = new();
            about.ShowDialog();
        }

        private void 百度网盘专用代理请求头配置按钮_Click(object sender, EventArgs e)
        {
            if (!百度网盘专用代理请求头配置按钮.Checked)
            {
                UserAgentConfig = "netdisk;P2SP;2.2.60.26";
                MessageBox.Show("已更改 User-Agent 配置。所有的配置将在新的下载开始时生效。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                用户自定义代理配置请求头按钮.Text = "用户自定义";
                默认代理请求头配置按钮.Checked = false;
                用户自定义代理配置请求头按钮.Checked = false;
                百度网盘专用代理请求头配置按钮.Checked = true;
            }
            else
            {
                MessageBox.Show("User-Agent 配置未被更改。因为已勾选预设的配置。", "配置未被更改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
