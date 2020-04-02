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
        public static Socket socket;
        public static IPAddress address = Dns.GetHostEntry("go.bilihp.com").AddressList[0];
        //IPAddress address = Dns.GetHostEntry("127.0.0.1").AddressList[0];
        private void MainForm_Close(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            connect();
            ecam_action(socket.ProtocolType.ToString());
            ecam_action(socket.SocketType.ToString());
            Thread sock = new Thread(recieve);
            sock.IsBackground = true;
            sock.Start();
            init();
            //fn.socket = socket;
            fn.ecam = ecam;
            t1 = new Thread(fn.yingyuan_sign);
            t1.IsBackground = true;
            //t1.Start();

            t2 = new Thread(fn.daily_task);
            t2.IsBackground = true;
            t2.Start();

            t3 = new Thread(fn.silver_task);
            t3.IsBackground = true;
            t3.Start();

            t4 = new Thread(fn.online_silver);
            t4.IsBackground = true;
            t4.Start();

            t5 = new Thread(fn.daily_bag);
            t5.IsBackground = true;
            t5.Start();

            t6 = new Thread(fn.app_heart);
            t6.IsBackground = true;
            t6.Start();

            t7 = new Thread(fn.pc_heart);
            t7.IsBackground = true;
            t7.Start();

            t8 = new Thread(fn.Ping);
            t8.IsBackground = true;
            t8.Start();

            Thread sett = new Thread(setting_load);
            sett.IsBackground = true;
            sett.Start();
            update_user_info();
        }

        Function fn = new Function();
        Thread t1, t2;
        Thread t3;
        Thread t4;
        Thread t5;
        Thread t6;
        Thread t7;
        Thread t8;

        private void setting_load()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (Properties.Settings.Default.setting_read)
                {
                    Properties.Settings.Default.setting_lock = true;
                    Properties.Settings.Default.Save();
                    app_heart.Checked = Properties.Settings.Default.app_heart;
                    pc_heart.Checked = Properties.Settings.Default.pc_heart;
                    daily_bag.Checked = Properties.Settings.Default.daily_bag;
                    online_silver.Checked = Properties.Settings.Default.online_silver;
                    silver_task.Checked = Properties.Settings.Default.silver_task;
                    daily_task.Checked = Properties.Settings.Default.daily_task;
                    join_room.Checked = Properties.Settings.Default.join_room;
                    silver_to_coin.Checked = Properties.Settings.Default.silver_to_coin;
                    yingyuan_sign.Checked = Properties.Settings.Default.yingyuan_sign;
                    raffle.Checked = Properties.Settings.Default.raffle;
                    guard.Checked = Properties.Settings.Default.guard;
                    tianxuan.Checked = Properties.Settings.Default.tianxuan;
                    pk.Checked = Properties.Settings.Default.pk;
                    storm.Checked = Properties.Settings.Default.storm;
                    percent.Text = Properties.Settings.Default.percent.ToString();
                    storm_catch.Checked = Properties.Settings.Default.strom_catch;
                    storm_count.Text = Properties.Settings.Default.storm_count.ToString();
                    storm_time.Text = Properties.Settings.Default.storm_time.ToString();
                    ban_words.Text = Properties.Settings.Default.ban_words.ToString();

                    foreach (var item in ActionRoute.get_time())
                    {
                        switch (item.Key)
                        {

                            case "t0":
                                time0.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t1":
                                time1.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t2":
                                time2.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t3":
                                time3.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t4":
                                time4.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t5":
                                time5.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t6":
                                time6.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t7":
                                time7.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t8":
                                time8.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t9":
                                time9.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t10":
                                time10.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t11":
                                time11.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t12":
                                time12.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t13":
                                time13.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t14":
                                time14.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t15":
                                time15.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t16":
                                time16.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t17":
                                time17.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t18":
                                time18.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t19":
                                time19.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t20":
                                time20.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t21":
                                time21.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t22":
                                time22.Checked = item.Value.ToObject<bool>();
                                break;

                            case "t23":
                                time23.Checked = item.Value.ToObject<bool>();
                                break;

                            default:
                                break;
                        }
                    }

                    debug.Checked = Properties.Settings.Default.debug;
                    Properties.Settings.Default.setting_lock = false;
                    Properties.Settings.Default.setting_read = false;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.NoDelay = false;
                socket.Connect(address.ToString(), 181);
            }
            catch (Exception e)
            {
                ecam_action(e);
                Thread.Sleep(1000);
                connect();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "silver_to_coin");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "daily_bag");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "fengji");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "yingyuan_sign");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "user_info");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "refresh_token");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "user_info");
            update_user_info();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            send("func", new JObject(), "black_out");
        }

        private void update_user_info()
        {
            try
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
            catch (Exception e)
            {
                ecam_action(e);

                ecam_action("正在获取用户信息……请稍后刷新");
            }

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


        private void button9_Click(object sender, EventArgs e)
        {
            this.send("ping", null, "ping");
        }

        private void send(string type, JObject data, string echo)
        {
            send_raw(RET.ws_succ(type, 0, data, echo));
        }

        private void send_setting(string route, JObject data, string echo)
        {
            send_raw(RET.ws_succ2("app", route, data, echo));
        }

        private void send_raw(string raw)
        {
            socket.Send(Encoding.UTF8.GetBytes(raw));
        }

        private void init()
        {
            JObject aa = new JObject();
            aa["username"] = Properties.Settings.Default.username;
            aa["token"] = Properties.Settings.Default.token;
            aa["type"] = "win";
            aa["version"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            send("init", aa, "init");
        }
        public void recieve()
        {
            byte[] buffer = new byte[8192];
            string temp = "";
            try
            {
                while (true)
                {
                    int length = socket.Receive(buffer);

                    if (length == 0)
                    {
                        ecam_action("已经断开了……");
                        socket.Close();
                        return;
                    }
                    else
                    {
                        string data = System.Text.Encoding.UTF8.GetString(buffer, 0, length);
                        richTextBox2.Text = data.ToString();
                        temp += data;
                        JObject tp = TCPObject.tcpobj(temp);
                        JArray arr = tp["arr"].ToObject<JArray>();
                        temp = tp["json"].ToString();

                        foreach (var item in arr)
                        {
                            ActionRoute act = new ActionRoute();
                            act.rtb = richTextBox1;
                            //act.socket = socket;
                            act.ecam = ecam;
                            act.username = Properties.Settings.Default.username;
                            act.json = item.ToObject<JObject>();
                            Thread th = new Thread(act.Route);
                            th.IsBackground = true;
                            th.Start();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
                connect();
                init();
                ecam_action(e);
                recieve();
                throw e;
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

        private void app_heart_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.app_heart = app_heart.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "app_heart";
                setting["value"] = app_heart.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void online_silver_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.app_heart = app_heart.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "online_silver";
                setting["value"] = online_silver.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void silver_to_coin_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.app_heart = app_heart.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "silver_to_coin";
                setting["value"] = silver_to_coin.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void pc_heart_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.pc_heart = pc_heart.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "pc_heart";
                setting["value"] = pc_heart.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void yingyuan_sign_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.yingyuan_sign = yingyuan_sign.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "yingyuan_sign";
                setting["value"] = yingyuan_sign.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void daily_bag_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.daily_bag = daily_bag.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "daily_bag";
                setting["value"] = daily_bag.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void silver_task_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.silver_task = silver_task.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "silver_task";
                setting["value"] = silver_task.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void daily_task_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.daily_task = daily_task.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "daily_task";
                setting["value"] = daily_task.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void raffle_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.raffle = raffle.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "raffle";
                setting["value"] = raffle.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void guard_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.guard = guard.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "guard";
                setting["value"] = guard.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void tianxuan_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tianxuan = tianxuan.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "tianxuan";
                setting["value"] = tianxuan.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void pk_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.pk = pk.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "pk";
                setting["value"] = pk.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void storm_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.storm = storm.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "storm";
                setting["value"] = storm.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time0_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t0"] = time0.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time1_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t1"] = time1.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time2_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t2"] = time2.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time3_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t3"] = time3.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time4_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t4"] = time4.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time5_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t5"] = time5.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time6_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t6"] = time6.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time7_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t7"] = time7.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time8_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t8"] = time8.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time9_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t9"] = time9.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time10_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t10"] = time10.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time11_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t11"] = time11.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time12_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t12"] = time12.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time13_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t13"] = time13.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time14_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t14"] = time14.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time15_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t15"] = time15.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time16_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t16"] = time16.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time17_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t17"] = time17.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time18_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t18"] = time18.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time19_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t19"] = time19.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time20_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t20"] = time20.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time21_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t21"] = time21.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time22_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t22"] = time22.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void time23_CheckedChanged(object sender, EventArgs e)
        {
            JObject tm = ActionRoute.get_time();
            tm["t23"] = time23.Checked;
            Properties.Settings.Default.time = tm.ToString(Newtonsoft.Json.Formatting.None);
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "time";
                setting["value"] = ActionRoute.get_time().ToString(Newtonsoft.Json.Formatting.None);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void percent_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.percent = int.Parse(percent.Text);

            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "percent";
                setting["value"] = int.Parse(percent.Text);
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

            //隐藏任务栏区图标
            this.ShowInTaskbar = true;
            this.Visible = false;
            //图标显示在托盘区

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;

            this.Visible = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void join_room_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.join_room = join_room.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "join_room";
                setting["value"] = join_room.Checked;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void storm_catch_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.strom_catch = storm_catch.Checked;
            Properties.Settings.Default.Save();
        }

        private void storm_count_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.storm_count = int.Parse(storm_count.Text);
            Properties.Settings.Default.Save();
        }

        private void storm_time_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.storm_time = int.Parse(storm_time.Text);
            Properties.Settings.Default.Save();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Properties.Settings.Default.percent.ToString());

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ban_words = ban_words.Text;

            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.setting_lock)
            {
                JObject setting = new JObject();
                setting["key"] = "ban_words";
                setting["value"] = ban_words.Text;
                send_setting("pc_set_setting", setting, "pc_set_setting");
            }
        }

        private void debug_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.debug = debug.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
