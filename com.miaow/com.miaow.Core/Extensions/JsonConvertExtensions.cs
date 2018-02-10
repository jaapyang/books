using Newtonsoft.Json;

namespace com.miaow.Core.Extensions
{
    public static class JsonConvertExtensions
    {
        public static string ToJson(this object obj)
        {
            if (obj is null) return string.Empty;
            return JsonConvert.SerializeObject(obj);
        }

        public static T To<T>(this string str)
            where T:class
        {
            return str.IsNullOrEmpty() ? null : JsonConvert.DeserializeObject<T>(str);
        }
    }
}