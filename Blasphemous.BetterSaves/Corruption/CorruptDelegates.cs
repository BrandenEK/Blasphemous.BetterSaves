using System;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.BetterSaves.Corruption;

internal static class Invalidities
{
    public delegate IEnumerable<SerializedModInfo> InvalidDelegate(IEnumerable<SerializedModInfo> saved, IEnumerable<SerializedModInfo> current);

    public static InvalidDelegate GetMissing() => (saved, current) =>
    {
        return saved.Where(x => !current.Any(y => x.Name == y.Name));
    };

    public static InvalidDelegate GetAdded() => (saved, current) =>
    {
        return current.Where(x => !saved.Any(y => x.Name == y.Name));
    };

    public static InvalidDelegate GetUpdated() => (saved, current) =>
    {
        return saved.Where(x => current.TryGetItem(y => x.Name == y.Name, out var z) && new Version(x.Version) < new Version(z.Version));
    };

    public static InvalidDelegate GetDowngraded() => (saved, current) =>
    {
        return saved.Where(x => current.TryGetItem(y => x.Name == y.Name, out var z) && new Version(x.Version) > new Version(z.Version));
    };
}
