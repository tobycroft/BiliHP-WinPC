using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020.login
{
    public partial class Login : Form
    {
        public Login()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.live.bilibili.com/client/v1/Ip/getInfoNew");
            req.Method = "POST";
            CookieContainer cookies = new CookieContainer();
            Cookie cookie = new Cookie();
            cookie.Name = "LIVE_BUVID";
            cookie.Value = "AUTO7515833991642284";
            cookie.Domain = req.RequestUri.Host;

            cookies.Add(cookie);
            req.CookieContainer = cookies;

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            JObject header = new JObject();
            Dictionary<string, dynamic> headers = new Dictionary<string, dynamic>();

            foreach (string item in resp.Headers)
            {
                headers.Add(item, resp.Headers[item]);
            }

            header = JObject.FromObject(headers);

            richTextBox1.Text += header;

            byte[] buffer = new byte[int.Parse(resp.ContentLength.ToString())];
            Stream reStream = resp.GetResponseStream();
            string body = "";
            using (StreamReader sr = new StreamReader(reStream))
            {
                body = sr.ReadToEnd();
            }
            richTextBox1.Text += body;

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
