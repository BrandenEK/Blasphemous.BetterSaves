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

[HarmonyPatch(typeof(SelectSaveSlots), "Update")]
class t
{
    public static void Postfix(List<SaveSlot> ___slots, List<EventsButton> ___AllSlots)
    {
        if (!Input.GetKeyDown(KeyCode.Keypad9))
            return;

        SaveSlot slot = ___slots[0];

        Main.BetterSaves.LogWarning(slot.name);
        foreach (Transform t in slot.transform)
        {
            Main.BetterSaves.Log(t.name);
        }
        foreach (Component c in slot.gameObject.GetComponents<Component>())
        {
            Main.BetterSaves.LogError(c.ToString());
        }

        foreach (var b in ___AllSlots)
        {
            Main.BetterSaves.Log(b.name);
        }
    }
}

//[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.SaveGame))]
//class PM_Save_Patch { }

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class Slot_Clear_Patch
{
    public static void Prefix(List<SaveSlot> ___slots)
    {
        if (___slots.Count > 3)
            return;

        // Make more copies of the buttons
        for (int i = 0; i < 3; i++)
        {
            ___slots.Add(Object.Instantiate(___slots[0].gameObject, ___slots[0].transform.parent).GetComponent<SaveSlot>());
            ___slots.Add(Object.Instantiate(___slots[1].gameObject, ___slots[1].transform.parent).GetComponent<SaveSlot>());
            ___slots.Add(Object.Instantiate(___slots[2].gameObject, ___slots[2].transform.parent).GetComponent<SaveSlot>());
        }

        Main.BetterSaves.LogError("Added more save slots");
    }
}