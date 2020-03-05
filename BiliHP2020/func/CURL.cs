using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace BiliHP2020.func
{
    class CURL
    {
        public void Curl(string url, string method, Dictionary<string, object> values, Dictionary<string, object> headers, Dictionary<string, string> cookie, string type, string echo, string conn, string route, int delay)
        {
            Thread th = new Thread(SuperCurl);
        }

        public void SuperCurl(string url, string method, Dictionary<string, object> values, Dictionary<string, object> headers, Dictionary<string, string> cookie,string type,string echo ,string conn,string route)
        {
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













            //--------------ret----------
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            JObject header = new JObject();
            Dictionary<string, dynamic> headers = new Dictionary<string, dynamic>();

            foreach (string item in resp.Headers)
            {
                headers.Add(item, resp.Headers[item]);
            }

            header = JObject.FromObject(headers);

            richTextBox1.Text += header;

            byte[] buffer = new byte[int.Parse(resp.ContentLength.ToString())];
            Stream reStream = resp.GetResponseStream();
            string body = "";
            using (StreamReader sr = new StreamReader(reStream))
            {
                body = sr.ReadToEnd();
            }
            richTextBox1.Text += body;
        }

    }
}
