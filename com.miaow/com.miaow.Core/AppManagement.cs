using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Cache;

namespace com.miaow.Core
{
    public static class AppManagement
    {
        public static ICache Cache { get; }

        static AppManagement()
        {
            Cache = new LocalCache();
        }
    }
}
