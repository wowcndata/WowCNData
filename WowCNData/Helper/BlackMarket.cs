using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
namespace WowCNData.Helper
{
    public class BlackMarket
    {
        public class BMData
        {
            public string Name;
            public string Realm;
            public string TimeLeft;
        }
        public static DateTime LastUpdate = new DateTime(1970, 1, 1, 1, 1, 1);
        public static List<BMData> BMDatas = new List<BMData>();
        public static void RequestUpdate()
        {
            if((DateTime.Now - LastUpdate).TotalHours > 24)
            {
                LastUpdate = DateTime.Now;
                GatherData();
            }
            if(LastUpdate.Date != DateTime.Now.Date)
            {
                LastUpdate = DateTime.Now;
                GatherData();
            }
            if(LastUpdate.Hour >= 23)
            {
                if((DateTime.Now - LastUpdate).TotalMinutes > 10)
                {
                    LastUpdate = DateTime.Now;
                    GatherData();
                }
            }
        }
        public static void GatherData()
        {
            // !!! FROM SOMEWHERE...
            var data = new WebClient().DownloadString("http://www.baidu.com");
            BMDatas = JsonHelper.DeserializeFromString<List<BMData>>(data);
            if (BMDatas == null) BMDatas = new List<BMData>();
        }
    }
}