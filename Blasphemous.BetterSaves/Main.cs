using BepInEx;

namespace Blasphemous.BetterSaves;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "3.0.0")]
[BepInDependency("Blasphemous.CheatConsole", "1.1.0")]
[BepInDependency("Blasphemous.Framework.Menus", "0.5.0")]
internal class Main : BaseUnityPlugin
{
    public static BetterSaves BetterSaves { get; private set; }

    private void Start()
    {
        BetterSaves = new BetterSaves();
    }
}
