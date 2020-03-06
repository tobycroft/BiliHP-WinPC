using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace BiliHP2020.func
{
    class Net
    {

        public static string ip = "127.0.0.1";
        public static bool proxy = true;
        public static int port = 9000;
        public static JObject Post(string url, string method, JObject values, JObject headers, JObject cookie)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            WebProxy px = new WebProxy(ip,port);
            req.Proxy = px;
            req.Method = method;
            CookieContainer cookies = new CookieContainer();
            if (cookie != null && cookie.HasValues)
            {
                foreach (var item in cookie)
                {
                    Cookie sk = new Cookie();
                    sk.Name = item.Key;
                    sk.Value = item.Value.ToString();
                    sk.Domain = req.RequestUri.Host;
                    cookies.Add(sk);
                }
            }
            if (headers != null && headers.HasValues)
            {
                foreach (var item in headers)
                {

                    switch (item.Key)
                    {
                        case "Accept":
                            req.Accept = item.Value.ToString();
                            break;

                        case "Connection":
                            req.Connection = item.Value.ToString();
                            break;

                        case "Content-length":
                        case "Content-Length":
                            req.ContentLength = item.Value.ToObject<int>();
                            break;

                        case "Content-type":
                        case "Content-Type":
                            req.ContentType = item.Value.ToString();
                            break;

                        case "Expect":
                            req.Expect = item.Value.ToString();
                            break;

                        case "Date":
                            //req.date = item.Value.ToString();
                            break;

                        case "Host":
                            //req.Headers = item.Value.ToString();
                            break;

                        case "If-Modified-Since	":
                        case "If-modified-since	":
                            //req.IfModifiedSince = item.Value.ToString();
                            break;

                        case "Range":
                            //req.Headers = item.Value.ToString();
                            break;


                        case "Referer":
                            req.Referer = item.Value.ToString();
                            break;

                        case "Transfer-Encoding	":
                        case "Transfer-encoding	":
                            req.TransferEncoding = item.Value.ToString();
                            break;

                        case "User-Agent":
                        case "User-agent":
                            req.UserAgent = item.Value.ToString();
                            break;


                        default:
                            req.Headers.Set(item.Key, item.Value.ToString());
                            break;
                    }
                }
            }

            req.CookieContainer = cookies;
            req.ContentType = "application/x-www-form-urlencoded";
            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            JObject dict = new JObject();
            foreach (var item in values)
            {
                dict.Add(item.Key, item.Value);
            }
            sw.Write(http_build_query(dict));
            sw.Close();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            JObject ret_header = new JObject();
            foreach (string item in resp.Headers)
            {
                ret_header.Add(item, resp.Headers[item]);
            }
            JObject ret_cookie = new JObject();
            foreach (Cookie item in resp.Cookies)
            {
                ret_cookie.Add(item.Name, item.Value);
            }
            Stream reStream = resp.GetResponseStream();
            string body = "";
            using (StreamReader sr = new StreamReader(reStream))
            {
                body = sr.ReadToEnd();
                sr.Close();
            }
            resp.Close();
            JObject ret = new JObject();
            ret["body"] = JObject.Parse(body);
            ret["headers"] = ret_header;
            ret["cookie"] = ret_cookie;
            return ret;
        }


        public static string http_build_query(JObject dict = null)
        {
            if (dict == null)
            {
                return "";
            }
            var builder = new UriBuilder();
            
            var query = HttpUtility.ParseQueryString(builder.Query);
            StringBuilder sb = new StringBuilder();
            foreach (var item in dict)
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(UrlEncode(item.Value.ToString()));
                sb.Append("&");
                //query.Add(item.Key,HttpUtility.UrlDecode(UrlEncode(item.Value.ToString())));
            }
            
            return sb.ToString().TrimEnd('&');
        }
        public static string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public void Post_Full(string url, JObject cookie, JObject value)
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
            Dictionary<string, dynamic> self_headers = new Dictionary<string, dynamic>();

            foreach (string item in resp.Headers)
            {
                self_headers.Add(item, resp.Headers[item]);
            }

            header = JObject.FromObject(self_headers);



            byte[] buffer = new byte[int.Parse(resp.ContentLength.ToString())];
            Stream reStream = resp.GetResponseStream();
            string body = "";
            using (StreamReader sr = new StreamReader(reStream))
            {
                body = sr.ReadToEnd();
            }
        }

        public void Get_Full()
        {

        }
    }
}
