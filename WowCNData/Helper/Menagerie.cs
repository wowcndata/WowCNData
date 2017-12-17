using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class Menagerie
    {
        private static List<string> PetsSequence = new List<string>()
        {
            "三傻鸟", "亡灵羊", "冰与火之歌", "鬣蜥人三兄弟",
            "火凤凰", "变小动物的三机械", "缝合二世", "三虚空行者",
            "暗黑三人组", "霜鬃野猪", "萝卜软泥三人组", "大嘴鸟",
            "弗洛里特王后大王", "戈隆兄弟俩", "蛆、虫、蛾"
        };
        private static DateTime MgStartTime = new DateTime(2015, 3, 13, 0, 0, 0);
        public static List<string> GetNextThreePets()
        {
            var result = new List<string>();
            var currentIndex = (int)Math.Floor((DateTime.Now - MgStartTime).TotalDays / PetsSequence.Count) % PetsSequence.Count;
            result.Add(PetsSequence[currentIndex++]);
            if (currentIndex >= PetsSequence.Count) currentIndex = 0;
            result.Add(PetsSequence[currentIndex++]);
            if (currentIndex >= PetsSequence.Count) currentIndex = 0;
            result.Add(PetsSequence[currentIndex]);
            return result;
        }
        private static List<string> FrSequence = new List<string>()
        {
            "守护者蕾娜-瓦尔莎拉", "阿库勒·河角-至高岭", "科尔宾-风暴峡湾", "莎乐丝-苏拉玛",
            "英帕斯-破碎海滩", "“活水”伊丽西娅-阿苏纳"
        };
        private static DateTime FrStartTime = new DateTime(2017, 10, 12, 0, 0, 0);
        public static List<string> GetFisherFriends()
        {
            var result = new List<string>();
            var currentIndex = (int)Math.Floor((DateTime.Now - FrStartTime).TotalDays / FrSequence.Count) % FrSequence.Count;
            result.Add(FrSequence[currentIndex++]);
            if (currentIndex >= FrSequence.Count) currentIndex = 0;
            result.Add(FrSequence[currentIndex++]);
            if (currentIndex >= FrSequence.Count) currentIndex = 0;
            result.Add(FrSequence[currentIndex]);
            return result;
        }
    }
}