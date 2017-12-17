using System;
using System.Security.Cryptography;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WowCNData.Helper
{
    internal static class Utility
    {
        internal static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = true, string custom = "")
        {
            byte[] b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        internal static string GetRandomHexString(int len)
        {
            string s = "abcdef";
            string reValue = string.Empty;
            Random rnd = new Random(GetNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue.ToLower();
        }
        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }
        internal static bool DownloadBin(string httpUrl, out byte[] data)
        {
            try
            {
                data = new WebClient().DownloadData(httpUrl);
                return true;
            }
            catch
            {
                data = new byte[] { 0 };
                return false;
            }
        }
        internal static bool DownloadFile(string httpUrl, string fileDest)
        {
            try
            {
                new WebClient().DownloadFile(httpUrl, fileDest);
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static String GetData(String getUrl, Encoding dataEncoding)
        {
            String ret = String.Empty;
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(getUrl));
                webReq.Method = "GET";
                webReq.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return ret;
            }
            catch
            {
            }
            return "";
        }
        internal static String PostData(String postUrl, String postData, Encoding dataEncoding)
        {
            String ret = String.Empty;
            try
            {
                byte[] byteArray = dataEncoding.GetBytes(postData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
                return ret;
            }
            catch
            {
            }
            return "";
        }
        internal static byte[] HexToBins(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
    internal class GZip
    {
        internal static bool Compress(string filename, string toFile)
        {
            try
            {
                byte[] buffer = new byte[0x1000];
                if (!File.Exists(filename))
                {
                    return false;
                }
                using (FileStream stream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream stream2 = File.Create(toFile))
                    {
                        using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Compress))
                        {
                            int num;
                            while ((num = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                stream3.Write(buffer, 0, num);
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static bool DecompressBin(byte[] toDecompress, out byte[] bin)
        {
            try
            {
                using (MemoryStream streamReturn = new MemoryStream())
                {
                    using (MemoryStream stream = new MemoryStream(toDecompress))
                    {
                        using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            stream3.CopyTo(streamReturn);
                            bin = streamReturn.ToArray();
                            return true;
                        }
                    }
                }
            }
            catch
            {
                bin = new byte[1];
                return false;
            }
        }
        internal static bool Decompress(string filename)
        {
            try
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    string str = filename;
                    string path = str.Remove(str.Length - 3);
                    using (FileStream stream2 = File.Create(path))
                    {
                        using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            stream3.CopyTo(stream2);
                            return File.Exists(path);
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private static byte[] m_Decoder = null;
        internal static byte[] EncryptRC4(byte[] data)
        {
            if (m_Decoder == null)
            {
                m_Decoder = new byte[] { 1, 2, 3, 4 };
            }
            byte[] encrypted = RC4Encrypt(m_Decoder, data);
            return encrypted;
        }
        internal static byte[] DecryptRC4(byte[] data)
        {
            if (m_Decoder == null)
            {
                m_Decoder = new byte[] { 1, 2, 3, 4 };
            }
            byte[] encrypted = RC4Encrypt(m_Decoder, data);
            return encrypted;
        }
        internal static byte[] RC4Encrypt(byte[] pwd, byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }
        internal static byte[] RC4Decrypt(byte[] pwd, byte[] data)
        {
            return RC4Encrypt(pwd, data);
        }
    }
    class SString
    {
        internal static string GetLeft(string str, string s)
        {
            string temp = str.Substring(0, str.IndexOf(s));
            return temp;
        }
        internal static string GetRight(string str, string s)
        {
            string temp = str.Substring(str.IndexOf(s), str.Length - str.Substring(0, str.IndexOf(s)).Length);
            return temp;
        }
        internal static string Between(string str, string leftstr, string rightstr)
        {
            try
            {
                int lpos = str.IndexOf(leftstr);
                int rpos = str.IndexOf(rightstr);
                if (lpos < 0 || rpos < 0) return "";
                int i = lpos + leftstr.Length;
                string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
                return temp;
            }
            catch
            {
                return "";
            }
        }
        internal static List<string> BetweenArray(string str, string leftstr, string rightstr)
        {
            try
            {
                List<string> list = new List<string>();
                int leftIndex = str.IndexOf(leftstr);
                int leftlength = leftstr.Length;
                int rightIndex = 0;
                string temp = "";
                while (leftIndex != -1)
                {
                    rightIndex = str.IndexOf(rightstr, leftIndex + leftlength);
                    if (rightIndex == -1)
                    {
                        break;
                    }
                    temp = str.Substring(leftIndex + leftlength, rightIndex - leftIndex - leftlength);
                    list.Add(temp);
                    leftIndex = str.IndexOf(leftstr, rightIndex + 1);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
    class Time
    {
        internal static int GetCurrentTimeStamp()
        {
            return (int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        internal static DateTime TimeStampToDateTime(int timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dateTimeStart.AddSeconds(timeStamp);
        }
        internal static string FormatTimeSpan(TimeSpan ts)
        {
            string ret = "";
            if (ts.Days > 0)
            {
                ret += ts.Days + "天";
            }
            if (ts.Hours > 0)
            {
                ret += ts.Hours + "时";
            }
            if (ts.Minutes > 0)
            {
                ret += ts.Minutes + "分";
            }
            return ret;
        }
    }
}