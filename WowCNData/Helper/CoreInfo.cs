using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class CoreInfo
    {
        public static int GetLastUpdate()
        {
            return StaticData.BrokenIsleData["lastUpdate"];
        }
        public static int GetWowToken()
        {
            return StaticData.BrokenIsleData["wowToken"];
        }
    }
}