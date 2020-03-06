using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace BiliHP2020.func
{
    class CURL:MainForm
    {
        public string url;
        public string method;
        public JObject values;
        public JObject headers;
        public JObject cookie;
        public string type;
        public string echo;
        public string conn;
        public string route;
        public int delay;

        public void Curl()
        {
            Thread th = new Thread(SuperCurl);
            th.Start();
        }

        public void SuperCurl()
        {
            string url = this.url;
            string method = this.method;
            JObject values = this.values;
            JObject headers = this.headers;
            JObject cookie = this.cookie;
            string type = this.type;
            string echo = this.echo;
            string conn = this.conn;
            string route = this.route;
            int delay = this.delay;

            Thread.Sleep(delay);

            //---------req-------
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            CookieContainer cookies = new CookieContainer();
            Cookie sk = new Cookie();
            sk.Name = "LIVE_BUVID";
            sk.Value = "AUTO7515833991642284";
            sk.Domain = req.RequestUri.Host;

            cookies.Add(sk);
            req.CookieContainer = cookies;

            if (this.method == "post")
            {

            }
            else
            {

            }






            //--------------ret----------
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            JObject header = new JObject();
            Dictionary<string, dynamic> self_headers = new Dictionary<string, dynamic>();

            foreach (string item in resp.Headers)
            {
                headers.Add(item, resp.Headers[item]);
            }

            header = JObject.FromObject(headers);

 

            byte[] buffer = new byte[int.Parse(resp.ContentLength.ToString())];
            Stream reStream = resp.GetResponseStream();
            string body = "";
            using (StreamReader sr = new StreamReader(reStream))
            {
                body = sr.ReadToEnd();
            }
            
        }

  

    }
}
