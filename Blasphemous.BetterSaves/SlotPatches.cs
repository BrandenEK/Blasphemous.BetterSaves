using Blasphemous.ModdingAPI.Input;
using Framework.Managers;
using Gameplay.UI.Others;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            Main.BetterSaves.LogWarning("Moving left to " + (__instance.SelectedSlot - 3));
            Main.BetterSaves.CurrentScreen--;

            RefreshSlots(___slots);

            //__instance.OnSelectedSlots(__instance.SelectedSlot - 3);
            //EventSystem.current.SetSelectedGameObject(___slots[__instance.SelectedSlot - 3].gameObject, null);
        }
        if (Main.BetterSaves.CurrentScreen < 3 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryRight))
        {
            Main.BetterSaves.LogWarning("Moving right to " + (__instance.SelectedSlot + 3));
            Main.BetterSaves.CurrentScreen++;

            RefreshSlots(___slots);
            //__instance.OnSelectedSlots(__instance.SelectedSlot + 3);
            //EventSystem.current.SetSelectedGameObject(___slots[__instance.SelectedSlot + 3].gameObject, null);
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
        var focuses = Object.FindObjectsOfType<KeepFocus>();
        foreach (KeepFocus keepFocus in focuses)
        {
            Main.BetterSaves.LogWarning(keepFocus.name);
            Main.BetterSaves.LogError(keepFocus.transform.parent?.name);
        }

        // Create extra slots if they dont already exist
        if (___slots.Count <= 3)
            AddSlots(__instance, ___slots);
    }

    private static void AddSlots(SelectSaveSlots selector, List<SaveSlot> slots)
    {
        // Make more copies of the buttons
        for (int i = 0; i < 3; i++)
        {
            GameObject slot1 = Object.Instantiate(slots[0].gameObject, slots[0].transform.parent);
            GameObject slot2 = Object.Instantiate(slots[1].gameObject, slots[1].transform.parent);
            GameObject slot3 = Object.Instantiate(slots[2].gameObject, slots[2].transform.parent);

            slot1.name = $"slot{i * 3 + 3}";
            slot2.name = $"slot{i * 3 + 4}";
            slot3.name = $"slot{i * 3 + 5}";

            EventsButton button1 = slot1.GetComponent<EventsButton>();
            EventsButton button2 = slot2.GetComponent<EventsButton>();
            EventsButton button3 = slot3.GetComponent<EventsButton>();

            Navigation nav1 = button1.navigation;
            nav1.selectOnDown = button2;
            button1.navigation = nav1;

            Navigation nav2 = button2.navigation;
            nav2.selectOnUp = button1;
            nav2.selectOnDown = button3;
            button2.navigation = nav2;

            Navigation nav3 = button3.navigation;
            nav3.selectOnUp = button2;
            button3.navigation = nav3;

            button1.onSelected.RemoveAllListeners();
            button2.onSelected.RemoveAllListeners();
            button3.onSelected.RemoveAllListeners();

            button1.onSelected.AddListener(() => selector.OnSelectedSlots(3));
            button2.onSelected.AddListener(() => selector.OnSelectedSlots(4));
            button3.onSelected.AddListener(() => selector.OnSelectedSlots(5));

            button1.onClick.RemoveAllListeners();
            button2.onClick.RemoveAllListeners();
            button3.onClick.RemoveAllListeners();

            button1.onClick.AddListener(() => selector.OnAcceptSlots(i * 3 + 0));
            button2.onClick.AddListener(() => selector.OnAcceptSlots(i * 3 + 1));
            button3.onClick.AddListener(() => selector.OnAcceptSlots(i * 3 + 2));

            slots.Add(slot1.GetComponent<SaveSlot>());
            slots.Add(slot2.GetComponent<SaveSlot>());
            slots.Add(slot3.GetComponent<SaveSlot>());

            t2.FocusObjects.Add(slot1);
            t2.FocusObjects.Add(slot2);
            t2.FocusObjects.Add(slot3);
        }

        Main.BetterSaves.LogError("Added more save slots");
    }
}

[HarmonyPatch(typeof(KeepFocus), "Awake")]
class t2
{
    public static void Prefix(KeepFocus __instance, List<GameObject> ___allowedObjects)
    {
        if (__instance.name == "UI_SLOT")
        {
            foreach (var obj in ___allowedObjects)
            {
                Main.BetterSaves.Log(obj.name);
            }
            FocusObjects = ___allowedObjects;
        }
    }

    public static List<GameObject> FocusObjects { get; private set; }
}