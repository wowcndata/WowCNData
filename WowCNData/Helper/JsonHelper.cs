using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WowCNData.Helper
{
    public class JsonHelper
    {
        public static void SerializeToFile(string file, object source)
        {
            try
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
                File.AppendAllText(file, JsonConvert.SerializeObject(source, Formatting.Indented), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                File.AppendAllText(HttpContext.Current.Server.MapPath("") + "log.txt", ex.ToString() + Environment.NewLine, Encoding.UTF8);
            }
        }
        public static T DeserializeFromFile<T>(string file)
        {
            try
            {
                T result = default(T);
                if (!File.Exists(file)) return result;
                var data = File.ReadAllText(file, Encoding.UTF8);
                return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return default(T);
            }
        }
        public static T DeserializeFromString<T>(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(text, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return default(T);
            }
        }
    }
}