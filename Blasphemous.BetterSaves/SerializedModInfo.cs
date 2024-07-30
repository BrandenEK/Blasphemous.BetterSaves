
namespace Blasphemous.BetterSaves;

internal class SerializedModInfo(string name, string version)
{
    public string Name { get; } = name;
    public string Version { get; } = version;
}
