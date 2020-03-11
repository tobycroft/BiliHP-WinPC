﻿using BiliHP2020.func;
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
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.username)&& !string.IsNullOrWhiteSpace(Properties.Settings.Default.token))
            {
                mainframe();
                return;
            }

            if (username.Text.Length < 6)
            {
                MessageBox.Show("用户名太短啦");
                return;
            }
            if (password.Text.Length < 6)
            {
                MessageBox.Show("密码不能小于6位");
                return;
            }
            if (string.IsNullOrEmpty(captcha.Text))
            {
                MessageBox.Show("记得要输入验证码哦~");
                return;
            }
            if (!checkBox1.Checked)
            {
                MessageBox.Show("呵呵");
                return;
            }
            JObject value = new JObject();
            value["username"] = username.Text;
            value["password"] = password.Text;
            value["captcha"] = captcha.Text;
            JObject ret = Net.Post("http://go.bilihp.com:180/v1/index/login/2", value, null, null);
            if (ret["body"]["code"].ToObject<int>() == 0)
            {
                JObject data = ret["body"]["data"].ToObject<JObject>();
                JObject cookie = data["cookie"].ToObject<JObject>();
                JObject header = data["header"].ToObject<JObject>();
                JObject values = data["values"].ToObject<JObject>();
                string url = data["url"].ToString();
                string method = data["method"].ToString();
                JObject ret2 = Net.Curl(url, method, values, header, cookie);

                JObject send = new JObject();
                send["username"] = username.Text;
                send["password"] = password.Text;
                send["captcha"] = captcha.Text;
                send["ret"] = ret2.ToString(Newtonsoft.Json.Formatting.None);

                JObject ret3 = Net.Post("http://go.bilihp.com:180/v1/index/login/ret", send, null, null);
                if (ret3["body"]["code"].ToObject<int>() == 0)
                {
                    Properties.Settings.Default.username = ret3["body"]["data"]["username"].ToString();
                    Properties.Settings.Default.password = password.Text;
                    Properties.Settings.Default.token = ret3["body"]["data"]["token"].ToString();
                    Properties.Settings.Default.Save();
                    MessageBox.Show(ret3["body"]["data"]["message"].ToString());
                    this.Visible = false;
                    MainForm ea = new MainForm();
                    ea.ecam.Items.Add("首页登录完成……");
                    ea.ShowDialog();
                    Application.ExitThread();
                }
                else
                {
                    MessageBox.Show(ret3["body"]["data"].ToString());
                }
                richTextBox1.Text = ret3.ToString();
            }
            else
            {
                MessageBox.Show(ret["body"]["data"].ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("还没做好欸……");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            eula ea = new eula();
            ea.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            username.Text = Properties.Settings.Default.username;
            password.Text = Properties.Settings.Default.password;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            WebClient wb = new WebClient();
            //string ret = wb.DownloadString("http://go.bilihp.com:180/v1/index/login/bili_captcha?username=" + textBox1.Text);
            pictureBox2.ImageLocation = "http://go.bilihp.com:180/v1/index/login/bili_captcha?username=" + username.Text;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            mainframe();
        }

        private void mainframe()
        {
            this.Visible = false;
            MainForm ea = new MainForm();
            ea.ecam.Items.Add("首页登录完成……");
            ea.ShowDialog();
            Application.ExitThread();
        }

    }
}
