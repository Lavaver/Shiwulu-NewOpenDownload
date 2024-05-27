using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace OpenDownload.NET
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// ���ڹ�����������ظ�������� Dictionary ��������ֶ���������ʱ listView ������� Items ����������
        /// </summary>
        private Dictionary<Task, ListViewItem> downloadTaskMap = new Dictionary<Task, ListViewItem>();

        /// <summary>
        /// �û���������ͷ��User-Agent�����á�Ĭ��Ϊ Chrome �������Ĭ���û��������������������ͷ��Ϊ�˲��������׳� 403 ����״̬�뵼�³����׳��쳣�����ڰٶ�����Ҳ�Ѿ�������Ԥ�����ѡ��ɹ�ֱ��ʹ�á�
        /// </summary>
        public string UserAgentConfig = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

        /// <summary>
        /// ����һ�� NotifyIcon �ؼ�
        /// </summary>
        NotifyIcon notifyIcon = new NotifyIcon();

        /// <summary>
        /// ���������񴰿�
        /// </summary>
        private New_Task newTaskForm;

        public Form1()
        {
            InitializeComponent();
            Status.Text = "׼������";
            newTaskForm = new New_Task();
            newTaskForm.DownloadStarted += OnDownloadStarted;
            // ���� NotifyIcon �ؼ�������
            notifyIcon.Icon = new Icon("icon.ico");
            notifyIcon.Visible = true;
        }

        #region �˵��ť���� A

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

        #endregion

        private async void OnDownloadStarted(object sender, DownloadEventArgs e)
        {
            string sourceUrl = e.SourceUrl;
            string savePath = e.SavePath;

            if (������ģʽToolStripMenuItem.Checked) 
            {
                await DownloadFileAsync(sourceUrl, savePath);
            }
            else if (������ģʽToolStripMenuItem.Checked)
            {
                await MultiPartDownloadAsync(sourceUrl, savePath, 4);
            }
            
        }


        /// <summary>
        /// ����������ģʽ
        /// </summary>
        /// <param name="sourceUrl">��Դ URL</param>
        /// <param name="savePath">�����ļ���ַ</param>
        /// <returns></returns>
        private async Task DownloadFileAsync(string sourceUrl, string savePath)
        {
            ListViewItem newItem = null;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // �����Զ��� User-Agent
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgentConfig);

                    HttpResponseMessage response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead);

                    // ��ʾ������ʾ����
                    notifyIcon.ShowBalloonTip(1000, $"���� {savePath} �ѿ�ʼ����", "���ص��������Բ鿴��ϸ��Ϣ", ToolTipIcon.Info);

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

                    notifyIcon.ShowBalloonTip(1000, $"���� {savePath} ���������", "���ص��������Բ鿴��ϸ��Ϣ", ToolTipIcon.Info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�ļ�����ʧ�ܣ���־�ѱ����� /Exception/{time}.log ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    notifyIcon.ShowBalloonTip(1000, $"���� {savePath} ʧ��", "��־�ѱ����� /Exception/{time}.log �У���������Ҫʱʹ��", ToolTipIcon.Error);
                    CrashLogSpawn(ex);
                    if (newItem != null)
                    {
                        newItem.SubItems[1].Text = "����";
                        newItem.SubItems[3].Text = ""; // ��������ٶ�����
                    }
                }
            }
        }

        /// <summary>
        /// ����������ģʽ��Hotfix���޸�ʹ��ͬһ�� ListViewItem ��������⣩
        /// </summary>
        /// <param name="sourceUrl">��Դ URL</param>
        /// <param name="savePath">�����ַ</param>
        /// <param name="numParts">������</param>
        /// <returns>�����ļ�</returns>
        private async Task MultiPartDownloadAsync(string sourceUrl, string savePath, int numParts)
        {
            try
            {
                notifyIcon.ShowBalloonTip(1000, $"���������� {savePath} �ѿ�ʼ����", "���ص��������Բ鿴��ϸ��Ϣ", ToolTipIcon.Info);
                long fileSize = await GetFileSizeAsync(sourceUrl);
                long partSize = fileSize / numParts;

                List<Task> downloadTasks = new List<Task>();

                for (int i = 0; i < numParts; i++)
                {
                    long startRange = i * partSize;
                    long endRange = (i == numParts - 1) ? fileSize - 1 : (i + 1) * partSize - 1;
                    string partSavePath = $"{savePath}.part{i}";

                    ListViewItem newItem = new ListViewItem(sourceUrl);
                    newItem.SubItems.Add(savePath);
                    newItem.SubItems.Add("0%"); // ���ؽ�������
                    newItem.SubItems.Add("0 KB/s"); // �����ٶ�����
                    newItem.SubItems.Add("0 KB/s"); // ˲ʱ�ٶ�����
                    listView1.Items.Add(newItem);

                    Task downloadTask = DownloadPartAsync(sourceUrl, partSavePath, startRange, endRange, newItem);
                    downloadTasks.Add(downloadTask);
                    downloadTaskMap.Add(downloadTask, newItem); // ������Ͷ�Ӧ��ListViewItem��������
                }

                await Task.WhenAll(downloadTasks);

                // �����в����ļ��ϲ�Ϊ�����ļ�
                CombineFile(savePath, numParts);

                notifyIcon.ShowBalloonTip(1000, $"���������� {savePath} ���������", "���ص��������Բ鿴��ϸ��Ϣ", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�ļ�����ʧ�ܣ���־�ѱ����� /Exception/{time}.log ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                notifyIcon.ShowBalloonTip(1000, $"���������� {savePath} ʧ��", "��־�ѱ����� /Exception/{time}.log �У���������Ҫʱʹ��", ToolTipIcon.Error);
                CrashLogSpawn(ex);
            }
        }

        /// <summary>
        /// �������飨�����ʵʱ����������Ľ����ο�...���������޸���������̲߳��� UI �̵߳�������������⣨�� 257~260 �еĴ�����룺await Task.Run(() =>{ �����ע�ͣ� });���������в����豸��Ϊ����ԭ����Ӳ���Լ�����
        /// </summary>
        /// <param name="sourceUrl">Դ URL</param>
        /// <param name="savePath">�����ַ</param>
        /// <param name="startRange">���鿪ʼλ��</param>
        /// <param name="endRange">�������λ��</param>
        /// <param name="item">ListView ����</param>
        /// <returns>�����ļ�</returns>
        private async Task DownloadPartAsync(string sourceUrl, string savePath, long startRange, long endRange, ListViewItem item)
        {
            // ��¼���ؿ�ʼʱ��
            DateTime startTime = DateTime.Now;

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
                        DateTime lastUpdateTime = DateTime.Now;

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            // ������Ⱥ��ٶ�
                            double progress = (double)totalBytesRead / (endRange - startRange + 1) * 100;
                            TimeSpan elapsedTime = DateTime.Now - startTime;
                            double speed = totalBytesRead / 1024.0 / elapsedTime.TotalSeconds;
                            double instantSpeed = bytesRead / 1024.0;

                            if (DateTime.Now - lastUpdateTime > TimeSpan.FromSeconds(1)) // ���Ƹ���Ƶ��Ϊ1��
                            {
                                // ���� ListView ����
                                item.SubItems[2].Text = $"{progress:F2}%";
                                item.SubItems[3].Text = CalculateDownloadSpeed(speed);
                                item.SubItems[4].Text = CalculateDownloadSpeed(instantSpeed);

                                // ʹ�� Invoke ���� UI �߳��ϸ���UI
                                listView1.Invoke(new Action(() =>
                                {
                                    item.SubItems[2].Text = $"{progress:F2}%";
                                    item.SubItems[3].Text = CalculateDownloadSpeed(speed);
                                    item.SubItems[4].Text = CalculateDownloadSpeed(instantSpeed);
                                }));

                                lastUpdateTime = DateTime.Now; // ����������ʱ��
                            }
                        }

                        // ѭ��������һ���Ը������� ListView
                        listView1.Invoke(new Action(() =>
                        {
                            item.SubItems[2].Text = "�����";
                            item.SubItems[3].Text = ""; // ������Ҫ����ʵ������޸�
                            item.SubItems[4].Text = ""; // ������Ҫ����ʵ������޸�
                        }));
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�ļ���С
        /// </summary>
        /// <param name="sourceUrl">Դ URL</param>
        /// <returns>�ļ���С</returns>
        private async Task<long> GetFileSizeAsync(string sourceUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, sourceUrl), HttpCompletionOption.ResponseHeadersRead);
                return response.Content.Headers.ContentLength ?? 0;
            }
        }

        /// <summary>
        /// �ϲ�����Ϊһ���ļ���Ȼ�� ж ĥ ɱ ¿ ����ָɾ�������ļ��������ռ�ã�
        /// </summary>
        /// <param name="savePath">����·��</param>
        /// <param name="numParts">������</param>
        private void CombineFile(string savePath, int numParts)
        {
            using (FileStream combinedFile = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < numParts; i++)
                {
                    string partFilePath = $"{savePath}.part{i}";
                    byte[] partBytes = File.ReadAllBytes(partFilePath);
                    combinedFile.Write(partBytes, 0, partBytes.Length);
                    File.Delete(partFilePath); // ɾ����ʱ�����ļ�
                }
            }
        }

        /// <summary>
        /// �ٶȵ�λ����
        /// </summary>
        /// <param name="speed">Դ�ٶȣ��ֽڣ�</param>
        /// <returns>������ٶ�</returns>
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

        /// <summary>
        /// ����ָ�����ȵ�����ַ���
        /// </summary>
        /// <param name="length">����</param>
        /// <returns>�ַ���</returns>
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// ���������صĸ��½�����״̬�߼�
        /// </summary>
        /// <param name="sourceUrl">Դ URL</param>
        /// <param name="savePath">����·��</param>
        /// <param name="response">������Ϣ</param>
        /// <param name="listItem">ListView ��</param>
        /// <returns>ʵʱ������״̬</returns>
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
                    DateTime lastUpdateTime = DateTime.Now; // ���������ʱ�����
                    long previousDownloadedBytes = 0;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalDownloadedBytes += bytesRead;

                        if (DateTime.Now - lastUpdateTime > TimeSpan.FromSeconds(1)) // ���Ƹ���Ƶ��Ϊ1��
                        {
                            progress = (double)totalDownloadedBytes / totalBytes * 100;
                            listItem.SubItems[2].Text = $"{progress:F2}%";

                            TimeSpan elapsedTime = DateTime.Now - startTime;
                            double speed = totalDownloadedBytes / elapsedTime.TotalSeconds;
                            listItem.SubItems[3].Text = CalculateDownloadSpeed(speed);

                            long downloadedBytesSincePrevious = totalDownloadedBytes - previousDownloadedBytes;
                            double instantSpeed = downloadedBytesSincePrevious / 1024.0; // ��ʱ�ٶȣ���λΪ KB/s
                            listItem.SubItems[4].Text = CalculateDownloadSpeed(instantSpeed);

                            previousDownloadedBytes = totalDownloadedBytes;
                            lastUpdateTime = DateTime.Now; // ����������ʱ��
                        }
                    }

                    // ѭ��������һ���Ը������� ListView
                    listItem.SubItems[2].Text = "�����";
                    listItem.SubItems[3].Text = ""; 
                    listItem.SubItems[4].Text = ""; 
                }
            }
        }

        #region �˵��ť���� B

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

        private void ������ģʽToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!������ģʽToolStripMenuItem.Checked)
            {
                ������ģʽToolStripMenuItem.Checked = true;
                ������ģʽToolStripMenuItem.Checked = false;
                MessageBox.Show("�Ѹ�������ģʽ���µ�����ģʽ�����µ����ؿ�ʼʱ��Ч��", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("δ�л�����ģʽ����Ϊ���Ѿ��������ģʽ��", "����δ������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ������ģʽToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!������ģʽToolStripMenuItem.Checked)
            {
                ������ģʽToolStripMenuItem.Checked = true;
                ������ģʽToolStripMenuItem.Checked = false;
                MessageBox.Show("�Ѹ�������ģʽ���µ�����ģʽ�����µ����ؿ�ʼʱ��Ч��", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("δ�л�����ģʽ����Ϊ���Ѿ��������ģʽ��", "����δ������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

    }
}
