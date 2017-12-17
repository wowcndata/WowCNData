using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WowCNData.Controllers
{
    using Helper;
    public class uploadController : Controller
    {
        // GET: upload

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult updateInternal()
        {
            var key = Request["key"];
            if (string.IsNullOrEmpty(key) || key != "wowdatakey")
            {
                return Json(new { Error = 1 });
            }
            var data = Request["data"];
            if (string.IsNullOrEmpty(data)) return Json(new { Error = 2 });
            var spilt = data.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            var updateTimeStamp = int.Parse(spilt[0]);
            var wowToken = int.Parse(spilt[1]);

            var mageTowerState = int.Parse(spilt[2]);
            var mageTowerData = int.Parse(spilt[3]);
            var commandCenterState = int.Parse(spilt[4]);
            var commandCenterData = int.Parse(spilt[5]);
            var netherDisruptorState = int.Parse(spilt[6]);
            var netherDisruptorData = int.Parse(spilt[7]);
            var mageTowerBuff = int.Parse(spilt[8]);
            var commandCenterBuff = int.Parse(spilt[9]);
            var netherDisruptorBuff = int.Parse(spilt[10]);

            var mageTowerPreState = StaticData.BrokenIsleData["mageTowerState"];
            var mageTowerPreData = StaticData.BrokenIsleData["mageTowerData"];
            var commandCenterPreState = StaticData.BrokenIsleData["commandCenterState"];
            var commandCenterPreData = StaticData.BrokenIsleData["commandCenterData"];
            var netherDisruptorPreState = StaticData.BrokenIsleData["netherDisruptorState"];
            var netherDisruptorPreData = StaticData.BrokenIsleData["netherDisruptorData"];
            var lastUpdate = StaticData.BrokenIsleData["lastUpdate"];
            if (mageTowerPreState != mageTowerState)
            {
                StaticData.BrokenIsleData["mageTowerChangeTime"] = updateTimeStamp;
            }
            if (commandCenterPreState != commandCenterState)
            {
                StaticData.BrokenIsleData["commandCenterChangeTime"] = updateTimeStamp;
            }
            if (netherDisruptorPreState != netherDisruptorState)
            {
                StaticData.BrokenIsleData["netherDisruptorChangeTime"] = updateTimeStamp;
            }
            var mageTowerDataDelta = mageTowerData - mageTowerPreData;
            var commandCenterDataDelta = commandCenterData - commandCenterPreData;
            var netherDisruptorDataDelta = netherDisruptorData - netherDisruptorPreData;
            StaticData.BrokenIsleData["mageTowerState"] = mageTowerState;
            StaticData.BrokenIsleData["mageTowerData"] = mageTowerData;
            StaticData.BrokenIsleData["commandCenterState"] = commandCenterState;
            StaticData.BrokenIsleData["netherDisruptorData"] = netherDisruptorData;
            StaticData.BrokenIsleData["commandCenterData"] = commandCenterData;
            StaticData.BrokenIsleData["mageTowerBuff"] = mageTowerBuff;
            StaticData.BrokenIsleData["netherDisruptorState"] = netherDisruptorState;
            StaticData.BrokenIsleData["netherDisruptorBuff"] = netherDisruptorBuff;
            StaticData.BrokenIsleData["commandCenterBuff"] = commandCenterBuff;
            StaticData.BrokenIsleData["mageTowerDataDelta"] = mageTowerDataDelta;
            StaticData.BrokenIsleData["commandCenterDataDelta"] = commandCenterDataDelta;
            StaticData.BrokenIsleData["netherDisruptorDataDelta"] = netherDisruptorDataDelta;
            StaticData.BrokenIsleData["wowToken"] = wowToken;
            StaticData.BrokenIsleData["lastUpdateElapsed"] = (updateTimeStamp - lastUpdate);
            StaticData.BrokenIsleData["lastUpdate"] = updateTimeStamp;
            StaticData.UpdateDynamicData();
            JsonHelper.SerializeToFile(StaticData.BrokenIsleDataFile, StaticData.BrokenIsleData);
            return Json(new { Error = 0 });
        }
        public ActionResult updateCD()
        {
            var key = Request["key"];
            if (string.IsNullOrEmpty(key) || key != "wowdatakey")
            {
                return Json(new { Error = 1 });
            }
            var data = Request["data"];
            if (string.IsNullOrEmpty(data)) return Json(new { Error = 2 });

            string faction = Request["faction"];
            int time = int.Parse(Request["lastUpdate"]);

            var names = data.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            if(faction == "horde")
            {
                StaticData.HordeCDAvailable.Clear();
                StaticData.HordeCDAvailable.AddRange(names);
                StaticData.HordeCDLastUpdate = Time.TimeStampToDateTime(time);
            }
            return Json(new { Error = 0 });
        }
    }
}