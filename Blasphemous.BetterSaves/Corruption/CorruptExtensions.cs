using System;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.BetterSaves.Corruption;

internal static class CorruptExtensions
{
    public static string FormatList(this IEnumerable<SerializedModInfo> list, bool includeVersion)
    {
        return string.Join(", ", list.Select(x => $"{x.Name}{(includeVersion ? $" v{x.Version}" : "")}").ToArray());
    }

    public static bool TryGetItem<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T item)
    {
        foreach (T t in list)
        {
            if (!predicate(t))
                continue;

            item = t;
            return true;
        }

        item = default;
        return false;
    }
}
