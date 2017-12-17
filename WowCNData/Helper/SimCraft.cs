using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Threading;

namespace WowCNData.Helper
{
    using Helper;
    using System.IO;
    using System.Text;

    public class SimCraft
    {
        public static int CurrentPercent;
        public static int MaxThreads = 1;
        public static int CurrentThread;
        public static DateTime LastReceiveResult = new DateTime(1970, 1, 1, 1, 1, 1);
        class ScalingInfo
        {
            public string Name;
            public float value;

            public ScalingInfo(string n, float v)
            {
                Name = n; value = v;
            }
        }
        class SimCraftRequest
        {
            public string FileName;
            public SimCraftOutputCallbackDelegate Callback;
            public bool EnableScaling;
            public bool QuickSimulation;
            public bool EnableBloodlust;
        }
        public delegate void SimCraftOutputCallbackDelegate(string context, bool end);
        public static void StartSimulation(string fileName, bool enableScale, bool quickSimc, bool enableBloodlust, SimCraftOutputCallbackDelegate outputCallback)
        {
            CurrentThread++;
            ThreadPool.QueueUserWorkItem(SimCraftWorkThread, new SimCraftRequest() { FileName = fileName, Callback = outputCallback, EnableScaling = enableScale, QuickSimulation = quickSimc, EnableBloodlust = enableBloodlust });
        }
        private static string PreProcessSimcFile(string file, bool enableScale = false, bool quickSimc = true,bool enableBloodlust = true)
        {
            quickSimc = true; // FORCE
            string playerName = "PlayerUnknown"; // TRICK
            string spec = "";
            string @class = "";
            StringBuilder sb = new StringBuilder(0x2000);
            foreach (var line in File.ReadAllLines(file))
            {
                if (line.Contains("calculate_scale_factors")) continue;
                if (line.Contains("target_error")) continue;
                if (line.Contains("iterations")) continue;
                if (line.Contains("raid_events")) continue;
                if (line.Contains("override")) continue;
                if (line.Contains("\""))
                {
                    playerName = SString.Between(line, "\"", "\"");
                    @class = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
                if (line.Contains("spec="))
                {
                    spec = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                }

                sb.AppendLine(line);
            }
            if (enableScale)
            {
                var extra = GetSpecAttrib(@class, spec);

                sb.AppendLine("calculate_scale_factors=1");
                sb.AppendLine("scale_only=crit,haste,mastery,vers," + extra);
            }
            if (quickSimc)
            {
                sb.AppendLine("target_error=0");
                sb.AppendLine("iterations=10000");
            }
            if (enableBloodlust) sb.AppendLine("override.bloodlust=1");
            else sb.AppendLine("override.bloodlust=0");

            File.Delete(file);
            File.AppendAllText(file, sb.ToString(), new UTF8Encoding(true));
            return playerName;
        }
        private static string GetSpecAttrib(string @class, string spec)
        {
            string tAttrib = "";
            if (StaticData.SpecAttrib.TryGetValue(spec, out tAttrib))
            {
                if (spec == "frost")
                {
                    if (@class == "法师") return "intellect";
                    else return "strength";
                }
                else return tAttrib;
            }
            return "";
        }
        private static void GeneratePlayerInfo(List<string> data,out string race,out int level,out string @class,out string spec,out string attrib)
        {
            race = "";
            level = 0;
            @class = "";
            spec = "";
            attrib = "";

            string tRace, tClass, tSpec, tAttrib;
            int val;
            foreach (var d in data)
            {
                if(int.TryParse(d,out val))
                {
                    level = val;
                }
                if (StaticData.RaceName.TryGetValue(d, out tRace))
                {
                    race = tRace;
                }
                if (StaticData.ClassName.TryGetValue(d,out tClass))
                {
                    @class = tClass;
                }
                if (StaticData.SpecName.TryGetValue(d, out tSpec))
                {
                    spec = tSpec;
                }
                if (StaticData.SpecAttrib.TryGetValue(d, out tAttrib))
                {
                    if (d == "frost")
                    {
                        if (tClass == "法师") attrib = "intellect";
                        else attrib = "strength";
                    }
                    else attrib = tAttrib;
                }
            }
        }
        private static void SimCraftWorkThread(object request)
        {
            var simRequest = (SimCraftRequest)request;
            string report = "";
            bool finishDPS = false;
            bool finishScaling = false;
            bool enableScaling = true;
            string playerName = PreProcessSimcFile(simRequest.FileName, simRequest.EnableScaling, simRequest.QuickSimulation, simRequest.EnableBloodlust);
            using (Process process = new Process())
            {
                try
                {
                    string attrib = "";
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.StandardInput.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "Executable\\simc\\simc.exe " + simRequest.FileName);
                    process.StandardInput.WriteLine("exit");
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var data = process.StandardOutput.ReadLine();
                        Trace.WriteLine(data);
                        if (data.StartsWith("Generating "))
                        {
                            if (data.Contains("Generating reports"))
                            {
                                continue;
                            }
                            var c = SString.Between(data, ": ", "[").Replace(" ", "");
                            var totalProgress = c.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            var currentSection = int.Parse(totalProgress[0]);
                            var totalSection = int.Parse(totalProgress[1]);

                            var t = SString.Between(data, "] ", " ").Replace(" ", "");
                            var progress = t.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            if (progress.Length == 2 && totalProgress.Length == 2)
                            {
                                var percent = int.Parse(progress[0]) * 100 / int.Parse(progress[1]);
                                var totalPercent = percent / totalSection + (currentSection - 1) * 100 / totalSection;
                                if (!enableScaling)
                                {
                                    simRequest.Callback("模拟进度:" + percent + "%", false);
                                }
                                else
                                {
                                    if (totalSection == 1)
                                    {
                                        simRequest.Callback("模拟进度-正在计算DPS:" + percent + "%", false);
                                    }
                                    else
                                    {
                                        if (currentSection == 1)
                                        {
                                            simRequest.Callback("模拟进度(1/2)-正在计算总体DPS:" + totalPercent + "%", false);
                                        }
                                        else simRequest.Callback("模拟进度(2/2)-正在模拟属性占比:" + totalPercent + "%", false);
                                    }
                                }
                                CurrentPercent = totalPercent;
                            }
                        }
                        else if (data.StartsWith("Player:"))
                        {
                            // process
                            var spilt = data.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            spilt[1] = playerName;
                            spilt[0] = "玩家:";
                            data = "";
                            string race, @class, spec;
                            int level;
                            GeneratePlayerInfo(spilt.ToList(), out race, out level, out @class, out spec, out attrib);
                            report += "玩家:" + playerName + " " + level + " " + race + " " + spec + @class + Environment.NewLine;
                            //spilt.ToList().ForEach(t => data += t + " ");
                            //report += data + Environment.NewLine;
                        }
                        else if (data.Contains("DPS-Range=") && !finishDPS)
                        {
                            report += "DPS模拟:" + SString.Between(data, ": ", "  ") + Environment.NewLine;
                            finishDPS = true;
                        }
                        else if (data.Contains("Weights") && !finishScaling)
                        {
                            finishScaling = true;
                            var crit = float.Parse(SString.Between(data, "Crit=", "("));
                            var haste = float.Parse(SString.Between(data, "Haste=", "("));
                            var mastery = float.Parse(SString.Between(data, "Mastery=", "("));
                            var vers = float.Parse(SString.Between(data, "Vers=", "("));
                            var extraData = 0f;
                            if (attrib == "strength")
                            {
                                extraData = float.Parse(SString.Between(data, "Str=", "("));
                            }
                            else if (attrib == "intellect")
                            {
                                extraData = float.Parse(SString.Between(data, "Int=", "("));
                            }
                            else if (attrib == "agility")
                            {
                                extraData = float.Parse(SString.Between(data, "Agi=", "("));
                            }
                            List<ScalingInfo> scalings = new List<ScalingInfo>();
                            scalings.Add(new ScalingInfo("暴击", crit));
                            scalings.Add(new ScalingInfo("急速", haste));
                            scalings.Add(new ScalingInfo("精通", mastery));
                            scalings.Add(new ScalingInfo("全能", vers));
                            scalings.Add(new ScalingInfo("Extra", extraData));
                            scalings.Sort((ScalingInfo s1, ScalingInfo s2) =>
                            {
                                if (s1.value > s2.value) return -1;
                                if (s1.value < s2.value) return 1;
                                return 0;
                            });
                            report += "属性权重:" + Environment.NewLine;
                            float max = scalings[0].value;
                            scalings.ForEach(t =>
                            {
                                t.value = (float)Math.Round(t.value / max, 2);
                                report += (t.Name == "Extra" ? StaticData.AttribName[attrib] : t.Name) + ":" + t.value.ToString() + Environment.NewLine;
                            });
                        }
                    }
                    process.WaitForExit();
                    simRequest.Callback(report, true);
                }
                catch
                {

                }
                finally
                {
                    CurrentThread--;
                    LastReceiveResult = DateTime.Now;
                    try { process.Kill(); } catch { }
                }
            }
        }
    }
}