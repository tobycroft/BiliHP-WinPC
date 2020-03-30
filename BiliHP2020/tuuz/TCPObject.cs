using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BiliHP2020.tuuz
{
    class TCPObject
    {
        public static JObject tcpobj(string json)
        {
            JArray arr = new JArray();
            JObject ret = new JObject();

            if (json.Length > 655350)
            {
                ret["json"] = "";
                ret["arr"] = arr;
                return ret;
            }

            string[] strs = Regex.Split(json, "}{");


            if (strs.Length > 1)
            {

                string unable = "";
                int index = 0;
                foreach (var item in strs)
                {
                    if (index == 0)
                    {
                        try
                        {
                            JObject temp = JObject.Parse(item+"}");
                            arr.Add(temp);
                        }
                        catch
                        {

                        }
                    }else if (strs.Length==index+1)
                    {
                        try
                        {
                            JObject temp = JObject.Parse("{"+item);
                            arr.Add(temp);
                        }
                        catch 
                        {
                            unable += "{" + item;
                        }
                    }
                    else
                    {
                        try
                        {
                            JObject temp = JObject.Parse("{"+item+"}");
                            arr.Add(temp);

                        }
                        catch 
                        {
                            unable += "{" + item + "}";
                        }   
                    }
                    index++;
                }
                ret["json"] = unable;
                ret["arr"] = arr;
                return ret;
            }
            //else if (strs.Length > 1)
            //{
            //    try
            //    {
            //        JObject temp = JObject.Parse(strs[0] + "}");
            //        arr.Add(temp);
            //        json = "{" + strs[1];
            //        ret["json"] = json;
            //        ret["arr"] = arr;

            //        return ret;
            //    }
            //    catch
            //    {
            //        ret["json"] = json;
            //        ret["arr"] = arr;
            //        return ret;
            //    }
            //}
            else
            {

                try
                {
                    JObject temp = JObject.Parse(json);
                    arr.Add(temp);
                    ret["json"] = "";
                    ret["arr"] = arr;
                    return ret;
                }
                catch 
                {
                    ret["json"] = json;
                    ret["arr"] = arr;
                    return ret;
                }
            }            
        }
    }
}
