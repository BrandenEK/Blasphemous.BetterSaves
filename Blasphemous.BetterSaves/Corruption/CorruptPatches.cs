using Blasphemous.Framework.Menus;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;

namespace Blasphemous.BetterSaves.Corruption;

/// <summary>
/// Prevent input on slots menu if the confirmation box is active
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Update))]
class SelectSaveSlots_Update_Patch
{
    public static bool Prefix() => !Main.BetterSaves.CorruptHandler.IsShowingConfirmation;
}

/// <summary>
/// Possibly open save confirmation box when loading a save file
/// </summary>
[HarmonyPatch(typeof(MenuFramework), "TryStartGame")]
class MenuFramework_TryStartGame_Patch
{
    public static bool Prefix(int slot, bool isContinue) => !isContinue || !Main.BetterSaves.CorruptHandler.ShouldDisplayBox(slot);
}