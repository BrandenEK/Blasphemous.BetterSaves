using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.BetterSaves.Extensions;

internal static class StringExtensions
{
    public static string FormatList(this IEnumerable<string> list)
    {
        return string.Join(", ", list.ToArray());
}
}
