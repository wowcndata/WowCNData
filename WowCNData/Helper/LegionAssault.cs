using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class LegionAssault
    {
        class Time
        {
            internal static int GetCurrentTimeStamp()
            {
                return (int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
            }
            internal static int GetTimeStamp(DateTime time)
            {
                return (int)(time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
            }
            internal static DateTime TimeStampToDateTime(int timeStamp)
            {
                DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return dateTimeStart.AddSeconds(timeStamp);
            }
            internal static string FormatTimeSpan(TimeSpan ts)
            {
                string ret = "";
                if (ts.Days > 0)
                {
                    ret += ts.Days + "天";
                }
                if (ts.Hours > 0)
                {
                    ret += ts.Hours + "时";
                }
                if (ts.Minutes > 0)
                {
                    ret += ts.Minutes + "分";
                }
                return ret;
            }
        }
        public class LegionAssaultInfo
        {
            public string Area;
            public bool Started;
            public string StartDateTime;
            public string EndOrStartTimeSpan;
        }
        static string[] AssaultSequence = new string[]
        {
            "瓦尔莎拉", "风暴峡湾", "阿苏纳", "至高岭", "风暴峡湾", "至高岭", "瓦尔莎拉", "阿苏纳","风暴峡湾","瓦尔莎拉","至高岭","阿苏纳"
        };
        const int ASSAULT_DURATION = 6 * 3600;
        const int ASSAULT_COOLDOWN = (int)(12.5 * 3600);
        const int ASSAULT_PERIOD = ASSAULT_COOLDOWN + ASSAULT_DURATION;
        const int ASSAULT_START_TIMESTAMP = 1491433200;
        static DateTime ASSAULT_START_DATETIME = new DateTime(2017, 4, 6, 7, 0, 0);
        public static List<LegionAssaultInfo> GetRecentAssaluts(int count)
        {
            List<LegionAssaultInfo> result = new List<LegionAssaultInfo>();
            //var timestamp = Time.GetTimeStamp(DateTime.Now);
            var timestamp = Time.GetTimeStamp(DateTime.Now);
            var timeElapsed = timestamp - ASSAULT_START_TIMESTAMP;
            var currentPeriod = timeElapsed % (ASSAULT_PERIOD);
            var currentIndex = timeElapsed / ASSAULT_PERIOD % AssaultSequence.Length;
            var startDate = DateTime.Now;

            int loops = count;
            if (currentPeriod < ASSAULT_DURATION)
            {
                LegionAssaultInfo tempInfo = new LegionAssaultInfo();
                tempInfo.Started = true;
                tempInfo.EndOrStartTimeSpan = Time.FormatTimeSpan( TimeSpan.FromSeconds(ASSAULT_DURATION - currentPeriod));
                tempInfo.Area = AssaultSequence[currentIndex];
                result.Add(tempInfo);
                loops--;
            }
            startDate = ASSAULT_START_DATETIME.AddSeconds(timeElapsed / ASSAULT_PERIOD * ASSAULT_PERIOD + ASSAULT_PERIOD);
            for (int i = 0; i < loops; i++)
            {
                currentIndex++;
                if (currentIndex == AssaultSequence.Length) currentIndex = 0;
                LegionAssaultInfo info = new LegionAssaultInfo();
                info.Started = false;
                info.Area = AssaultSequence[currentIndex];
                info.StartDateTime = GetDatePrefix(startDate) + startDate.ToString("HH:mm");
                info.EndOrStartTimeSpan = Time.FormatTimeSpan(startDate - Time.TimeStampToDateTime(timestamp));
                result.Add(info);
                startDate = startDate.AddSeconds(ASSAULT_PERIOD);
                
            }
            return result;
        }
        public static string GetDatePrefix(DateTime date)
        {
            var now = DateTime.Now;
            if (date.Day == now.Day) return "今天";
            now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var dayPassed = Math.Floor((date - now).TotalSeconds / (3600.0 * 24));
            if (dayPassed == 1) return "明天";
            if (dayPassed == 2) return "后天";
            return dayPassed + "天后";
        }
        public static string GetStateText(int state)
        {
            if (state == 1) return "建造中";
            if (state == 2) return "已激活";
            if (state == 3) return "遭到攻击";
            if (state == 4) return "被摧毁";
            return "未知";
        }
    }
}