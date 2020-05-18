using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BiliHP2020.func
{
    class SuperCurl
    {
        private string url;
        private string method;
        private JObject values;
        private JObject headers;
        private JObject cookie;
        private string type;
        private string echo;
        private string route;
        private int delay;
        //private Socket socket;
        private ListBox ecam;

        public static void Curl(Socket socket, string url, string method, JObject values, JObject headers, JObject cookie, string type, string echo, string route, int delay, ListBox ecam)
        {
            SuperCurl sp = new SuperCurl();
            sp.url = url;
            sp.method = method;
            sp.values = values;
            sp.headers = headers;
            sp.cookie = cookie;
            sp.type = type;
            sp.echo = echo;
            sp.route = route;
            sp.delay = delay;
            //sp.socket = socket;
            sp.ecam = ecam;
            Thread th = new Thread(sp.SuperCURL);
            th.IsBackground = true;
            th.Start();
        }

        public void SuperCURL()
        {
            if (delay > 0)
            {
                Thread.Sleep(1000 * delay);
            }
            //try
            //{
                JObject Curl = Net.Curl(url, method, values, headers, cookie, ecam);
                Curl["route"] = route;
                Curl["echo"] = echo;
                JObject ret = new JObject();
                ret["type"] = type;
                ret["data"] = Curl;
                ret["echo"] = echo;
                ret["statusCode"] = 200;
                send(ret.ToString(Newtonsoft.Json.Formatting.None));
            //}
            //catch (Exception e)
            //{
            //    ecam_action(e);
            //}

        }
        private void send(string data)
        {
            try
            {
                MainForm.socket.Send(Encoding.UTF8.GetBytes(data));
            }
            catch (Exception e)
            {
                ecam_action(e);
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
    }
}
