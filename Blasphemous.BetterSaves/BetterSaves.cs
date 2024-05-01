using Blasphemous.BetterSaves.Naming;
using Blasphemous.BetterSaves.Slots;
using Blasphemous.ModdingAPI;

namespace Blasphemous.BetterSaves;

public class BetterSaves : BlasMod
{
    public BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public NameHandler NameHandler { get; private set; }
    public SlotHandler SlotHandler { get; private set; }

    protected override void OnInitialize()
    {
        Config cfg = ConfigHandler.Load<Config>();

        NameHandler = new NameHandler();
        SlotHandler = new SlotHandler(System.Math.Max(cfg.totalSlots, 3) / 3 - 1);
    }

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
