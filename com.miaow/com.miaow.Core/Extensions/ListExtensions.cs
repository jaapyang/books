using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace com.miaow.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || !list.Any();
        }
        
        public static bool IsNullOrEmpty<T>(this IQueryable<T> queryable)
        {
            return queryable == null || !queryable.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}