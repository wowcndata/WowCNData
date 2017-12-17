using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowCNData.Helper
{
    public interface IDataProvider
    {
        bool Set<T>(string key, T value);
        bool Add<T>(string key, T value);
        T Get<T>(string key);
    }
}