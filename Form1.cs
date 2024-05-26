using System.Net;
using System.Net.NetworkInformation;

namespace OpenDownload.NET
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// �û���������ͷ��User-Agent�����á�Ĭ��Ϊ Chrome �������Ĭ���û��������������������ͷ��Ϊ�˲��������׳� 403 ����״̬�뵼�³����׳��쳣�����ڰٶ����̺���ר�ż��о���ȥ���Ի������ͷ���뵽���Ԥ������У����ڰٶ����̸���������ͷ��ֻ��˵���ٶȣ����е㼫���ˡ�����
        /// </summary>
        public string UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

        private New_Task newTaskForm;
        public Form1()
        {
            InitializeComponent();
            Status.Text = "׼������";
            newTaskForm = new New_Task();
            newTaskForm.DownloadStarted += OnDownloadStarted;

        }

        private void mozilla50WindowsNT100Win64X64AppleWebKit53736KHTMLLikeGeckoChrome107000Safari53736ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Ĭ�ϴ�������ͷ���ð�ť.Checked)
            {
                UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
                MessageBox.Show("�ѻָ� User-Agent ���á����е����ý����µ����ؿ�ʼʱ��Ч��", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                �û��Զ��������������ͷ��ť.Text = "�û��Զ���";
                Ĭ�ϴ�������ͷ���ð�ť.Checked = true;
                �û��Զ��������������ͷ��ť.Checked = false;
                �ٶ�����ר�ô�������ͷ���ð�ť.Checked = false;
            }
            else
            {
                MessageBox.Show("User-Agent ����δ�����ġ���Ϊ�ѹ�ѡԤ������á�", "����δ������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void �û��Զ��������������ͷ��ť_Click(object sender, EventArgs e)
        {
            User_Agent_Config_FormPage ConfigPage = new();
            ConfigPage.MainForm = this;
            ConfigPage.ShowDialog();
        }

        public void UpdateUserAgent(string newAgent)
        {
            UserAgentConfig = newAgent;
            Ĭ�ϴ�������ͷ���ð�ť.Checked = false;
            �û��Զ��������������ͷ��ť.Checked = true;
            �ٶ�����ר�ô�������ͷ���ð�ť.Checked = false;
            �û��Զ��������������ͷ��ť.Text = $"���Զ�������Ϊ {newAgent} ��������޸�����";
        }

        private void �½�������ToolStripMenuItem_Click(object sender, EventArgs e)
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
                        throw new Exception($"����ʧ�ܣ�{response.StatusCode}");
                    }

                    newItem = new ListViewItem(sourceUrl);
                    newItem.SubItems.Add(savePath);
                    newItem.SubItems.Add("0%"); // ���ؽ�������
                    newItem.SubItems.Add("0 KB/s"); // �����ٶ�����
                    newItem.SubItems.Add("0 KB/s"); // ˲ʱ�ٶ�����
                    listView1.Items.Add(newItem);

                    await DownloadFileWithProgressAsync(sourceUrl, savePath, response, newItem);

                    newItem.SubItems[2].Text = "���";
                    newItem.SubItems[3].Text = ""; // ��������ٶ�����
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�ļ�����ʧ�ܣ���־�ѱ����� /Exception/{time}.log ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CrashLogSpawn(ex);
                    if (newItem != null)
                    {
                        newItem.SubItems[1].Text = "����";
                        newItem.SubItems[3].Text = ""; // ��������ٶ�����
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
        /// �����쳣���ɶ�ջ��־���߼�������·��Ϊ�����Ŀ¼�µ� /Exception/{time}.log �С������첽����Ϊ���ɹ��̲���Ҫռ�ù���ʱ�䣬��������һ˲���¼����ʱ����Դ�ģ����� JSON ��ʽ����Ϊ�ø�������Ա��Ҳ�ò�����죨�� VS Code ������Ϣ���ƺ���һ���� JSON �����ˣ�
        /// </summary>
        /// <param name="ex">�κ��쳣</param>
        static void CrashLogSpawn(Exception ex)
        {
            // ��¼�쳣����ʱ��
            string time = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            // ���ɴ�����ַ�������־�ļ�·��
            string randomString = GenerateRandomString(8);
            string logFileName = $"{time}_{randomString}_Exception.log";
            string logFilePath = $"./Exception/{logFileName}";

            try
            {
                // ����쳣Ŀ¼�����ڣ��򴴽�
                Directory.CreateDirectory("./Exception/");

                // ���쳣��Ϣд����־�ļ�
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine("Shiwulu Opendownload .NET �������ջ��־");
                    sw.WriteLine("---------------------------------");
                    sw.WriteLine($"�쳣����ʱ�䣺{time}");
                    sw.WriteLine($"�쳣��Ϣ��{ex.Message}");
                    sw.WriteLine($"�쳣��ջ��{ex.StackTrace}");
                    sw.WriteLine("---------------------------------");
                    sw.WriteLine("*** �����쳣����һλ���ж�ջ���ٵ�ĩβ ***");
                    sw.WriteLine(ex);
                    sw.WriteLine("---------------------------------");
                }
            }
            catch (Exception)
            {

            }
        }

        // ����ָ�����ȵ�����ַ���
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
            // ������Ӧ������½���
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

        private void �ٶ�����ר�ô�������ͷ���ð�ť_Click(object sender, EventArgs e)
        {
            if (!�ٶ�����ר�ô�������ͷ���ð�ť.Checked)
            {
                UserAgentConfig = "netdisk;P2SP;2.2.60.26";
                MessageBox.Show("�Ѹ��� User-Agent ���á����е����ý����µ����ؿ�ʼʱ��Ч��", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                �û��Զ��������������ͷ��ť.Text = "�û��Զ���";
                Ĭ�ϴ�������ͷ���ð�ť.Checked = false;
                �û��Զ��������������ͷ��ť.Checked = false;
                �ٶ�����ר�ô�������ͷ���ð�ť.Checked = true;
            }
            else
            {
                MessageBox.Show("User-Agent ����δ�����ġ���Ϊ�ѹ�ѡԤ������á�", "����δ������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
