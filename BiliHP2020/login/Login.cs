using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020.login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.live.bilibili.com/client/v1/Ip/getInfoNew");
            req.Method = "POST";
            
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            JObject header = new JObject();
            foreach (string item in resp.Headers)
            {
                header.Add(item, resp.Headers[item]);
            }
            richTextBox1.Text += header;
            
           

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
    }
}
