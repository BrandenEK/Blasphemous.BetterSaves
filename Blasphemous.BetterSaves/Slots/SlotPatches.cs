using Gameplay.UI.Others;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.BetterSaves.Slots;

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.SetAllData))]
class Slot_SetData_Patch
{
    public static void Postfix()
    {
        Main.BetterSaves.SlotHandler.RefreshSlots();
    }
}

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class Slot_Clear_Patch
{
    public static void Prefix(List<SaveSlot> ___slots)
    {
        Main.BetterSaves.SlotHandler.StoreSlotList(___slots);
        Main.BetterSaves.SlotHandler.CreateNewSlots();
    }
}

[HarmonyPatch(typeof(KeepFocus), "Awake")]
class Focus_Awake_Patch
{
    public static void Prefix(KeepFocus __instance, List<GameObject> ___allowedObjects)
    {
        if (__instance.name == "UI_SLOT")
        {
            Main.BetterSaves.SlotHandler.StoreFocusList(___allowedObjects);
        }
    }
}
