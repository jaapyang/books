using System.Collections.Generic;
using System.Linq;

namespace com.miaow.Core.Cache
{
    public class LocalCache : ICache
    {
        private readonly Dictionary<string, object> _cacheDictionary;

        public LocalCache()
        {
            _cacheDictionary = new Dictionary<string, object>();
        }

        public T Get<T>(string cacheKey)
        {
            if (_cacheDictionary.All(x => x.Key != cacheKey)) return default(T);
            return (T)_cacheDictionary[cacheKey];
        }

        public void Set<T>(string cacheKey, T obj)
        {
            if (_cacheDictionary.All(x => x.Key != cacheKey))
            {
                _cacheDictionary[cacheKey] = obj;
                return;
            }

            _cacheDictionary.Add(cacheKey, obj);
        }

        public void Remove(string cacheKey)
        {
            _cacheDictionary.Remove(cacheKey);
        }
    }
}