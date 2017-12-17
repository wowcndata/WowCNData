using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WowCNData.Controllers
{
    public class extraController : Controller
    {
        // GET: extra
        public ActionResult Index()
        {
            var mgs = Helper.Menagerie.GetNextThreePets();
            ViewData["Menagerie"] = "今天:" + mgs[0] + ",明天:" + mgs[1] + ",后天:" + mgs[2] + ".";
            mgs = Helper.Menagerie.GetFisherFriends();
            ViewData["FisherFriends"] = "今天:" + mgs[0] + ",明天:" + mgs[1] + ",后天:" + mgs[2] + ".";
            return View();
        }
    }
}