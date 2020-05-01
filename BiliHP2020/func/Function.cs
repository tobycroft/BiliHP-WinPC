using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BiliHP2020.func
{
    class Function
    {
        //public Socket socket;
        public ListBox ecam;
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
            try
            {
                MainForm.socket.Send(Encoding.UTF8.GetBytes(data));
            }
            catch (Exception e)
            {
                MainForm.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MainForm.socket.NoDelay = false;
                MainForm.socket.Connect(MainForm.address.ToString(), 181);
                ecam_action("FUNCTION:" + e.Message);
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

        public void do_sign()
        {
            while (true)
            {
                if (Properties.Settings.Default.do_sign)
                {
                    string obj = send_obj("func", new JObject(), "do_sign");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

        public void manga_sign()
        {
            while (true)
            {
                if (Properties.Settings.Default.manga_sign)
                {
                    string obj = send_obj("func", new JObject(), "manga_sign");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

        public void manga_share()
        {
            while (true)
            {
                if (Properties.Settings.Default.manga_share)
                {
                    string obj = send_obj("func", new JObject(), "manga_share");
                    send(obj);
                }
                Thread.Sleep(86400 * 1000);
            }
        }

    }
}
