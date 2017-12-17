using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WowCNData.Helper;
using WowCNData.Models;

namespace WowCNData.Controllers
{
    public class navbarController : Controller
    {
        // GET: navbar
        public ActionResult get()
        {
            var model = new NavBarModel();
            model.Brand = "魔兽数据中文站";
            model.SubItems = GenerateSubItems(HttpContext);
            return View(model);
        }

        private static List<LinkModel> GenerateSubItems(HttpContextBase context)
        {
            List<LinkModel> result = new List<LinkModel>();
            var curPage = context.Request.RawUrl;
            foreach (var url in StaticData.NavBarData.Keys)
            {
                LinkModel model = new LinkModel();
                model.Text = StaticData.NavBarData[url];
                model.Url = url;
                model.Active = "";
                model.Enabled = "";
                if (url.Contains("appAddons"))
                {
                    model.Enabled = "disabled";
                    model.Url = "";
                }
                if (curPage == url) model.Active = "active";
                result.Add(model);
            }
            return result;
        }
    }
}