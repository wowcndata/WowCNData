using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class RedisHelper : IDataProvider
    {
        private RedisClient m_Client;
        public RedisHelper()
        {
            m_Client = new RedisClient("localhost", 6379);
            string[] columns = new string[] {"mageTowerState","mageTowerData", "mageTowerBuff", "commandCenterState", "commandCenterData", "commandCenterBuff",
                "netherDisruptorState","netherDisruptorData","netherDisruptorBuff","mageTowerDataDelta","commandCenterDataDelta","netherDisruptorDataDelta",
                "lastUpdate","wowToken","mageTowerChangeTime","commandCenterChangeTime","netherDisruptorChangeTime","lastUpdateElapsed"};
            int[] defaultValues = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < columns.Length; i++)
            {
                if (m_Client.Exists(columns[i]) <= 0)
                {
                    m_Client.Set(columns[i], defaultValues[i]);
                    Trace.WriteLine("FORCE CREATE");
                }
            }
            var t = Get<int>("lastUpdate");
            Trace.WriteLine("CREATED");
        }
        public RedisClient GetClient()
        {
            return m_Client;
        }
        public bool Set<T>(string key,T value)
        {
            return m_Client.Set(key, value);
        }
        public T Get<T>(string key)
        {
            return m_Client.Get<T>(key);
        }
        public bool Add<T>(string key,T value)
        {
            return m_Client.Add(key, value);
        }
    }
}