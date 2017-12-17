using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public static class StaticData
    {
        public static Dictionary<string, int> BrokenIsleData = new Dictionary<string, int>();
        public static Dictionary<int, string> BrokenIsleBuildingsBuffData = new Dictionary<int, string>();
        public static Dictionary<int, string> BrokenIsleBuildingsBuffDescData = new Dictionary<int, string>();
        public static string LocalStoragePath = AppDomain.CurrentDomain.BaseDirectory + "LocalStorage\\";
        public static string BrokenIsleDataFile = LocalStoragePath + "BrokenIsleData.json";
        public static string DynamicDataFile = LocalStoragePath + "DynamicData.json";
        public static Dictionary<string, string> NavBarData = new Dictionary<string, string>();
        public static List<string> HordeCDAvailable = new List<string>();
        public static DateTime HordeCDLastUpdate = new DateTime(1970, 1, 1, 1, 1, 1);
        public static Dictionary<string, int> DynamicData = new Dictionary<string, int>();
        public static Dictionary<string, string> SpecName = new Dictionary<string, string>();
        public static Dictionary<string, string> SpecAttrib = new Dictionary<string, string>();
        public static Dictionary<string, string> RaceName = new Dictionary<string, string>();
        public static Dictionary<string, string> ClassName = new Dictionary<string, string>();
        public static Dictionary<string, string> AttribName = new Dictionary<string, string>();
        public static void UpdateDynamicData()
        {
            try
            {
                DynamicData = JsonHelper.DeserializeFromFile<Dictionary<string, int>>(DynamicDataFile);
                if (DynamicData == null || DynamicData.Count == 0)
                {
                    DynamicData = new Dictionary<string, int>();
                    DynamicData["mageTowerNextBuffId"] = 0;
                    DynamicData["commandCenterNextBuffId"] = 0;
                    DynamicData["netherDisruptorNextBuffId"] = 0;
                }
                JsonHelper.SerializeToFile(DynamicDataFile, DynamicData);
                DynamicData = JsonHelper.DeserializeFromFile<Dictionary<string, int>>(DynamicDataFile);
            }
            catch
            {

            }
        }
        static StaticData()
        {
            BrokenIsleData = new Dictionary<string, int>();
            BrokenIsleBuildingsBuffData = new Dictionary<int, string>();
            BrokenIsleBuildingsBuffDescData = new Dictionary<int, string>();
            DynamicData = new Dictionary<string, int>();
            LocalStoragePath = AppDomain.CurrentDomain.BaseDirectory + "LocalStorage\\";
            BrokenIsleDataFile = LocalStoragePath + "BrokenIsleData.json";
            #region Building Json Data
            BrokenIsleData = JsonHelper.DeserializeFromFile<Dictionary<string, int>>(BrokenIsleDataFile);
            if (BrokenIsleData == null || BrokenIsleData.Count == 0)
            {
                BrokenIsleData = new Dictionary<string, int>();
                BrokenIsleData["mageTowerChangeTime"] = 0;
                BrokenIsleData["netherDisruptorChangeTime"] = 0;
                BrokenIsleData["mageTowerState"] = 0;
                BrokenIsleData["mageTowerData"] = 0;
                BrokenIsleData["commandCenterState"] = 0;
                BrokenIsleData["netherDisruptorData"] = 0;
                BrokenIsleData["commandCenterData"] = 0;
                BrokenIsleData["mageTowerBuff"] = 0;
                BrokenIsleData["netherDisruptorState"] = 0;
                BrokenIsleData["netherDisruptorBuff"] = 0;
                BrokenIsleData["commandCenterBuff"] = 0;
                BrokenIsleData["mageTowerDataDelta"] = 0;
                BrokenIsleData["commandCenterDataDelta"] = 0;
                BrokenIsleData["netherDisruptorDataDelta"] = 0;
                BrokenIsleData["wowToken"] = 0;
                BrokenIsleData["lastUpdateElapsed"] = 0;
                BrokenIsleData["lastUpdate"] = 0;
            }
            UpdateDynamicData();
            #endregion
            #region Building Buff Name/Description Data
            BrokenIsleBuildingsBuffData[237137] = "学识渊博";
            BrokenIsleBuildingsBuffDescData[237137] = "当你在团队副本或地下城中拾取用来获得神器能量的物品时，有几率获得额外的神器能量物品。";
            BrokenIsleBuildingsBuffData[237139] = "势不可挡";
            BrokenIsleBuildingsBuffDescData[237139] = "完成世界任务时有几率获得额外的神器能量物品。";
            BrokenIsleBuildingsBuffData[240979] = "声望高绝";
            BrokenIsleBuildingsBuffDescData[240979] = "在抗魔联军阵营的声望获取速度提高30%。";
            BrokenIsleBuildingsBuffData[240980] = "轻盈如羽";
            BrokenIsleBuildingsBuffDescData[240980] = "你的坐骑可以在水上行走。";
            BrokenIsleBuildingsBuffData[239966] = "全力备战";
            BrokenIsleBuildingsBuffDescData[239966] = "获得抗魔联军战争物资时有几率获得额外的物资。";
            BrokenIsleBuildingsBuffData[240986] = "优秀的勇士";
            BrokenIsleBuildingsBuffDescData[240986] = "集中精力为你提供一件传说品质的追随者装备。";
            BrokenIsleBuildingsBuffData[240989] = "协调强化";
            BrokenIsleBuildingsBuffDescData[240989] = "完成世界任务时有几率获得被污染的强化符文。";
            BrokenIsleBuildingsBuffData[240987] = "充分准备";
            BrokenIsleBuildingsBuffDescData[240987] = "在破碎海滩难度较低的区域内，所有主要属性提高10%。";
            BrokenIsleBuildingsBuffData[239967] = "封印你的命运";
            BrokenIsleBuildingsBuffDescData[239967] = "虚空干扰器激活时，你每天都可以回到破碎海滩完成一个任务，获得一枚免费的破碎命运印记。";
            BrokenIsleBuildingsBuffData[239968] = "命运在对你微笑";
            BrokenIsleBuildingsBuffDescData[239968] = "如果你在破碎群岛进行额外拾取时没有如愿，有几率返还使用的破碎命运印记。";
            BrokenIsleBuildingsBuffData[239969] = "虚空风暴";
            BrokenIsleBuildingsBuffDescData[239969] = "获得虚空碎片时有几率得到额外的虚空碎片。";
            BrokenIsleBuildingsBuffData[240985] = "强化缰绳";
            BrokenIsleBuildingsBuffDescData[240985] = "可以在骑乘状态下与目标互动。";

            BrokenIsleBuildingsBuffData[0] = "---";
            BrokenIsleBuildingsBuffDescData[0] = "---";
            #endregion
            #region SimCraft
            SpecAttrib["arms"] = "strength";
            SpecAttrib["fury"] = "strength";
            SpecAttrib["retribution"] = "strength";
            SpecAttrib["beast_mastery"] = "agility";
            SpecAttrib["marksmanship"] = "agility";
            SpecAttrib["survival"] = "agility";
            SpecAttrib["assassination"] = "agility";
            SpecAttrib["outlaw"] = "agility";
            SpecAttrib["subtlety"] = "agility";
            SpecAttrib["discipline"] = "intellect";
            SpecAttrib["shadow"] = "intellect";
            SpecAttrib["blood"] = "strength";
            SpecAttrib["unholy"] = "strength";
            SpecAttrib["elemental"] = "intellect";
            SpecAttrib["enhancement"] = "agility";
            SpecAttrib["arcane"] = "intellect";
            SpecAttrib["fire"] = "intellect";
            SpecAttrib["affliction"] = "intellect";
            SpecAttrib["demonology"] = "intellect";
            SpecAttrib["destruction"] = "intellect";
            SpecAttrib["brewmaster"] = "agility";
            SpecAttrib["mistweaver"] = "intellect";
            SpecAttrib["windwalker"] = "agility";
            SpecAttrib["balance"] = "intellect";
            SpecAttrib["feral"] = "agility";
            SpecAttrib["guardian"] = "agility";
            SpecAttrib["havoc"] = "agility";
            SpecAttrib["vengeance"] = "agility";
            SpecAttrib["protection"] = "strength";
            SpecAttrib["frost"] = "intellect";
            SpecAttrib["restoration"] = "intellect";

            AttribName["intellect"] = "智力";
            AttribName["strength"] = "力量";
            AttribName["agility"] = "敏捷";

            SpecName["arms"] = "武器";
            SpecName["fury"] = "狂怒";
            SpecName["retribution"] = "惩戒";
            SpecName["beast_mastery"] = "野兽控制";
            SpecName["marksmanship"] = "射击";
            SpecName["survival"] = "生存";
            SpecName["assassination"] = "刺杀";
            SpecName["outlaw"] = "狂徒";
            SpecName["subtlety"] = "敏锐";
            SpecName["discipline"] = "戒律";
            SpecName["shadow"] = "暗影";
            SpecName["blood"] = "鲜血";
            SpecName["unholy"] = "邪恶";
            SpecName["elemental"] = "元素";
            SpecName["enhancement"] = "增强";
            SpecName["arcane"] = "奥术";
            SpecName["fire"] = "火焰";
            SpecName["affliction"] = "痛苦";
            SpecName["demonology"] = "恶魔";
            SpecName["destruction"] = "毁灭";
            SpecName["brewmaster"] = "酿酒";
            SpecName["mistweaver"] = "织雾";
            SpecName["windwalker"] = "踏风";
            SpecName["balance"] = "平衡";
            SpecName["feral"] = "野性";
            SpecName["guardian"] = "守护";
            SpecName["havoc"] = "浩劫";
            SpecName["vengeance"] = "复仇";
            SpecName["protection"] = "防护";
            SpecName["frost"] = "冰霜";
            SpecName["restoration"] = "恢复";

            ClassName["warrior"] = "战士";
            ClassName["hunter"] = "猎人";
            ClassName["monk"] = "武僧";
            ClassName["paladin"] = "圣骑士";
            ClassName["rogue"] = "潜行者";
            ClassName["shaman"] = "萨满祭司";
            ClassName["mage"] = "法师";
            ClassName["warlock"] = "术士";
            ClassName["druid"] = "德鲁伊";
            ClassName["deathknight"] = "死亡骑士";
            ClassName["priest"] = "牧师";
            ClassName["demonhunter"] = "恶魔猎手";

            RaceName["human"] = "人类";
            RaceName["orc"] = "兽人";
            RaceName["dwarf"] = "矮人";
            RaceName["night_elf"] = "暗夜精灵";
            RaceName["undead"] = "亡灵";
            RaceName["tauren"] = "牛头人";
            RaceName["gnome"] = "侏儒";
            RaceName["troll"] = "巨魔";
            RaceName["goblin"] = "地精";
            RaceName["blood_elf"] = "血精灵";
            RaceName["draenei"] = "德莱尼";
            RaceName["worgen"] = "狼人";
            RaceName["pandaren"] = "熊猫人";
            RaceName["pandaren_alliance"] = "熊猫人";
            RaceName["pandaren_horde"] = "熊猫人";

            #endregion
            NavBarData["/"] = "今日破碎群岛";
            NavBarData["/freeInstanceCD"] = "免费CD君(Beta)";
            NavBarData["/tools/simcraft"] = "DPS在线模拟";
            NavBarData["/blackmarket"] = "黑市拍卖行";
            NavBarData["/extra"] = "其他数据";
            NavBarData["/appAddons"] = "插件&APP";
        }
    }
}