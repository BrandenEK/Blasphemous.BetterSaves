using Blasphemous.BetterSaves.Corruption;
using Blasphemous.BetterSaves.Naming;
using Blasphemous.BetterSaves.Slots;
using Blasphemous.CheatConsole;
using Blasphemous.Framework.Menus;
using Blasphemous.ModdingAPI;

namespace Blasphemous.BetterSaves;

/// <summary>
/// Increases available save slots and allows naming them
/// </summary>
public class BetterSaves : BlasMod
{
    internal BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public CorruptHandler CorruptHandler { get; private set; }
    public NameHandler NameHandler { get; private set; }
    public SlotHandler SlotHandler { get; private set; }

    /// <summary>
    /// Loads config and creates handlers
    /// </summary>
    protected override void OnInitialize()
    {
        Config cfg = ConfigHandler.Load<Config>();
        LocalizationHandler.RegisterDefaultLanguage("en");

        CorruptHandler = new CorruptHandler();
        NameHandler = new NameHandler();
        SlotHandler = new SlotHandler(System.Math.Max(cfg.totalSlots, 3) / 3 - 1);
    }

    /// <summary>
    /// Process handlers
    /// </summary>
    protected override void OnNewGame()
    {
        CorruptHandler.StoreModInfo();
    }

    /// <summary>
    /// Process handlers
    /// </summary>
    protected override void OnLoadGame()
    {
        CorruptHandler.StoreModInfo();
    }

    /// <summary>
    /// Process handlers
    /// </summary>
    protected override void OnUpdate()
    {
        SlotHandler.UpdateSlots();
    }

    /// <summary>
    /// Register slots command
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterCommand(new NameCommand());
        provider.RegisterNewGameMenu(new NameMenu());
    }
}
