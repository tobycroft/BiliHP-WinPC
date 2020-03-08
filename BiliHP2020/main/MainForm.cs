using BiliHP2020.func;
using BiliHP2020.tuuz;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Windows.Forms;

namespace BiliHP2020
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress address = Dns.GetHostEntry("go.bilihp.com").AddressList[0];
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.socket.NoDelay = false;
            ecam_action(this.socket.ProtocolType.ToString());
            ecam_action(this.socket.SocketType.ToString());
            this.socket.Connect(this.address.ToString(), 81);
            Thread sock = new Thread(recieve);
            sock.IsBackground = true;
            sock.Start();
            JObject aa = new JObject();
            aa["username"] = Properties.Settings.Default.username;
            aa["token"] = Properties.Settings.Default.token;
            aa["type"] = "win";
            aa["version"] = "0.0.1";
            this.send("init", aa, "init");
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

        private JObject check_time()
        {
            try
            {
                JObject job = JObject.Parse(Properties.Settings.Default.time);
                return job;
            }
            catch
            {
                Properties.Settings.Default.time = "{}";
                Properties.Settings.Default.Save();
                return this.check_time();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.send("init", null, "init");
        }

        private void send(string type, JObject data, string echo)
        {
            this.socket.Send(Encoding.UTF8.GetBytes(RET.ws_succ(type, 0, data, echo)));
        }
        public void recieve()
        {
            byte[] buffer = new byte[4096];
            string temp = "";
            while (true)
            {
                int length = this.socket.Receive(buffer);
                if (length == 0)
                {
                    this.socket.Close();
                    return;
                }
                else
                {
                    string data = System.Text.Encoding.UTF8.GetString(buffer, 0, length);
                    temp += data;
                    JObject tp = TCPObject.tcpobj(temp);
                    JArray arr = tp["arr"].ToObject<JArray>();
                    temp = tp["json"].ToString();
                    foreach (var item in arr)
                    {
                        ActionRoute.Route(item.ToObject<JObject>(), Properties.Settings.Default.username, this.socket);
                        richTextBox1.Text = data.ToString();
                        ecam_action(data.ToString());
                    }
                    
                }

            }

        }
        public void ecam_action(object str)
        {
            var date = DateTime.Now.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(date);
            sb.Append(":");
            sb.Append(str.ToString());
            ecam.Items.Insert(0, sb.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}
