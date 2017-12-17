using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WowCNData.Controllers
{
    public class blackmarketController : Controller
    {
        // GET: blackmarket
        public ActionResult Index()
        {
            if (Request.HttpMethod == "GET")
            {
                return View();
            }
            else
            {
                if(DateTime.Now.Hour == 23)
                {
                    var minute = DateTime.Now.Minute;
                    if(minute >= 10 || minute <= 50)
                    {
                        return View();
                    }
                }
                List<string> hashList = new List<string>();
                var query = Request["name"];
                if(string.IsNullOrEmpty(query)) return View();
                Helper.BlackMarket.RequestUpdate();
                var data = Helper.BlackMarket.BMDatas;
                var filteredData = new List<Helper.BlackMarket.BMData>();
                try
                {
                    foreach(var item in data)
                    {
                        if(item.Name.Contains(query) || item.Realm.Contains(query))
                        {
                            var hash = item.Name + item.Realm;
                            if (hashList.Contains(hash)) continue;
                            // change some thing
                            var spilt = item.Realm.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            spilt = spilt.OrderBy(t=>t).ToList();
                            var realm = "";
                            spilt.ForEach(t => realm += t + "/");
                            Helper.BlackMarket.BMData newItem = new Helper.BlackMarket.BMData()
                            {
                                Name = item.Name,
                                Realm = realm.TrimEnd('/'),
                                TimeLeft = item.TimeLeft
                            };
                            filteredData.Add(newItem);
                            hashList.Add(hash); // should do once ..lazy..
                        }
                    }
                    filteredData = filteredData.OrderBy(x => x.Name).ToList();
                    ViewData["Data"] = filteredData;
                    ViewData["Name"] = query;
                    return View();
                }
                catch
                {
                    return View();
                }
            }
        }
    }
}