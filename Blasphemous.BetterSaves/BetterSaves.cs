using Blasphemous.BetterSaves.Naming;
using Blasphemous.BetterSaves.Slots;
using Blasphemous.ModdingAPI;

namespace Blasphemous.BetterSaves;

public class BetterSaves : BlasMod
{
    public BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public NameHandler NameHandler { get; } = new();
    public SlotHandler SlotHandler { get; } = new();

    protected override void OnNewGame()
    {
        string name = System.DateTime.Now.ToString();
        NameHandler.SetSaveName(name);
    }

    protected override void OnUpdate()
    {
        SlotHandler.UpdateSlots();
    }
}
