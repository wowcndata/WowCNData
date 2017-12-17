using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WowCNData.Helper
{
    public class WCL
    {
        public class WCLData
        {
            public string Spec;
            public float Score;
            public string Color;
        }
        public static DateTime LastUpdate = new DateTime(1970, 1, 1, 1, 1, 1);
        public static Dictionary<int, List<WCLData>> WCLDatas = new Dictionary<int, List<WCLData>>();
        public static readonly object WCLLock = new object();
        private static void RequestUpdate()
        {
            if ((DateTime.Now - LastUpdate).TotalHours > 4)
            {
                LastUpdate = DateTime.Now;
                GatherData();
            }
        }
        public static List<WCLData> GetData(int type = 75)
        {
            var result = new List<WCLData>();
            lock (WCLLock)
            {
                RequestUpdate();
                if(type == 75) WCLDatas[75].ForEach(t => result.Add(t));
                if (type == 90) WCLDatas[90].ForEach(t => result.Add(t));
                if (type == 99) WCLDatas[99].ForEach(t => result.Add(t));
            }
            return result;
        }
        private static void GatherData()
        {
            WCLDatas = new Dictionary<int, List<WCLData>>();
            var percent = new int[] { 75, 90, 99 };
            foreach (var p in percent)
            {
                WCLDatas[p] = new List<WCLData>();
                var data = new WebClient().DownloadString("https://www.warcraftlogs.com/en/statistics/table/dps/13/0/5/20/2/" + p + "/1/14/0/DPS/Any/All/normalized/single/?keystone=15");
                var specData = SString.BetweenArray(data, "var series = { name:", "filterTimespan");
                foreach (var spec in specData)
                {
                    try
                    {
                        var oSpecName = SString.Between(spec, "\"", "\"");
                        var specName = GetSpecLocaleName(oSpecName);
                        if (specName == "") continue;
                        var floatData = SString.Between(spec, "series.data.push(", ")");
                        var value = (float)Math.Round(float.Parse(floatData), 2);
                        var wData = new WCLData() { Spec = specName, Score = value, Color = GetSpecColor(oSpecName) };
                        WCLDatas[p].Add(wData);
                    }
                    catch
                    {
                        // format error,or wrong spec
                    }
                }
                WCLDatas[p] = WCLDatas[p].OrderBy(t => t.Score).ToList();
            }
        }
        public static Dictionary<string, string> SpecLocale = new Dictionary<string, string>();
        public static Dictionary<string, string> SpecColor = new Dictionary<string, string>();
        public static string GetSpecColor(string spec)
        {
            CheckLoaded();
            foreach(var kv in SpecColor)
            {
                if (spec.Contains(kv.Key)) return kv.Value;
            }
            return "#eee";
        }
        private static void CheckLoaded()
        {
            if (SpecLocale.Count == 0)
            {
                SpecLocale["Subtlety Rogue"] = "敏锐贼";
                SpecLocale["Combat Rogue"] = "战斗贼";
                SpecLocale["Assassination Rogue"] = "战斗贼";
                SpecLocale["Elemental Shaman"] = "电萨";
                SpecLocale["Enhancement Shaman"] = "增强萨";
                SpecLocale["Affliction Warlock"] = "痛苦术";
                SpecLocale["Demonology Warlock"] = "恶魔术";
                SpecLocale["Destruction Warlock"] = "毁灭术";
                SpecLocale["Arms Warrior"] = "武器战";
                SpecLocale["Fury Warrior"] = "狂暴战";
                SpecLocale["Gladiator Warrior"] = "防战";
                SpecLocale["Shadow Priest"] = "暗牧";
                SpecLocale["Balance Druid"] = "平衡德";
                SpecLocale["Feral Druid"] = "野德";
                SpecLocale["Beast Mastery Hunter"] = "兽王猎";
                SpecLocale["Marksmanship Hunter"] = "射击猎";
                SpecLocale["Survival Hunter"] = "生存猎";
                SpecLocale["Arcane Mage"] = "奥法";
                SpecLocale["Fire Mage"] = "火法";
                SpecLocale["Frost Mage"] = "冰法";
                SpecLocale["Windwalker Monk"] = "踏风";
                SpecLocale["Retribution Paladin"] = "惩戒骑";
                SpecLocale["Havoc Demon Hunter"] = "浩劫";
                SpecColor["DeathKnight"] = "rgb(196, 31, 59)";
                SpecColor["DemonHunter"] = "#a330c9";
                SpecColor["Druid"] = "rgb(255, 125, 10)";

                SpecColor["Hunter"] = "rgb(171, 212, 115)";
                SpecColor["Mage"] = "rgb(105, 204, 240)";
                SpecColor["Monk"] = "#2D9B78";
                SpecColor["Paladin"] = "rgb(245, 140, 186)";
                SpecColor["Priest"] = "#eeeeee";
                SpecColor["Rogue"] = "rgb(255, 245, 105)";
                SpecColor["Shaman"] = "rgb(36, 89, 255)";
                SpecColor["Warlock"] = "rgb(148, 130, 201)";
                SpecColor["Warrior"] = "rgb(199, 156, 110)";
            }
        }
        public static string GetSpecLocaleName(string spec)
        {
            CheckLoaded();
            string specName;
            if (SpecLocale.TryGetValue(spec, out specName))
            {
                return specName;
            }
            /*
            if (SpecLocale.TryGetValue(spec, out string specName))
            {
                return specName;
            }*/
            return "";
        }
    }
}