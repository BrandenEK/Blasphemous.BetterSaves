using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;

namespace Blasphemous.BetterSaves;

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.OnSelectedSlots))]
class Slot_Select_Patch
{
    public static void Prefix(ref int idxSlot)
    {
        //Main.BetterSaves.LogError($"Select slot {idxSlot} to {idxSlot + 3 * Main.BetterSaves.CurrentMultiplier}");
        //idxSlot += 3 * Main.BetterSaves.CurrentMultiplier;
    }
}

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.OnAcceptSlots))]
class Slot_Accept_Patch
{
    public static void Prefix(ref int idxSlot)
    {
        //Main.BetterSaves.LogError($"Accept slot {idxSlot} to {idxSlot + 3 * Main.BetterSaves.CurrentMultiplier}");
        //idxSlot += 3 * Main.BetterSaves.CurrentMultiplier;
    }
}
[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.GetSlotData))]
class PM_GetSlotData_Patch
{
    public static void Prefix(ref int slot) => slot = Main.BetterSaves.GetRealSlot(slot);
}

[HarmonyPatch(typeof(SaveSlot), nameof(SaveSlot.SetNumber))]
class SaveSlot_Number_Patch
{
    public static void Prefix(ref int slot) => slot = Main.BetterSaves.GetRealSlot(slot);
}

//[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.SaveGame))]
//class PM_Save_Patch { }