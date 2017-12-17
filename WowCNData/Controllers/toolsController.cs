using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WowCNData.Controllers
{
    public class toolsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult simcraft()
        {
            Session["View"] = DateTime.Now;
            return View();
        }
        public ActionResult dpsranking()
        {
            int percent = 75;
            if(Request["percentage"] != null)
            {
                try
                {
                    percent = int.Parse(Request["percentage"]);
                    if (percent != 75 && percent != 90 && percent != 99) percent = 75;
                }
                catch
                {
                    percent = 75;
                }
            }
            //data: ['巴西', '印尼', '美国', '印度', '中国']
            var data = Helper.WCL.GetData(percent);
            // process it
            StringBuilder yAxis = new StringBuilder(0x100);
            StringBuilder dataSeries = new StringBuilder(0x100);
            StringBuilder color = new StringBuilder(0x100);
            data.ForEach(t =>
            {
                yAxis.Append("'");
                yAxis.Append(t.Spec);
                yAxis.Append("',");

                dataSeries.Append(t.Score);
                dataSeries.Append(",");

                color.Append("'");
                color.Append(t.Color);
                color.Append("',");
            });
            ViewData["Spec"] = yAxis.ToString().TrimEnd(',');
            ViewData["Score"] = dataSeries.ToString().TrimEnd(',');
            ViewData["Color"] = color.ToString().TrimEnd(',');
            ViewData["Percent"] = percent;
            ViewData["Update"] = Helper.WCL.LastUpdate.ToString("MM-dd HH:mm");
            return View();
        }
    }
}