using Blasphemous.BetterSaves.Naming;
using Blasphemous.BetterSaves.Slots;
using Blasphemous.ModdingAPI;

namespace Blasphemous.BetterSaves;

/// <summary>
/// Increases available save slots and allows naming them
/// </summary>
public class BetterSaves : BlasMod
{
    internal BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public NameHandler NameHandler { get; private set; }
    public SlotHandler SlotHandler { get; private set; }

    /// <summary>
    /// Loads config and creates handlers
    /// </summary>
    protected override void OnInitialize()
    {
        Config cfg = ConfigHandler.Load<Config>();

        NameHandler = new NameHandler();
        SlotHandler = new SlotHandler(System.Math.Max(cfg.totalSlots, 3) / 3 - 1);
    }

    /// <summary>
    /// Update handlers
    /// </summary>
    protected override void OnUpdate()
    {
        SlotHandler.UpdateSlots();
    }
}
