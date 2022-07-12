using System;
using System.Collections.Generic;

namespace Budgetter.Wpf.Framework.Extensions;

public static class EnumerableExtensions
{
    public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action(item);
    }
}