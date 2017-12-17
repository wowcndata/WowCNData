using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class BrokenIsle
    {
        public class BrokenIsleBuildingInfo
        {
            public string Name;
            public int State;
            public string Value;
            public float Percent;
            public string BuffName;
            public string BuffDesc;
            public string Prediction;
            public string StateText;
        }
        private static string[] Buildings = new string[] { "mageTower", "commandCenter", "netherDisruptor" };

        public static List<BrokenIsleBuildingInfo> GetBuildings()
        {
            List<BrokenIsleBuildingInfo> result = new List<BrokenIsleBuildingInfo>();

            const int MULTIPLIER = 10000000;
            var lastUpdateElapsed = StaticData.BrokenIsleData["lastUpdateElapsed"];
            foreach (var building in Buildings)
            {
                BrokenIsleBuildingInfo info = new BrokenIsleBuildingInfo();
                info.Name = building;
                info.State = StaticData.BrokenIsleData[building + "State"];
                info.StateText = LegionAssault.GetStateText(info.State);
                var data = StaticData.BrokenIsleData[building + "Data"];
                if (info.State == 1) // Building
                {
                    var passed = StaticData.BrokenIsleData[building + "DataDelta"];
                    if (passed == 0)
                    {
                        info.Value = "计算中...";
                        info.Prediction = "";
                        info.Percent = 0;
                    }
                    else
                    {
                        info.Value = Math.Round((data * 1.0 / MULTIPLIER * 100),2) + "%";
                        var x = (MULTIPLIER - data) / passed * lastUpdateElapsed;
                        info.Prediction = "预计将在" + Time.FormatTimeSpan(TimeSpan.FromSeconds((MULTIPLIER - data) / passed * lastUpdateElapsed)) + "后建成";
                        info.Percent = (float)Math.Round(data * 100.0 / MULTIPLIER, 2);
                    }
                }
                else if(info.State == 2) // active
                {
                    info.Percent = 100;
                    info.Prediction = "";
                    info.Value = "将在" + Time.FormatTimeSpan((Time.TimeStampToDateTime(StaticData.BrokenIsleData[building + "ChangeTime"]).AddDays(3) - DateTime.Now)) + "后被摧毁";
                }
                else if(info.State == 3) // underattack
                {
                    var timeLeft = Convert.ToInt32(data * 24.0 * 3600 / MULTIPLIER);
                    info.Value = "剩余" + Time.FormatTimeSpan(TimeSpan.FromSeconds(timeLeft));
                    info.Prediction = "";
                    info.Percent = (float)Math.Round(data * 100.0 / MULTIPLIER, 2);
                }
                else if(info.State == 4) // detroyed
                {
                    info.Prediction = "";
                    info.Percent = 0;
                    info.Value = "将在" + Time.FormatTimeSpan((Time.TimeStampToDateTime(StaticData.BrokenIsleData[building + "ChangeTime"]).AddDays(1) - DateTime.Now)) + "后可捐献";

                }
                if (info.State != 4)
                {
                    info.BuffName = StaticData.BrokenIsleBuildingsBuffData[StaticData.BrokenIsleData[building + "Buff"]];
                    info.BuffDesc = StaticData.BrokenIsleBuildingsBuffDescData[StaticData.BrokenIsleData[building + "Buff"]];
                }
                else
                {
                    info.BuffName = "下次Buff:"+ StaticData.BrokenIsleBuildingsBuffData[StaticData.DynamicData[building+"NextBuffId"]];
                    info.BuffDesc = StaticData.BrokenIsleBuildingsBuffDescData[StaticData.DynamicData[building + "NextBuffId"]];
                    
                }
                result.Add(info);
            }
            return result;
        }
    }
}