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
