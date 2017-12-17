using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using WowCNData.Helper;

namespace WowCNData.Controllers
{
    public class serviceController : Controller
    {
        // SimCraft
        public static int SimId = 0;
        // TOOD:need a lock?
        public ActionResult simcraft()
        {
            try
            {
                SimId++;
                var operation = Request["operation"];
                var content = Request["content"];
                var quickSimulation = true;
                var enableScaling = false;
                var enableBloodlust = true;
                try
                {
                    enableScaling = Request["enableScaling"] == "true";
                    enableBloodlust = Request["enableBloodlust"] == "true";
                }
                catch
                {

                }
                Session["simcCompleted"] = false;
                Session["simcProgress"] = "";
                if (string.IsNullOrEmpty(operation)) return Json(new { error = 1, message = "请输入操作选项" });
                if (string.IsNullOrEmpty(content)) return Json(new { error = 1, message = "请输入模拟内容" });
                if (operation == "start")
                {
                    if (SimCraft.CurrentThread >= SimCraft.MaxThreads)
                    {
                        return Json(new { error = 1, message = "当前已达到服务器同时运行SimCraft的数量，请稍后模拟(最近一个请求已完成:" + SimCraft.CurrentPercent + "%)" });
                    }
                    var cooldown = Convert.ToInt32((SimCraft.LastReceiveResult.AddSeconds(5) - DateTime.Now).TotalSeconds);
                    if(cooldown > 0)
                    {
                        return Json(new { error = 1, message = "为防止对下一位玩家模拟造成影响，请在" + cooldown + "秒后再试" });
                    }
                    SimCraft.LastReceiveResult = DateTime.Now; // just prevent some bugs
                    if (content.Length < 50) return Json(new { error = 1, message = "模拟输入有误" });
                    var fileName = Server.MapPath("\\UploadData\\SimCraft\\" + SimId + ".simc");
                    if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
                    System.IO.File.AppendAllText(fileName, content, Encoding.UTF8);
                    var session = Session;
                    SimCraft.StartSimulation(fileName, enableScaling, quickSimulation, enableBloodlust,(string context, bool result) =>
                      {
                          session["simcCompleted"] = result;
                          session["simcProgress"] = context;
                      });
                    return Json(new { error = 0, message = "SimCraft已开始模拟" });
                }
                return View();
            }
            catch
            {
                return Json(new { error = 1, message = "操作发生异常(4)" });
            }
        }
        public ActionResult progress()
        {
            if (Session["simcCompleted"] != null)
            {
                if (!(bool)Session["simcCompleted"])
                {
                    return Json(new { error = 0, completed = Session["simcCompleted"], message = Session["simcProgress"] });
                }
                else
                {
                    return Json(new { error = 0, completed = Session["simcCompleted"], report = Session["simcProgress"] });
                }
            }
            return Json(new { error = 1, message = "未找到模拟请求" });
        }
        // TodayBrokenIsle
        [HttpPost]
        public ActionResult brokenIsle()
        {
            var buildings = BrokenIsle.GetBuildings();
            return Json(new
            {
                MageTower = new
                {
                    State = buildings[0].State,
                    Value = buildings[0].Value,
                    Percentage = buildings[0].Percent,
                    Buff = buildings[0].BuffName,
                    BuffDescription = buildings[0].BuffDesc,
                    Prediction = buildings[0].Prediction,
                    StateText = buildings[0].StateText
                },
                CommandCenter = new
                {
                    State = buildings[1].State,
                    Value = buildings[1].Value,
                    Percentage = buildings[1].Percent,
                    Buff = buildings[1].BuffName,
                    BuffDescription = buildings[1].BuffDesc,
                    Prediction = buildings[1].Prediction,
                    StateText = buildings[1].StateText
                },
                NetherDisruptor = new
                {
                    State = buildings[2].State,
                    Value = buildings[2].Value,
                    Percentage = buildings[2].Percent,
                    Buff = buildings[2].BuffName,
                    BuffDescription = buildings[2].BuffDesc,
                    Prediction = buildings[2].Prediction,
                    StateText = buildings[2].StateText
                }
            });
        }
        [HttpPost]
        public ActionResult legionAssaults()
        {
            var list = Helper.LegionAssault.GetRecentAssaluts(3);
            return Json(new
            {
                Assult1 = new
                {
                    Area = list[0].Area,
                    Started = list[0].Started,
                    StartDateTime = list[0].StartDateTime,
                    EndOrStartTimeSpan = list[0].EndOrStartTimeSpan
                },
                Assult2 = new
                {
                    Area = list[1].Area,
                    Started = list[1].Started,
                    StartDateTime = list[1].StartDateTime,
                    EndOrStartTimeSpan = list[1].EndOrStartTimeSpan
                },
                Assult3 = new
                {
                    Area = list[2].Area,
                    Started = list[2].Started,
                    StartDateTime = list[2].StartDateTime,
                    EndOrStartTimeSpan = list[2].EndOrStartTimeSpan
                }
            });

        }
        [HttpPost]
        public ActionResult mythicAffixes()
        {
            var w1 = MythicAffixes.GetWeekAffixes(0);
            var w2 = MythicAffixes.GetWeekAffixes(1);
            var w3 = MythicAffixes.GetWeekAffixes(2);
            return Json(new
            {
                Week1 = new
                {
                    Affix1 = new
                    {
                        LocaleName = w1[0].LName,
                        OriginalName = w1[0].OName
                    },
                    Affix2 = new
                    {
                        LocaleName = w1[1].LName,
                        OriginalName = w1[1].OName
                    },
                    Affix3 = new
                    {
                        LocaleName = w1[2].LName,
                        OriginalName = w1[2].OName
                    }
                },
                Week2 = new
                {
                    Affix1 = new
                    {
                        LocaleName = w2[0].LName,
                        OriginalName = w2[0].OName
                    },
                    Affix2 = new
                    {
                        LocaleName = w2[1].LName,
                        OriginalName = w2[1].OName
                    },
                    Affix3 = new
                    {
                        LocaleName = w2[2].LName,
                        OriginalName = w2[2].OName
                    }
                },
                Week3 = new
                {
                    Affix1 = new
                    {
                        LocaleName = w3[0].LName,
                        OriginalName = w3[0].OName
                    },
                    Affix2 = new
                    {
                        LocaleName = w3[1].LName,
                        OriginalName = w3[1].OName
                    },
                    Affix3 = new
                    {
                        LocaleName = w3[2].LName,
                        OriginalName = w3[2].OName
                    }
                },
            });
        }
        [HttpPost]
        public ActionResult coreInfo()
        {
            var lastUpdate = CoreInfo.GetLastUpdate();
            var message = Hot.GetHotNews();

            return Json(new
            {
                LastUpdate = lastUpdate,
                WowToken = CoreInfo.GetWowToken(),
                LastUpdateString = Time.TimeStampToDateTime(lastUpdate).ToString("MM-dd HH:mm"),
                Hot1= message[0],
                Hot2= message[1],
                Hot3= message[2],
            }
            );
        }
    }
}