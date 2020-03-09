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
        public string url;
        public string method;
        public JObject values;
        public JObject headers;
        public JObject cookie;
        public string type;
        public string echo;
        public string route;
        public int delay = 0;

        public Socket socket;

        public void Curl()
        {
            Thread.Sleep(1000 * delay);
            SuperCURL();
        }

        public void SuperCURL()
        {
            Net.Post();
        }
    }
}
