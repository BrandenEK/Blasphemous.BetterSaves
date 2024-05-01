using Blasphemous.ModdingAPI.Input;
using Gameplay.UI.Others;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves.Slots;

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
        t.RefreshSlots(___slots);
    }
}

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
            EventSystem.current.SetSelectedGameObject(___slots[__instance.SelectedSlot - 3].gameObject, null);
        }
        if (Main.BetterSaves.CurrentScreen < 3 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryRight))
        {
            Main.BetterSaves.LogWarning("Moving right to " + (__instance.SelectedSlot + 3));
            Main.BetterSaves.CurrentScreen++;

            RefreshSlots(___slots);
            EventSystem.current.SetSelectedGameObject(___slots[__instance.SelectedSlot + 3].gameObject, null);
        }
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

[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.Clear))]
class Slot_Clear_Patch
{
    public static void Prefix(SelectSaveSlots __instance, List<SaveSlot> ___slots)
    {
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

            AddEvents(button1, i * 3 + 3, selector);
            AddEvents(button2, i * 3 + 4, selector);
            AddEvents(button3, i * 3 + 5, selector);

            slots.Add(slot1.GetComponent<SaveSlot>());
            slots.Add(slot2.GetComponent<SaveSlot>());
            slots.Add(slot3.GetComponent<SaveSlot>());

            t2.FocusObjects.Add(slot1);
            t2.FocusObjects.Add(slot2);
            t2.FocusObjects.Add(slot3);
        }

        Main.BetterSaves.LogError("Added more save slots");
    }

    private static void AddEvents(EventsButton button, int idx, SelectSaveSlots selector)
    {
        button.onSelected.RemoveAllListeners();
        button.onSelected = new EventsButton.ButtonSelectedEvent();
        button.onSelected.AddListener(() => selector.OnSelectedSlots(idx));

        button.onClick.RemoveAllListeners();
        button.onClick = new EventsButton.ButtonClickedEvent();
        button.onClick.AddListener(() => selector.OnAcceptSlots(idx));
    }
}

[HarmonyPatch(typeof(KeepFocus), "Awake")]
class t2
{
    public static void Prefix(KeepFocus __instance, List<GameObject> ___allowedObjects)
    {
        if (__instance.name == "UI_SLOT")
        {
            FocusObjects = ___allowedObjects;
        }
    }

    public static List<GameObject> FocusObjects { get; private set; }
}