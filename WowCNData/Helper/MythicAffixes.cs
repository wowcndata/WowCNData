using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    using Helper;
    public class MythicAffixes
    {
        // STUPID!
        public class MythicAffixInfo
        {
            public string OName;
            public string LName;
        }
        static string[] MythicAffixesOriginalName = new string[]
        {
            "Raging", "Bursting", "Tyrannical", "Teeming", "Volcanic", "Bolstering", "Fortified", "Relentless", "Necrotic", "Skittish", "Sanguine", "Explosive", "Quaking", "Grievous"
        };
        static string[] MythicAffixesLocaleName = new string[]
        {
            "暴怒", "崩裂", "残暴", "繁盛", "火山", "激励", "强韧", "冷酷", "死疽", "无常", "血池", "易爆", "震荡", "重伤"
        };

        const int AFFIXES_SEQUENCE_START_TIMESTAMP = 1490828400;
        static List<List<string>> MythicAffixesSequence = new List<List<string>>()
        {
            new List<string>(){ "暴怒","火山","残暴"},
            new List<string>(){ "繁盛","易爆","强韧"},
            new List<string>(){ "激励","重伤","残暴"},
            new List<string>(){ "血池","火山","强韧"},
            new List<string>(){ "崩裂","无常","残暴"},
            new List<string>(){ "繁盛","震荡","强韧"},
            new List<string>(){ "暴怒","死疽","残暴"},
            new List<string>(){ "激励","无常","强韧"},
            new List<string>(){ "繁盛","死疽","残暴"},
            new List<string>(){ "血池","重伤","强韧"},
            new List<string>(){ "激励","易爆","残暴"},
            new List<string>(){ "崩裂","震荡","强韧"},
        };
        private static string LNameToOName(string name)
        {
            for(int i = 0;i<MythicAffixesLocaleName.Length;i++)
            {
                if (name == MythicAffixesLocaleName[i]) return MythicAffixesOriginalName[i];
            }
            return "";
        }
        public static List<MythicAffixInfo> GetWeekAffixes(int week)
        {
            List<MythicAffixInfo> result = new List<MythicAffixInfo>();
            var currentTimeStamp = Time.GetCurrentTimeStamp();
            var currentIndex = ((currentTimeStamp - AFFIXES_SEQUENCE_START_TIMESTAMP) / (3600 * 24 * 7) + week) % 12;
            var info = MythicAffixesSequence[currentIndex];
            foreach(var i in info)
            {
                result.Add(new MythicAffixInfo() { LName = i, OName = LNameToOName(i) });
            }
            return result;
        }
    }
}