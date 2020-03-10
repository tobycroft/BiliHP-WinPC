﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BiliHP2020.func
{
    class Function
    {
        public Socket socket;
        public void yingyuan_sign()
        {
            while (true) 
            {
                if (Properties.Settings.Default.yingyuan_sign)
                {
                    string obj = send_obj("func", new JObject(), "yingyuan_sign");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

        public void daily_task()
        {
            while (true)
            {
                if (Properties.Settings.Default.daily_task)
                {
                    string obj = send_obj("func", new JObject(), "daily_task");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

        public void silver_task()
        {
            while (true)
            {
                if (Properties.Settings.Default.silver_task)
                {
                    string obj = send_obj("func", new JObject(), "silver_task");
                    send(obj);
                }
                Thread.Sleep(60 * 10 * 1000);
            }
        }

        public void online_silver()
        {
            while (true)
            {
                if (Properties.Settings.Default.online_silver)
                {
                    string obj = send_obj("func", new JObject(), "online_silver");
                    send(obj);
                }
                Thread.Sleep(60 * 10 * 1000);
            }
        }

        public void daily_bag()
        {
            while (true)
            {
                if (Properties.Settings.Default.daily_bag)
                {
                    string obj = send_obj("func", new JObject(), "daily_bag");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

        public void app_heart()
        {
            while (true)
            {
                if (Properties.Settings.Default.app_heart)
                {
                    string obj = send_obj("func", new JObject(), "app_heart");
                    send(obj);
                }
                Thread.Sleep(59 * 1000);
            }
        }

        public void pc_heart()
        {
            while (true)
            {
                if (Properties.Settings.Default.pc_heart)
                {
                    string obj = send_obj("func", new JObject(), "pc_heart");
                    send(obj);
                }
                Thread.Sleep(57 * 1000);
            }
        }

        public void Ping()
        {
            while (true)
            {
                if (Properties.Settings.Default.pc_heart)
                {
                    string obj = send_obj("ping", "ping", "ping");
                    send(obj);
                }
                Thread.Sleep(57 * 1000);
            }
        }


        private string send_obj(string type, dynamic data, string echo, dynamic values = null)
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