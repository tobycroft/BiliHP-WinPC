using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.update_user_info();

        }

        private void update_user_info()
        {
            WebClient wb = new WebClient();
            NameValueCollection nv = new NameValueCollection();
            nv.Add("username", Properties.Settings.Default.username);
            nv.Add("token", Properties.Settings.Default.token);
            byte[] ret = wb.UploadValues("http://go.bilihp.com:180/v1/user/user/user_info", nv);
            string data = Encoding.UTF8.GetString(ret);
            JObject job = JObject.Parse(data);
            if (job["code"].ToObject<int>() == -1)
            {
                Properties.Settings.Default.username = null;
                Properties.Settings.Default.password = null;
                Properties.Settings.Default.token = null;
                Properties.Settings.Default.Save();
                MessageBox.Show("你的登陆信息已经过期，请重新打开软件登陆");
                Environment.Exit(0);

            }
            else if (job["code"].ToObject<int>() == 0)
            {
                JObject dat = job["data"].ToObject<JObject>();
                face.ImageLocation = dat["face"].ToString();
                username.Text = dat["username"].ToString();
                uname.Text = dat["uname"].ToString();
                birthday.Text = dat["birthday"].ToString();
                coins.Text = dat["coins"].ToString();
                date.Text = dat["date"].ToString();
                user_level.Text = dat["user_level"].ToString();
                silver.Text = dat["silver"].ToString();

                guard_today.Text = dat["guard_today"].ToString();
                raffle_today.Text = dat["raffle_today"].ToString();
                pk_today.Text = dat["pk_today"].ToString();

                today_gifts.Text = dat["today_gifts"].ToString();
                today_guards.Text = dat["today_guards"].ToString();
                today_pks.Text = dat["today_pks"].ToString();
            }
            else
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = null;
            Properties.Settings.Default.password = null;
            Properties.Settings.Default.token = null;
            Properties.Settings.Default.Save();
            MessageBox.Show("已经退出");
            Environment.Exit(0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
