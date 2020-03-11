using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020.func
{
    class PCRoute
    {

        public RichTextBox rtb;
        public ListBox ecam;
        public string username;
        public Socket socket;
        public JObject json;
        public void Route()
        {
            string route = json["route"].ToString();
            JObject data = json["data"].ToObject<JObject>();
            string echo = json["echo"].ToString();
            switch (route)
            {

                case "update_config":
                    Properties.Settings.Default.app_heart = data["app_heart"].ToObject<bool>();
                    Properties.Settings.Default.pc_heart = data["pc_heart"].ToObject<bool>();
                    Properties.Settings.Default.daily_bag = data["daily_bag"].ToObject<bool>();
                    Properties.Settings.Default.online_silver = data["online_silver"].ToObject<bool>();
                    Properties.Settings.Default.silver_task = data["silver_task"].ToObject<bool>();
                    Properties.Settings.Default.daily_task = data["daily_task"].ToObject<bool>();
                    Properties.Settings.Default.silver_to_coin = data["silver_to_coin"].ToObject<bool>();
                    Properties.Settings.Default.yingyuan_sign = data["yingyuan_sign"].ToObject<bool>();
                    Properties.Settings.Default.raffle = data["raffle"].ToObject<bool>();
                    Properties.Settings.Default.guard = data["guard"].ToObject<bool>();
                    Properties.Settings.Default.tianxuan = data["tianxuan"].ToObject<bool>();
                    Properties.Settings.Default.pk = data["pk"].ToObject<bool>();
                    Properties.Settings.Default.storm = data["storm"].ToObject<bool>();
                    Properties.Settings.Default.time = data["time"].ToString();
                    Properties.Settings.Default.percent = data["pk"].ToObject<int>();
                    Properties.Settings.Default.update_time = data["pk"].ToObject<string>();
                    Properties.Settings.Default.date = data["date"].ToObject<string>();
                    Properties.Settings.Default.Save();

                    
                    ecam2("[ECAM-设置同步]", "收到设置同步消息");
                    break;

                

                default:

                    break;
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
            rtb.Text += msg.ToString() + (string)ret;
            ecam_action(msg + (string)ret);
            this.send(this.send_obj("send_app", msg.ToString(), "", ret));
        }

        private void send(string data)
        {
            this.socket.Send(Encoding.UTF8.GetBytes(data));
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
    }
}
