
using System;
using System.Collections.Generic;

namespace FCHA.WPFHelpers
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> p)
        {
            if (null == sequence)
                return;
            foreach (T item in sequence)
                p(item);
        }
    }
}