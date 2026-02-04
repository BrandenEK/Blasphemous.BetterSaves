using Gameplay.UI.Others;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.BetterSaves.Slots;

/// <summary>
/// Refresh slots whenever menu is updated
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.SetAllData))]
class SelectSaveSlots_SetAllData_Patch
{
    public static void Postfix()
    {
        Main.BetterSaves.SlotHandler.RefreshSlots();
    }
}

/// <summary>
/// Not sure
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class SelectSaveSlots_Clear_Patch
{
    public static void Prefix(List<SaveSlot> ___slots)
    {
        Main.BetterSaves.SlotHandler.StoreSlotList(___slots);
        Main.BetterSaves.SlotHandler.CreateNewSlots();
    }
}

/// <summary>
/// Store the list of allowed focus objects
/// </summary>
[HarmonyPatch(typeof(KeepFocus), nameof(KeepFocus.Awake))]
class KeepFocus_Awake_Patch
{
    public static void Prefix(KeepFocus __instance, List<GameObject> ___allowedObjects)
    {
        if (__instance.name == "UI_SLOT")
        {
            Main.BetterSaves.SlotHandler.StoreFocusList(___allowedObjects);
        }
    }
}

/// <summary>
/// Always hide sacred sorrows button
/// </summary>
[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.IsAnySlotForBossRush))]
class NewMainMenu_IsAnySlotForBossRush_Patch
{
    public static void Postfix(ref bool __result) => __result = false;
}
