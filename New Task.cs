using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace OpenDownload.NET
{
    public partial class New_Task : Form
    {
        public event EventHandler<DownloadEventArgs> DownloadStarted;

        public New_Task()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fullFilePath = dialog.FileName.Replace('\\', '/');
                    textBox2.Text = fullFilePath;

                    try
                    {
                        string downloadFilePath = textBox1.Text;
                        long requiredSpace = await GetRemoteFileSizeAsync(downloadFilePath);
                        string savePath = Path.GetDirectoryName(fullFilePath);
                        long availableSpace = GetAvailableSpace(savePath);

                        if (availableSpace >= requiredSpace)
                        {
                            Spacelabel.ForeColor = System.Drawing.Color.Green;
                            Spacelabel.Text = "满足该文件所需空间需求";
                            启动下载按钮.Enabled = true;
                        }
                        else
                        {
                            Spacelabel.ForeColor = System.Drawing.Color.Red;
                            Spacelabel.Text = "不满足该文件所需空间要求";
                            启动下载按钮.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Spacelabel.ForeColor = System.Drawing.Color.Red;
                        Spacelabel.Text = $"发生异常，已将异常日志存放至 ./Exception/ 中。";
                        CrashLogSpawn(ex);
                        启动下载按钮.Enabled = false;
                    }
                }
            }
        }

        private async Task<long> GetRemoteFileSizeAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"HTTP 返回不正常：{response.StatusCode}.");
                    }

                    if (!response.Content.Headers.ContentLength.HasValue || response.Content.Headers.ContentLength.Value <= 0)
                    {
                        throw new Exception("返回的文件大小不正常，可能是仍有部分服务器拒绝提供目标文件大小。");
                    }

                    return response.Content.Headers.ContentLength.Value;
                }
            }
            catch (Exception)
            {
                throw; // 将异常 throw 到更高层级
            }
        }

        private long GetAvailableSpace(string directoryPath)
        {
            DriveInfo drive = new DriveInfo(Path.GetPathRoot(directoryPath));
            return drive.AvailableFreeSpace;
        }

        private void 启动下载按钮_Click(object sender, EventArgs e)
        {

            string sourceUrl = textBox1.Text;
            string savePath = textBox2.Text;
            OnDownloadStarted(sourceUrl, savePath);
            textBox1.Clear();
            textBox2.Clear();
            Spacelabel.Text = "";
            this.Close();
        }

        protected virtual void OnDownloadStarted(string sourceUrl, string savePath)
        {
            DownloadStarted?.Invoke(this, new DownloadEventArgs(sourceUrl, savePath));
        }

        /// <summary>
        /// 出现异常生成堆栈日志的逻辑。保存路径为该软件目录下的 /Exception/{time}.log 中。不用异步是因为生成过程不需要占用过多时间，往往都是一瞬间记录好随时可溯源的
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
    }

    public class DownloadEventArgs : EventArgs
    {
        public string SourceUrl { get; }
        public string SavePath { get; }

        public DownloadEventArgs(string sourceUrl, string savePath)
        {
            SourceUrl = sourceUrl;
            SavePath = savePath;
        }
    }

    
}