using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace OpenDownload.NET
{
    public partial class New_Task : Form
    {
        public event EventHandler<DownloadEventArgs> DownloadStarted;

        public New_Task()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                        long requiredSpace = GetRemoteFileSize(downloadFilePath);
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
                        Spacelabel.Text = "发生异常：" + ex.Message;
                        启动下载按钮.Enabled = false;
                    }
                }
            }
        }

        private long GetRemoteFileSize(string url)
        {
            long fileSize = 0;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "HEAD";

                using (WebResponse response = request.GetResponse())
                {
                    if (response is HttpWebResponse httpWebResponse)
                    {
                        fileSize = httpWebResponse.ContentLength;
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"网络异常：{ex.Message}");
            }

            return fileSize;
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
            this.Close();
        }

        protected virtual void OnDownloadStarted(string sourceUrl, string savePath)
        {
            DownloadStarted?.Invoke(this, new DownloadEventArgs(sourceUrl, savePath));
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