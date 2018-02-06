using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Core.Extensions.List
{
    public static class ListDictionaryExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> List)
        {
            return (List == null || List.Count < 1);
        }

        public static bool IsNullOrEmpty<T, U>(this IDictionary<T, U> Dictionary)
        {
            return (Dictionary == null || Dictionary.Count < 1);
        }
    }
}
