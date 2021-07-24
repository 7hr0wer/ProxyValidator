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
        }
        string[] proxy;
        int a;
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(Thread1));
            thread1.Start();
        }

        private void Thread1()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = false;
            richTextBox1.Clear();
            label1.Text = "正在验证代理......";
            for (int i=0;i<proxy.Length;i++)
            {
                a = i;
                Thread thread2 = new Thread(new ThreadStart(Thread2));
                thread2.Start();
                System.Threading.Thread.Sleep(100);
            }
            if(richTextBox1.Text=="")
            {
                label1.Text = "验证完毕！无可用代理！";
                MessageBox.Show("验证完毕！无可用代理！", "提示");
                button1.Enabled = true;
                button2.Enabled = true;
                button4.Enabled = true;
                textBox1.Enabled = true;
            }
            else
            {
                label1.Text = "验证完毕！";
                MessageBox.Show("验证完毕！", "提示");
                button1.Enabled = true;
                button2.Enabled = true;
                button4.Enabled = true;
                textBox1.Enabled = true;
            }
        }
        private void Thread2()
        {
            try
            {
                WebProxy proxyObject = new WebProxy(proxy[a]);
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(textBox1.Text);
                Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";//UA
                Req.Proxy = proxyObject;
                Req.Method = "GET";
                Req.Timeout = 1000;
                HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                richTextBox1.Text += proxy[a] + " 可用\n";
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread4 = new System.Threading.Thread(new System.Threading.ThreadStart(Thread4));
            thread4.ApartmentState = ApartmentState.STA;
            thread4.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread3 = new Thread(new ThreadStart(Thread3));
            thread3.Start();
        }
        private void Thread3()
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
        private void Thread4()
        {
            label1.Text = "正在导入代理......";
            openFileDialog1.Filter = "txt文件|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                proxy = File.ReadAllLines(openFileDialog1.FileName);
                int i = 0;
                richTextBox1.Clear();
                while (i<proxy.Length)
                {
                    richTextBox1.Text += proxy[i] + "\r";
                    i++;
                }
                button1.Enabled = true;
                label1.Text = "代理已导入！";
            }
            else
            {
                label1.Text = "代理已导入！";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread5 = new System.Threading.Thread(new System.Threading.ThreadStart(Thread5));
            thread5.ApartmentState = ApartmentState.STA;
            thread5.Start();
        }
        private void Thread5()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            saveFileDialog.FileName = "result.txt";
            bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, true);
                streamWriter.Write(this.richTextBox1.Text);
                streamWriter.Close();
            }
        }
    }
}
