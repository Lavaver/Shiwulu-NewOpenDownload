using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace OpenDownload.NET
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 用于管理多区块下载各个区块的 Dictionary ，避免出现多区块下载时 listView 创建多个 Items 的尴尬问题（我对 Dotnet 开发非常自信（要不是 Java 彻夜写程序的场面堪比二战否则不会有这个版本），这种低级错误肯定不会犯的（笑））
        /// </summary>
        private Dictionary<Task, ListViewItem> downloadTaskMap = new Dictionary<Task, ListViewItem>();

        /// <summary>
        /// 用户代理请求头（User-Agent）配置。默认为 Chrome 浏览器的默认用户代理。这里表明代理请求头是为了部分下载抛出 403 错误状态码导致程序抛出异常。对于百度网盘也已经更新了预设代理选项可供直接使用。
        /// </summary>
        public string UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

        /// <summary>
        /// 创建一个 NotifyIcon 控件
        /// </summary>
        NotifyIcon notifyIcon = new NotifyIcon();

        /// <summary>
        /// 创建新任务窗口
        /// </summary>
        private New_Task newTaskForm;

        public Form1()
        {
            InitializeComponent();
            Status.Text = "准备就绪";
            newTaskForm = new New_Task();
            newTaskForm.DownloadStarted += OnDownloadStarted;
            // 设置 NotifyIcon 控件的属性
            notifyIcon.Icon = new Icon("icon.ico");
            notifyIcon.Visible = true;
        }

        #region 菜单项按钮方法 A

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

        #endregion

        private async void OnDownloadStarted(object sender, DownloadEventArgs e)
        {
            string sourceUrl = e.SourceUrl;
            string savePath = e.SavePath;

            if (单区块模式ToolStripMenuItem.Checked) 
            {
                await DownloadFileAsync(sourceUrl, savePath);
            }
            else if (多区块模式ToolStripMenuItem.Checked)
            {
                await MultiPartDownloadAsync(sourceUrl, savePath, 4);
            }
            
        }


        /// <summary>
        /// 单区块下载模式
        /// </summary>
        /// <param name="sourceUrl">来源 URL</param>
        /// <param name="savePath">保存文件地址</param>
        /// <returns></returns>
        private async Task DownloadFileAsync(string sourceUrl, string savePath)
        {
            ListViewItem newItem = null;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // 设置自定义 User-Agent
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgentConfig);

                    HttpResponseMessage response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead);

                    // 显示气泡提示窗口
                    notifyIcon.ShowBalloonTip(1000, $"任务 {savePath} 已开始下载", "返回到主界面以查看详细信息", ToolTipIcon.Info);

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
                    notifyIcon.ShowBalloonTip(1000, $"任务 {savePath} 已完成下载", "返回到主界面以查看详细信息", ToolTipIcon.Info);
                    newItem.SubItems[3].Text = ""; // 清空下载速度子项
                }
                catch (Exception ex)
                {
                    MessageBox.Show("文件下载失败，日志已保存至 /Exception/{time}.log 中", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    notifyIcon.ShowBalloonTip(1000, $"任务 {savePath} 失败", "日志已保存至 /Exception/{time}.log 中，可在有需要时使用", ToolTipIcon.Error);
                    CrashLogSpawn(ex);
                    if (newItem != null)
                    {
                        newItem.SubItems[1].Text = "错误";
                        newItem.SubItems[3].Text = ""; // 清空下载速度子项
                    }
                }
            }
        }

        /// <summary>
        /// 多区块下载模式
        /// </summary>
        /// <param name="sourceUrl">来源 URL</param>
        /// <param name="savePath">保存地址</param>
        /// <param name="numParts">区块量</param>
        /// <returns>完整文件</returns>


        private async Task MultiPartDownloadAsync(string sourceUrl, string savePath, int numParts)
        {
            ListViewItem newItem = new ListViewItem(sourceUrl);
            try
            {
                notifyIcon.ShowBalloonTip(1000, $"多区块任务 {savePath} 已开始下载", "返回到主界面以查看详细信息", ToolTipIcon.Info);
                long fileSize = await GetFileSizeAsync(sourceUrl);
                long partSize = fileSize / numParts;

                List<Task> downloadTasks = new List<Task>();

                for (int i = 0; i < numParts; i++)
                {
                    long startRange = i * partSize;
                    long endRange = (i == numParts - 1) ? fileSize - 1 : (i + 1) * partSize - 1;
                    string partSavePath = $"{savePath}.part{i}";

                    
                    newItem.SubItems.Add(savePath);
                    newItem.SubItems.Add("0%"); // 下载进度子项
                    newItem.SubItems.Add("0 KB/s"); // 下载速度子项
                    newItem.SubItems.Add("0 KB/s"); // 瞬时速度子项
                    listView1.Items.Add(newItem);

                    Task downloadTask = DownloadPartAsync(sourceUrl, partSavePath, startRange, endRange, newItem);
                    downloadTasks.Add(downloadTask);
                    downloadTaskMap.Add(downloadTask, newItem); // 将任务和对应的ListViewItem关联起来
                }

                await Task.WhenAll(downloadTasks);

                // 将所有部分文件合并为完整文件
                CombineFile(savePath, numParts);
                newItem.SubItems[2].Text = "完成";
                notifyIcon.ShowBalloonTip(1000, $"多区块任务 {savePath} 已完成下载", "返回到主界面以查看详细信息", ToolTipIcon.Info);
                newItem.SubItems[3].Text = ""; // 清空下载速度子项
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件下载失败，日志已保存至 /Exception/{time}.log 中", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                notifyIcon.ShowBalloonTip(1000, $"多区块任务 {savePath} 失败", "日志已保存至 /Exception/{time}.log 中，可在有需要时使用", ToolTipIcon.Error);
                CrashLogSpawn(ex);

                if (newItem != null)
                {
                    newItem.SubItems[1].Text = "错误";
                    newItem.SubItems[3].Text = ""; // 清空下载速度子项
                }
            }
        }

        /// <summary>
        /// 下载区块（这里的实时更新内容真的仅供参考...）
        /// </summary>
        /// <param name="sourceUrl">源 URL</param>
        /// <param name="savePath">保存地址</param>
        /// <param name="startRange">区块开始位置</param>
        /// <param name="endRange">区块结束位置</param>
        /// <param name="item">ListView 的项</param>
        /// <returns>区块文件</returns>
        private async Task DownloadPartAsync(string sourceUrl, string savePath, long startRange, long endRange, ListViewItem item)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgentConfig);
                httpClient.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(startRange, endRange);
                HttpResponseMessage response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead);

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        long totalBytesRead = 0;
                        DateTime startTime = DateTime.Now;

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            // 更新ListViewItem的下载进度和速度
                            double progress = (double)totalBytesRead / (endRange - startRange + 1) * 100;
                            item.SubItems[2].Text = $"{progress:F2}%"; // 更新下载进度子项

                            TimeSpan elapsedTime = DateTime.Now - startTime;
                            double speed = totalBytesRead / 1024.0 / elapsedTime.TotalSeconds; // 下载速度，单位 KB/s
                            item.SubItems[3].Text = $"{speed:F2} KB/s"; // 更新下载速度子项

                            double instantSpeed = bytesRead / 1024.0; // 瞬时速度，单位 KB/s
                            item.SubItems[4].Text = $"{instantSpeed:F2} KB/s"; // 更新瞬时速度子项


                            // 异步更新UI
                            await Task.Run(() =>
                            {
                                listView1.BeginInvoke(new MethodInvoker(() => { listView1.Refresh(); }));
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="sourceUrl">源 URL</param>
        /// <returns>文件大小</returns>
        private async Task<long> GetFileSizeAsync(string sourceUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, sourceUrl), HttpCompletionOption.ResponseHeadersRead);
                return response.Content.Headers.ContentLength ?? 0;
            }
        }

        /// <summary>
        /// 合并区块为一个文件，然后《 卸 磨 杀 驴 》（指删除区块文件避免额外占用）
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="numParts">区块编号</param>
        private void CombineFile(string savePath, int numParts)
        {
            using (FileStream combinedFile = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < numParts; i++)
                {
                    string partFilePath = $"{savePath}.part{i}";
                    byte[] partBytes = File.ReadAllBytes(partFilePath);
                    combinedFile.Write(partBytes, 0, partBytes.Length);
                    File.Delete(partFilePath); // 删除临时部分文件
                }
            }
        }

        /// <summary>
        /// 速度单位换算
        /// </summary>
        /// <param name="speed">源速度（字节）</param>
        /// <returns>换算后速度</returns>
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

        /// <summary>
        /// 生成指定长度的随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>字符串</returns>
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// 单区块下载的更新进度与状态逻辑
        /// </summary>
        /// <param name="sourceUrl">源 URL</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="response">返回信息</param>
        /// <param name="listItem">ListView 项</param>
        /// <returns>实时进度与状态</returns>
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

        #region 菜单项按钮方法 B

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

        private void 单区块模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!单区块模式ToolStripMenuItem.Checked)
            {
                单区块模式ToolStripMenuItem.Checked = true;
                多区块模式ToolStripMenuItem.Checked = false;
                MessageBox.Show("已更改下载模式。新的下载模式将在新的下载开始时生效。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("未切换下载模式。因为你已经处于这个模式下", "配置未被更改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void 多区块模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!多区块模式ToolStripMenuItem.Checked)
            {
                多区块模式ToolStripMenuItem.Checked = true;
                单区块模式ToolStripMenuItem.Checked = false;
                MessageBox.Show("已更改下载模式。新的下载模式将在新的下载开始时生效。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("未切换下载模式。因为你已经处于这个模式下", "配置未被更改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

    }
}
