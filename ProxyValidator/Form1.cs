using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace ProxyValidator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        string[] proxys;
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(TestProxy));
            thread1.Start();
        }

        private void TestProxy()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = false;
            richTextBox1.Clear();
            label1.Text = "正在验证代理......";
            List<Task> TaskList = new List<Task>();
            for (int i=0;i<proxys.Length;i++)
            {
                int l = i;
                TaskList.Add(Task.Factory.StartNew(() =>
                {
                    bool result = TestProxy(proxys[l]);
                    if(result)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            richTextBox1.Text += proxys[l] + " 可用\r";
                            proxys[l] = "";
                        }));
                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            richTextBox1.Text += proxys[l] + " 不可用\r";
                            proxys[l] = "";
                        }));
                    }
                }));
            }
            Task.WaitAll(TaskList.ToArray());
            MessageBox.Show("验证完毕！", "提示");
            button2.Enabled = true;
            button4.Enabled = true;
            textBox1.Enabled = true;
        }
        private bool TestProxy(string proxy)
        {
            try
            {
                WebProxy proxyObject = new WebProxy(proxy);
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(textBox1.Text);
                Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                Req.Proxy = proxyObject;
                Req.Method = "GET";
                Req.Timeout = 1000;
                HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread3 = new Thread(new ThreadStart(IncludeProxy));
            thread3.ApartmentState = ApartmentState.STA;
            thread3.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(new ThreadStart(TestURL));
            thread2.Start();
        }
        private void TestURL()
        {
            button2.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = false;
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(textBox1.Text);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Timeout = 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                MessageBox.Show("验证网址可用！", "提示");
                button2.Enabled = true;
                textBox1.Enabled = true;
            }
            catch
            {
                MessageBox.Show("验证网址不可用！", "提示");
                button2.Enabled = true;
                button4.Enabled = true;
                textBox1.Enabled = true;
            }
        }
        private void IncludeProxy()
        {
            label1.Text = "正在导入代理......";
            openFileDialog1.Filter = "文本文档|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                proxys = File.ReadAllLines(openFileDialog1.FileName);
                int i = 0;
                richTextBox1.Clear();
                while (i<proxys.Length)
                {
                    richTextBox1.Text += proxys[i] + "\r";
                    i++;
                }
                button1.Enabled = true;
                label1.Text = "代理已导入！";
            }
            else
            {
                label1.Text = "代理导入已取消！";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread4 = new Thread(new ThreadStart(SaveResult));
            thread4.ApartmentState = ApartmentState.STA;
            thread4.Start();
        }
        private void SaveResult()
        {
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "文本文档|*.txt";
            if(a.ShowDialog()==DialogResult.OK)
            {
                string b = a.FileName;
                File.WriteAllLines(b, proxys);
            }
        }
    }
}
