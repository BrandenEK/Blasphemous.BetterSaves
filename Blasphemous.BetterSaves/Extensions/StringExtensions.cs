using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.BetterSaves.Extensions;

internal static class StringExtensions
{
    public static string FormatList(this IEnumerable<SerializedModInfo> list, bool includeVersion)
    {
        return string.Join(", ", list.Select(x => $"{x.Name}{(includeVersion ? $" v{x.Version}" : "")}").ToArray());
    }
}
