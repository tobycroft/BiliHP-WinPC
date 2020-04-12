using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.Threading;

namespace BiliHP2020.func
{
    class ActionRoute
    {
        public RichTextBox rtb;
        public ListBox ecam;
        public string username;
        public JObject json;

        public void Route()
        {
            int code = json["code"].ToObject<int>();
            string type = json["type"].ToString();
            dynamic ret = json["data"];
            string echo = json["echo"].ToString();
            if (code == -1)
            {
                ecam_action("[登录信息]：" + "登录信息错误！" + ret);
                return;
            }

            switch (type)
            {
                case "orign":
                    ecam2(echo, ret);
                    break;

                case "app":
                    PCRoute pcr = new PCRoute();
                    pcr.json = json;
                    pcr.username = username;
                    pcr.ecam = ecam;
                    pcr.rtb = rtb;
                    //pcr.MainForm.socket = MainForm.socket;
                    Thread th = new Thread(pcr.Route);
                    th.IsBackground = true;
                    th.Start();
                    break;


                case "supercurl":
                    ecam2(echo, ret);
                    break;

                case "info":
                    ecam2(echo, ret);
                    break;

                case "warning":
                    ecam2(echo, ret);
                    break;

                case "error":
                    ecam2(echo, ret);
                    break;

                case "update":
                    ecam2(echo, ret);
                    Net.DownLoad(ret);
                    break;

                case "c2c":
                    break;

                case "force_update":
                    //todo:这里要加入自动下载的方法
                    ecam_action(echo);
                    Net.DownLoad(ret);
                    Environment.Exit(0);
                    break;

                case "close":
                    Environment.Exit(0);
                    break;

                case "reinit":
                    //todo:这里加入重新验证的方法
                    JObject aa = new JObject();
                    aa["username"] = Properties.Settings.Default.username;
                    aa["token"] = Properties.Settings.Default.token;
                    aa["type"] = "win";
                    aa["version"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    send(send_obj("init", "{}", "init"));
                    ecam2("", ret);
                    break;

                case "debug":
                    //todo:这里加入debug开关方法
                    if (Properties.Settings.Default.debug)
                    {
                        ecam2("[BiliHP-Debug]:", ret);
                    }
                    break;

                case "other":
                    ecam2("[BiliHP-Other]:", ret);
                    break;

                case "ecam":
                    ecam2("[BiliHP-ECAM]:", ret);
                    break;

                case "alert":
                    ecam2("[BiliHP-Alert]:", ret);
                    break;

                case "login":
                    ecam2("[BiliHP-Login]:", ret);
                    break;

                case "loged":
                    send(RET.ws_succ2("app", "get_config", new JObject(), "get_config"));
                    send(RET.ws_succ2("func", "", new JObject(), "user_info"));
                    send(RET.ws_succ2("func", "", new JObject(), "refresh_token"));
                    ecam2("[BiliHP-Loged]:", ret);
                    break;

                case "clear":
                    ecam.Items.Clear();
                    break;

                case "notam":
                    ecam2("[BiliHP-NOTAM]:", ret);
                    break;

                case "system":
                    ecam2("[BiliHP-系统消息]:", ret);
                    break;

                case "pong":
                    //todo:这里加入debug开关方法
                    ecam2("[BiliHP-Ping]:", ret);
                    break;


                case "curl":
                    ecam_action(echo);
                    JObject rets = ret;
                    JObject header = rets["header"].ToObject<JObject>();
                    JObject values = rets["values"].ToObject<JObject>();
                    JObject cookie = rets["cookie"].ToObject<JObject>();

                    string url = rets["url"].ToString();
                    string method = rets["method"].ToString();
                    string route = rets["route"].ToString();
                    string typ = rets["type"].ToString();
                    int delay = rets["delay"].ToObject<int>();

                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    break;

                case "join_room":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.join_room)
                    {
                        break;
                    }
                    rets = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    ecam2(type, echo);
                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    break;

                case "gift":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.raffle)
                    {
                        ecam2("[BiliHP-Net]", "电视类型的领奖被关闭，请在面板中开启");
                        break;
                    }
                    if (!Time_check())
                    {
                        ecam2("[BiliHP-Net]", "不在领取时间段中");
                        break;
                    }
                    if (!Gift_ratio())
                    {
                        ecam2("[BiliHP-Net]", "自动跳过本礼物，如需增加领取率，请提高概率设定");
                        break;
                    }
                    rets = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    ecam2(type, echo);
                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    break;

                case "guard":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.guard)
                    {
                        ecam2("[BiliHP-Net]", "舰长类型的领奖被关闭，请在面板中开启");
                        break;
                    }
                    if (!Time_check())
                    {
                        ecam2("[BiliHP-Net]", "不在领取时间段中");
                        break;
                    }
                    if (!Gift_ratio())
                    {
                        ecam2("[BiliHP-Net]", "自动跳过本礼物，如需增加领取率，请提高概率设定");
                        break;
                    }
                    rets = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    ecam2(type, echo);
                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    break;

                case "tianxuan":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.tianxuan)
                    {
                        ecam2("[BiliHP-Net]", "天选类型的领奖被关闭，请在面板中开启");
                        break;
                    }
                    if (!Time_check())
                    {
                        ecam2("[BiliHP-Net]", "不在领取时间段中");
                        break;
                    }
                    if (!Gift_ratio())
                    {
                        ecam2("[BiliHP-Net]", "自动跳过本礼物，如需增加领取率，请提高概率设定");
                        break;
                    }
                    JObject obj = json["object"].ToObject<JObject>();

                    string bw = Properties.Settings.Default.ban_words;
                    string[] bws = bw.Split(',');
                    foreach (var item in bws)
                    {
                        if (item.Length>0)
                        {
                            if (obj["award_name"].ToString().Contains(item))
                            {
                                ecam2("[BiliHP-Net]", obj["award_name"].ToString() + "本礼物在屏蔽词(" + item + ")里，自动跳过");
                                return;
                            }
                        }
                    }
                    rets = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    ecam2(type, echo);
                    //SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay);
                    break;

                case "pk":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.pk)
                    {
                        ecam2("[BiliHP-Net]", "PK类型的领奖被关闭，请在面板中开启");
                        break;
                    }
                    if (!Time_check())
                    {
                        ecam2("[BiliHP-Net]", "不在领取时间段中");
                        break;
                    }
                    if (!Gift_ratio())
                    {
                        ecam2("[BiliHP-Net]", "自动跳过本礼物，如需增加领取率，请提高概率设定");
                        break;
                    }
                    rets = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    ecam2(type, echo);
                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    break;

                case "storm":
                    //ecam_action(echo);
                    if (!Properties.Settings.Default.storm)
                    {
                        ecam2("[BiliHP-Net]", "风暴类型的领奖被关闭，请在面板中开启");
                        break;
                    }
                    if (!Time_check())
                    {
                        ecam2("[BiliHP-Net]", "不在领取时间段中");
                        break;
                    }
                    if (!Gift_ratio())
                    {
                        ecam2("[BiliHP-Net]", "自动跳过本礼物，如需增加领取率，请提高概率设定");
                        break;
                    }
                    rets = ret;
                    stm_ret = ret;
                    header = rets["header"].ToObject<JObject>();
                    values = rets["values"].ToObject<JObject>();
                    cookie = rets["cookie"].ToObject<JObject>();

                    url = rets["url"].ToString();
                    method = rets["method"].ToString();
                    route = rets["route"].ToString();
                    typ = rets["type"].ToString();
                    delay = rets["delay"].ToObject<int>();
                    stm_echo = echo;
                    ecam2(type, echo);
                    if (Properties.Settings.Default.strom_catch)
                    {
                        Thread stm = new Thread(self_storm);
                        stm.IsBackground = true;
                        stm.Start();
                    }
                    else
                    {
                        SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, echo, route, delay, ecam);
                    }
                    break;

                default:
                    ecam2("unknow-ecam:", ret);
                    break;
            }
        }

        private dynamic stm_ret;
        private string stm_echo;

        private void self_storm()
        {
            JObject rets = stm_ret;
            JObject header = rets["header"].ToObject<JObject>();
            JObject values = rets["values"].ToObject<JObject>();
            JObject cookie = rets["cookie"].ToObject<JObject>();

            string url = rets["url"].ToString();
            string method = rets["method"].ToString();
            string route = rets["route"].ToString();
            string typ = rets["type"].ToString();
            int delay = rets["delay"].ToObject<int>();
            for (int i = 0; i < Properties.Settings.Default.storm_time; i++)
            {
                for (int s = 0; s < Properties.Settings.Default.storm_count; s++)
                {
                    SuperCurl.Curl(MainForm.socket, url, method, values, header, cookie, typ, stm_echo, route, delay, ecam);
                    Thread.Sleep(1000 / Properties.Settings.Default.storm_count);
                }
                Thread.Sleep(1000 - 1000 / Properties.Settings.Default.storm_count);
            }
        }

        public static JObject get_time()
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
                return get_time();
            }
        }

        private static bool Time_check()
        {
            JObject time = get_time();
            string hour = DateTime.Now.Hour.ToString();
            if (time.Property("t" + hour) != null)
            {
                if (time["t" + hour].ToObject<bool>())
                {
                    return true;
                }
            }
            return false;
        }

        public bool Gift_ratio()
        {
            Random rand = new Random();
            int rd = rand.Next(0, 100);
            if (rd < Properties.Settings.Default.percent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ecam_action(object str)
        {
            var date = DateTime.Now.ToLongTimeString().ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(date);
            sb.Append(":");
            sb.Append(str.ToString());
            ecam.Items.Insert(0, sb.ToString());
        }

        public void ecam2(object msg, dynamic ret)
        {
            rtb.Text = msg.ToString() + (string)ret;
            ecam_action(msg + (string)ret);
            send(send_obj("send_app", msg.ToString(), "", ret));
        }

        private string send_obj(string type, string data, string echo, dynamic values = null)
        {
            JObject obj = new JObject();
            obj["type"] = type;
            obj["data"] = data;
            obj["echo"] = echo;
            obj["values"] = values;
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        private void send(string data)
        {
            try
            {
                MainForm.socket.Send(Encoding.UTF8.GetBytes(data));

            }
            catch (Exception e)
            {
                ecam_action("ActionRoute:" + e.Message);
            }
        }
    }
}
