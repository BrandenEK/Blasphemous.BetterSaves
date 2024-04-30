using Blasphemous.ModdingAPI.Input;
using Framework.Managers;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.BetterSaves;

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.OnSelectedSlots))]
class Slot_Select_Patch
{
    public static void Prefix(ref int idxSlot)
    {
        Main.BetterSaves.LogError($"Select slot {idxSlot}");
    }
}

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.OnAcceptSlots))]
class Slot_Accept_Patch
{
    public static void Prefix(ref int idxSlot)
    {
        Main.BetterSaves.LogError($"Accept slot {idxSlot}");
    }
}

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class Slot_SetData_Patch
{
    public static void Prefix()
    {
        Main.BetterSaves.LogError($"Clear slots");
    }
}

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.SetAllData))]
class Slot_Clear_Patchxxxxxxxxxxxxxx
{
    public static void Postfix(SelectSaveSlots __instance, List<SaveSlot> ___slots)
    {
        Main.BetterSaves.LogError($"Setting all data");
        // temp

        t.RefreshSlots(___slots);
    }
}

//[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.GetSlotData))]
//class PM_GetSlotData_Patch
//{
//    public static void Prefix(ref int slot) => slot = Main.BetterSaves.GetRealSlot(slot);
//}

//[HarmonyPatch(typeof(SaveSlot), nameof(SaveSlot.SetNumber))]
//class SaveSlot_Number_Patch
//{
//    public static void Prefix(ref int slot) => slot = Main.BetterSaves.GetRealSlot(slot);
//}

[HarmonyPatch(typeof(SelectSaveSlots), "Update")]
class t
{
    public static void Postfix(SelectSaveSlots __instance, List<SaveSlot> ___slots, List<EventsButton> ___AllSlots)
    {
        // If not on save slot menu, reset screen to zero
        if (!__instance.IsShowing)
        {
            Main.BetterSaves.CurrentScreen = 0;
            return;
        }

        if (Main.BetterSaves.CurrentScreen > 0 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryLeft))
        {
            Main.BetterSaves.LogWarning("Moving left");
            Main.BetterSaves.CurrentScreen--;

            RefreshSlots(___slots);

            __instance.OnSelectedSlots(__instance.SelectedSlot - 3);
        }
        if (Main.BetterSaves.CurrentScreen < 3 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryRight))
        {
            Main.BetterSaves.LogWarning("Moving right");
            Main.BetterSaves.CurrentScreen++;

            RefreshSlots(___slots);
            __instance.OnSelectedSlots(__instance.SelectedSlot + 3);
        }


        //if (!Input.GetKeyDown(KeyCode.Keypad9))
        //    return;

        //SaveSlot slot = ___slots[0];

        //Main.BetterSaves.LogWarning(slot.name);
        //foreach (Transform t in slot.transform)
        //{
        //    Main.BetterSaves.Log(t.name);
        //}
        //foreach (Component c in slot.gameObject.GetComponents<Component>())
        //{
        //    Main.BetterSaves.LogError(c.ToString());
        //}

        //foreach (var b in ___AllSlots)
        //{
        //    Main.BetterSaves.Log(b.name);
        //}
    }

    public static void RefreshSlots(List<SaveSlot> slots)
    {
        // Only enable ones on the current screen
        for (int i = 0; i < slots.Count; i += 3)
        {
            bool screenActive = i / 3 == Main.BetterSaves.CurrentScreen;
            slots[i + 0].gameObject.SetActive(screenActive);
            slots[i + 1].gameObject.SetActive(screenActive);
            slots[i + 2].gameObject.SetActive(screenActive);
        }
    }
}

//[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.SaveGame))]
//class PM_Save_Patch { }

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class Slot_Clear_Patch
{
    public static void Prefix(SelectSaveSlots __instance, List<SaveSlot> ___slots)
    {
        // Create extra slots if they dont already exist
        if (___slots.Count <= 3)
            AddSlots(___slots);
    }

    private static void AddSlots(List<SaveSlot> slots)
    {
        // Make more copies of the buttons
        for (int i = 0; i < 3; i++)
        {
            slots.Add(Object.Instantiate(slots[0].gameObject, slots[0].transform.parent).GetComponent<SaveSlot>());
            slots.Add(Object.Instantiate(slots[1].gameObject, slots[1].transform.parent).GetComponent<SaveSlot>());
            slots.Add(Object.Instantiate(slots[2].gameObject, slots[2].transform.parent).GetComponent<SaveSlot>());
        }

        Main.BetterSaves.LogError("Added more save slots");
    }
}