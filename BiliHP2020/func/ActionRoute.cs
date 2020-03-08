using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020.func
{
    class ActionRoute
    {
        public RichTextBox rtb;
        public ListBox ecam;
        public string username;
        public Socket socket;
        public JObject json;
        public static void Route(JObject json, string username, Socket socket)
        {

        }

        public void Route()
        {
            int code = json["code"].ToObject<int>();
            string type = json["type"].ToString();
            object ret = json["data"];
            string echo = json["echo"].ToString();
            if (code == -1)
            {
                ecam_action("[登录信息]：" + "登录信息错误！" + "");
                return;
            }

            switch (type)
            {
                case "orign":
                    ecam_action(ret);
                    break;

                case "app":
                    PCRoute pcr = new PCRoute();
                    pcr.json = json;
                    pcr.username = this.username;
                    pcr.ecam = this.ecam;
                    pcr.rtb = this.rtb;
                    pcr.Route();
                    break;


                case "supercurl":
                    ecam2("", ret);
                    break;

                case "info":
                    ecam2("", ret);
                    break;

                case "warning":
                    ecam2("", ret);
                    break;

                case "error":
                    ecam2("", ret);
                    break;

                case "update":
                    ecam2("", ret);
                    break;

                case "c2c":
                    break;

                case "force_update":
                    ecam_action(echo);

                    break;

                case "reinit":
                    ecam2("", ret);
                    break;







                default:
                    break;
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

        public void ecam2(object msg)
        {
            ecam_action(msg);
            this.send(this.send_obj("send_app", msg.ToString(), ""));
        }

        private string send_obj(string type, string data, string echo, JToken values = null)
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
            this.socket.Send(Encoding.UTF8.GetBytes(data));
        }
    }
}
