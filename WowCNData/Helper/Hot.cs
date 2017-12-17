using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public static class Hot
    {
        public static List<string> GetHotNews()
        {
            List<string> result = new List<string>();
            int count = 0;
            try
            {
                foreach(var kv in StaticData.DynamicData)
                {
                    if(kv.Key.Contains("Hot") && kv.Key.Contains("#"))
                    {
                        var date = Time.TimeStampToDateTime(kv.Value);
                        var expire = date.AddDays(1);
                        if (expire < DateTime.Now) continue;
                        var spilt = kv.Key.Split(new string[] { "#" }, StringSplitOptions.None);
                        var news = date.ToString("MM-dd HH:mm") + ":" + spilt[1];
                        result.Add(news);
                        count++;
                    }
                    if (count == 3) continue;
                }
            }
            catch
            {
                return new List<string>() { "", "", "" };
            }
            for(int i = count;i<3;i++)
            {
                result.Add("---");
            }
            return result;
        }
    }
}