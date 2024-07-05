using Blasphemous.ModdingAPI.Input;
using Framework.Managers;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves.Slots;

/// <summary>
/// Handles adding more slots to the main menu
/// </summary>
public class SlotHandler(int maxScreens)
{
    private readonly int _maxScreens = maxScreens;
    private int _currentScreen = 0;

    private List<SaveSlot> _slotList;
    private List<GameObject> _focusList;

    internal void StoreSlotList(List<SaveSlot> slotList) => _slotList = slotList;
    internal void StoreFocusList(List<GameObject> focusList) => _focusList = focusList;

    /// <summary>
    /// Check for shoulder button input and change screen
    /// </summary>
    public void UpdateSlots()
    {
        // If slots menu is not visible, dont process input
        if (_slotList == null || !SlotsWidget.IsShowing || !SlotsWidget.gameObject.activeSelf)
            return;

        if (_currentScreen > 0 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryLeft))
        {
            Main.BetterSaves.Log($"Moving left to screen {--_currentScreen}");

            RefreshSlots();
            EventSystem.current.SetSelectedGameObject(_slotList[SlotsWidget.SelectedSlot - 3].gameObject, null);
        }

        if (_currentScreen < _maxScreens && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryRight))
        {
            Main.BetterSaves.Log($"Moving right to screen {++_currentScreen}");

            RefreshSlots();
            EventSystem.current.SetSelectedGameObject(_slotList[SlotsWidget.SelectedSlot + 3].gameObject, null);
        }
    }

    /// <summary>
    /// Whenever opening or changing screens, change which slots are active
    /// </summary>
    public void RefreshSlots()
    {
        if (_slotList == null)
            return;

        for (int i = 0; i < _slotList.Count; i += 3)
        {
            bool screenActive = i / 3 == _currentScreen;
            _slotList[i + 0].gameObject.SetActive(screenActive);
            _slotList[i + 1].gameObject.SetActive(screenActive);
            _slotList[i + 2].gameObject.SetActive(screenActive);
        }
    }

    /// <summary>
    /// If new slots haven't already been created, duplicate the UI and adjust properties
    /// </summary>
    public void CreateNewSlots()
    {
        // Ensure lists have been stored
        if (_slotList == null || _focusList == null)
            return;

        // Ensure slots haven't already been created
        if (_slotList.Count > 3)
            return;

        int totalSlots = _maxScreens * 3 + 3;

        // Create all ui slots
        for (int i = 3; i < totalSlots; i++)
        {
            GameObject slot = Object.Instantiate(_slotList[i % 3].gameObject, _slotList[0].transform.parent);
            slot.name = $"slot{i}";

            _slotList.Add(slot.GetComponent<SaveSlot>());
            _focusList.Add(slot);
        }

        // Set navigation and events for each one
        for (int i = 3; i < totalSlots; i++)
        {
            EventsButton button = _slotList[i].GetComponent<EventsButton>();
            AddButtonEvents(button, i);

            Navigation nav = button.navigation;
            nav.selectOnUp = _slotList[i + (i % 3 == 0 ? 2 : -1)].GetComponent<Selectable>();
            nav.selectOnDown = _slotList[i + (i % 3 == 2 ? -2 : 1)].GetComponent<Selectable>();
            button.navigation = nav;
        }

        Main.BetterSaves.LogWarning($"Added {_maxScreens * 3} more save slots");
    }

    /// <summary>
    /// Add select and click event handlers to the button
    /// </summary>
    private void AddButtonEvents(EventsButton button, int idx)
    {
        button.onSelected.RemoveAllListeners();
        button.onSelected = new EventsButton.ButtonSelectedEvent();
        button.onSelected.AddListener(() => SlotsWidget.OnSelectedSlots(idx));

        button.onClick.RemoveAllListeners();
        button.onClick = new EventsButton.ButtonClickedEvent();
        button.onClick.AddListener(() => SlotsWidget.OnAcceptSlots(idx));
        button.onClick.AddListener(() => Core.Audio.PlayOneShot("event:/SFX/UI/EquipItem"));
    }

    private SelectSaveSlots x_slotsWidget = null;
    private SelectSaveSlots SlotsWidget
    {
        get
        {
            if (x_slotsWidget == null)
                x_slotsWidget = Object.FindObjectOfType<SelectSaveSlots>();
            return x_slotsWidget;
        }
    }
}
