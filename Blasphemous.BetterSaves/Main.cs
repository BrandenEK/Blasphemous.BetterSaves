using BepInEx;

namespace Blasphemous.BetterSaves;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.1.2")]
public class Main : BaseUnityPlugin
{
    public static BetterSaves BetterSaves { get; private set; }

    private void Start()
    {
        BetterSaves = new BetterSaves();
    }
}
