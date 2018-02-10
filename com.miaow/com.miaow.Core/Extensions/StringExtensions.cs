using System;
using System.Text;
using System.Threading.Tasks;

namespace com.miaow.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
