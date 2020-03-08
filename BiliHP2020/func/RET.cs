using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiliHP2020.func
{
    class RET
    {
        public static string ws_succ(string type,int code, JObject data,string echo)
        {
            if (data==null)
            {
                data = new JObject();
            }
            JObject job = new JObject();
            job["type"] = type;
            job["code"] = code;
            job["data"] = data;
            job["echo"] = echo;
            return job.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string ws_succ2(string type, int code, JObject data, string echo, JObject values)
        {
            if (data == null)
            {
                data = new JObject();
            }
            JObject job = new JObject();
            job["type"] = type;
            job["code"] = code;
            job["data"] = data;
            job["echo"] = echo;
            job["values"] = values;
            return job.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
