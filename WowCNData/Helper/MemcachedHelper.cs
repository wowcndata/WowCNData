using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public class MemcachedHelper : IDataProvider
    {
        public bool Set<T>(string key, T value)
        {
            return false;
        }
        public T Get<T>(string key)
        {
            return default(T);
        }
        public bool Add<T>(string key, T value)
        {
            return false;
        }
    }
}