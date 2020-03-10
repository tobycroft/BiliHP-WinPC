using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private Socket socket;

        public static void Curl(Socket socket,string url,string method, JObject values, JObject headers, JObject cookie, string type, string echo, string route, int delay)
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
            sp.socket = socket;
            Thread th = new Thread(sp.SuperCURL);
            th.IsBackground = true;
            th.Start();
        }

        public void SuperCURL()
        {
            if (delay>0)
            {
                Thread.Sleep(1000 * delay);
            }
            JObject Curl = Net.Curl(url, method, values, headers, cookie);
            Curl["route"] = route;
            Curl["echo"] = echo;
            JObject ret = new JObject();
            ret["type"] = type;
            ret["data"] = Curl;
            ret["echo"] = echo;
            ret["statusCode"] = 200;
            send(ret.ToString(Newtonsoft.Json.Formatting.None));
        }
        private void send(string data)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(data));

            }
            catch (Exception)
            {

            }
        }
    }
}
