using System;
using System.Text;
using System.Threading.Tasks;

namespace com.miaow.Core.Cache
{
    public interface ICache
    {
        T Get<T>(string cacheKey);
        void Set<T>(string cacheKey,T obj);
        void Remove(string cacheKey);
    }
}
