using Blasphemous.ModdingAPI;
using Framework.Managers;
using System;

namespace Blasphemous.BetterSaves;

public class BetterSaves : BlasMod
{
    public BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        LogError($"{ModInfo.MOD_NAME} has been initialized");
    }

    protected override void OnNewGame()
    {
        string name = DateTime.Now.ToString();
        SetSaveName(name);
    }

    public void SetSaveName(string name)
    {
        Core.Events.SetFlag("NAME_" + name, true, true);
    }
}
