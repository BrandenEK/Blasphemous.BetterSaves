using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;

namespace Blasphemous.BetterSaves.Corruption;

/// <summary>
/// Prevent input on slots menu if the confirmation box is active
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), "Update")]
class SaveSlots_Update_Patch
{
    public static bool Prefix() => !Main.BetterSaves.CorruptHandler.IsShowingConfirmation;
}

/// <summary>
/// Possibly open save confirmation box when loading a save file
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.OnAcceptSlots))]
class SaveSlots_Accept_Patch
{
    [HarmonyPriority(Priority.First)]
    public static bool Prefix(ref int idxSlot) => !Main.BetterSaves.CorruptHandler.ShouldDisplayBox(idxSlot);
}