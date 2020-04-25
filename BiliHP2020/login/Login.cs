using BiliHP2020.func;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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
            if (!eula.Checked)
            {
                MessageBox.Show("如果不信任我们，就不要用了吧？");
                return;
            }
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.username) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.token))
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
            if (!eula.Checked)
            {
                MessageBox.Show("呵呵");
                return;
            }
            JObject value = new JObject();
            value["username"] = username.Text;
            value["password"] = password.Text;
            JObject ret = Net.Post("http://go.bilihp.com:180/v1/index/login/3", value, null, null);
            if (ret["body"]["code"].ToObject<int>() == 0)
            {
                JObject data = ret["body"]["data"].ToObject<JObject>();
                JObject cookie = data["cookie"].ToObject<JObject>();
                JObject header = data["header"].ToObject<JObject>();
                JObject values = data["values"].ToObject<JObject>();
                string url = data["url"].ToString();
                string method = data["method"].ToString();
                JObject ret2 = Net.Curl(url, method, values, header, cookie, null);

                JObject send = new JObject();
                send["username"] = username.Text;
                send["password"] = password.Text;
                send["ret"] = ret2.ToString(Newtonsoft.Json.Formatting.None);

                JObject ret3 = Net.Post("http://go.bilihp.com:180/v1/index/login/ret", send, null, null);
                if (ret3["body"]["code"].ToObject<int>() == 0)
                {
                    Properties.Settings.Default.username = ret3["body"]["data"]["username"].ToString();
                    Properties.Settings.Default.password = password.Text;
                    Properties.Settings.Default.token = ret3["body"]["data"]["token"].ToString();
                    Properties.Settings.Default.Save();
                    MessageBox.Show(ret3["body"]["data"]["message"].ToString());
                    mainframe();
                }
                else if (ret3["body"]["code"].ToObject<int>() == -105)
                {
                    JObject geetest = Net.Get("http://go.bilihp.com:180/v1/index/captcha/gee_captcha?" + ret3["body"]["data"].ToString().Split('?')[1], new JObject(), null, null, null);
                    string uuu = "http://app.bilihp.com:81/geetest.html?username=" +
                    username.Text + "&challenge=" +
                     geetest["body"]["challenge"].ToString() + "&gt=" +
                     geetest["body"]["gt"].ToString();

                    SetRegistery();
                    //webBrowser1.Navigate(uuu);
                    try
                    {
                        System.Diagnostics.Process.Start(uuu);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("浏览器故障，开始调用IE");
                        try
                        {
                            System.Diagnostics.Process.Start("iexplore.exe", uuu);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("请先安装浏览器否则无法验证");
                        }
                    }
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
            JObject job = new JObject();
            job.Add("aa", 123);
            job.ToString();
            version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            username.Text = Properties.Settings.Default.username;
            password.Text = Properties.Settings.Default.password;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = "";
            Properties.Settings.Default.password = "";
            Properties.Settings.Default.token = "";
            Properties.Settings.Default.Save();
        }

        private void mainframe()
        {

            MainForm ea = new MainForm();
            //ea.ecam.Items.Add("首页登录完成……");
            //ea.ShowDialog();
            ea.Show();
            this.Visible = false;
        }

        private void version_Click(object sender, EventArgs e)
        {
            Net.DownLoad("http://pandorabox.tuuz.cc:8000/app/BiliHP哔哩哔哩助手.exe");
        }

        private void button6_Click(object sender, EventArgs e)
        {

            JObject value = new JObject();
            value["username"] = username.Text;
            value["cid"] = cid.Text;
            JObject ret = Net.Post("http://go.bilihp.com:180/v1/index/login/bili_sms", value, null, null);
            if (ret["body"]["code"].ToObject<int>() == 0)
            {
                MessageBox.Show("短信已经发出，请勿多次点击");
                JObject data = ret["body"]["data"].ToObject<JObject>();
                JObject cookie = data["cookie"].ToObject<JObject>();
                JObject header = data["header"].ToObject<JObject>();
                JObject values = data["values"].ToObject<JObject>();
                string url = data["url"].ToString();
                string method = data["method"].ToString();
                JObject ret2 = Net.Curl(url, method, values, header, cookie, null);
                if (ret2["body"]["code"].ToObject<int>() == 0)
                {
                    captcha_key.Text = ret2["body"]["data"]["captcha_key"].ToString();
                    MessageBox.Show("短信验证码已经发送，请注意查收");
                }
                else
                {
                    MessageBox.Show("短信发送失败:" + ret2["body"]["message"]);
                }
            }
            else
            {
                MessageBox.Show(ret["body"]["data"].ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JObject value = new JObject();
            value["username"] = username.Text;
            value["cid"] = cid.Text;
            value["captcha"] = captcha_key.Text;
            value["code"] = code.Text;
            JObject ret = Net.Post("http://go.bilihp.com:180/v1/index/login/login_sms", value, null, null);
            if (ret["body"]["code"].ToObject<int>() == 0)
            {
                JObject data = ret["body"]["data"].ToObject<JObject>();
                JObject cookie = data["cookie"].ToObject<JObject>();
                JObject header = data["header"].ToObject<JObject>();
                JObject values = data["values"].ToObject<JObject>();
                string url = data["url"].ToString();
                string method = data["method"].ToString();
                JObject ret2 = Net.Curl(url, method, values, header, cookie, null);

                JObject send = new JObject();
                send["username"] = username.Text;
                send["password"] = password.Text;
                send["ret"] = ret2.ToString(Newtonsoft.Json.Formatting.None);

                JObject ret3 = Net.Post("http://go.bilihp.com:180/v1/index/login/ret", send, null, null);
                if (ret3["body"]["code"].ToObject<int>() == 0)
                {
                    Properties.Settings.Default.username = ret3["body"]["data"]["username"].ToString();
                    Properties.Settings.Default.password = password.Text;
                    Properties.Settings.Default.token = ret3["body"]["data"]["token"].ToString();
                    Properties.Settings.Default.Save();
                    MessageBox.Show(ret3["body"]["data"]["message"].ToString());
                    mainframe();
                }
                else if (ret3["body"]["code"].ToObject<int>() == -105)
                {
                    JObject geetest = Net.Get("http://go.bilihp.com:180/v1/index/captcha/gee_captcha?" + ret3["body"]["data"].ToString().Split('?')[1], new JObject(), null, null, null);
                    string uuu = "http://app.bilihp.com:81/geetest.html?username=" +
                    username.Text + "&challenge=" +
                     geetest["body"]["challenge"].ToString() + "&gt=" +
                     geetest["body"]["gt"].ToString();
                    try
                    {
                        System.Diagnostics.Process.Start(uuu);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("浏览器故障，开始调用IE");
                        try
                        {
                            System.Diagnostics.Process.Start("iexplore.exe", uuu);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("请先安装浏览器否则无法验证");
                        }
                    }
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

        private bool SetRegistery()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64))
                {
                    using (RegistryKey key = hklm.OpenSubKey(@"MIME\Database\Content Type\application/json", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("CLSID", "{25336920-03F9-11cf-8FD0-00AA00686F13}");
                            key.SetValue("Encoding", new byte[] { 0x80, 0x00, 0x00, 0x00 });
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void phone_TextChanged(object sender, EventArgs e)
        {
            username.Text = phone.Text;
        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            phone.Text = username.Text;
            user.Text = username.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!eula.Checked)
            {
                MessageBox.Show("如果不信任我们，就不要用了吧？");
                return;
            }
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.username) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.token))
            {
                mainframe();
                return;
            }

            if (user.Text.Length < 6)
            {
                MessageBox.Show("用户名太短啦");
                return;
            }
            if (pass.Text.Length < 6)
            {
                MessageBox.Show("密码不能小于6位");
                return;
            }
            if (!eula.Checked)
            {
                MessageBox.Show("呵呵");
                return;
            }
            JObject value = new JObject();
            value["username"] = user.Text;
            value["password"] = pass.Text;

            JObject ret3 = Net.Post("http://go.bilihp.com:180/v1/index/login/self_login", value, null, null);
            if (ret3["body"]["code"].ToObject<int>() == 0)
            {
                Properties.Settings.Default.username = ret3["body"]["data"]["username"].ToString();
                Properties.Settings.Default.password = pass.Text;
                Properties.Settings.Default.token = ret3["body"]["data"]["token"].ToString();
                Properties.Settings.Default.Save();
                MessageBox.Show(ret3["body"]["data"]["message"].ToString());
                mainframe();
            }
            else
            {
                MessageBox.Show(ret3["body"]["data"]["message"].ToString());
            }
            richTextBox1.Text = ret3.ToString();
        }

        private void user_TextChanged(object sender, EventArgs e)
        {
            username.Text = user.Text;
        }

        private void pass_TextChanged(object sender, EventArgs e)
        {
            password.Text = pass.Text;
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            pass.Text = password.Text;
        }
    }
}
