﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
        public Socket socket;
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
                    ecam_action(ret);
                    break;

                case "app":
                    PCRoute pcr = new PCRoute();
                    pcr.json = json;
                    pcr.username = username;
                    pcr.ecam = ecam;
                    pcr.rtb = rtb;
                    pcr.socket = socket;
                    Thread th = new Thread(pcr.Route);
                    th.IsBackground = true;
                    th.Start();
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
                    //todo:这里要加入自动下载的方法
                    ecam_action(echo);

                    break;

                case "reinit":
                    //todo:这里加入重新验证的方法
                    ecam2("", ret);
                    break;

                case "debug":
                    //todo:这里加入debug开关方法
                    ecam2("[BiliHP-Debug]:", ret);
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
                    break;


                case "gift":
                    break;

                case "guard":
                    break;

                case "tianxuan":
                    break;

                case "pk":
                    break;

                case "storm":
                    break;

                default:
                    ecam2("unknow-ecam:",ret);
                    break;
            }
        }

        public bool Gift_check()
        {
            return true;
        }

        public bool Gift_ratio()
        {
            return true;

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

        public void ecam2(object msg, dynamic ret)
        {
            rtb.Text = msg.ToString()+(string)ret;
            ecam_action(msg+ (string)ret);
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
            socket.Send(Encoding.UTF8.GetBytes(data));
        }
    }
}
