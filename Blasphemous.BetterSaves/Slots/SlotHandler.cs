using Blasphemous.ModdingAPI.Input;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves.Slots;

public class SlotHandler
{
    private int _currentScreen = 0;

    private List<SaveSlot> _slotList;
    private List<GameObject> _focusList;

    public void StoreSlotList(List<SaveSlot> slotList) => _slotList = slotList;
    public void StoreFocusList(List<GameObject> focusList) => _focusList = focusList;

    /// <summary>
    /// Check for shoulder button input and change screen
    /// </summary>
    public void UpdateSlots()
    {
        if (_slotList == null)
            return;

        // If not on save slot menu, reset screen to zero
        if (!SlotsWidget.IsShowing)
        {
            _currentScreen = 0;
            return;
        }

        if (_currentScreen > 0 && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryLeft))
        {
            Main.BetterSaves.Log($"Moving left to screen {--_currentScreen}");

            RefreshSlots();
            EventSystem.current.SetSelectedGameObject(_slotList[SlotsWidget.SelectedSlot - 3].gameObject, null);
        }
        if (_currentScreen < MAX_SCREENS && Main.BetterSaves.InputHandler.GetButtonDown(ButtonCode.InventoryRight))
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

        // Make more copies of the buttons
        for (int i = 0; i < MAX_SCREENS; i++)
        {
            GameObject slot1 = Object.Instantiate(_slotList[0].gameObject, _slotList[0].transform.parent);
            GameObject slot2 = Object.Instantiate(_slotList[1].gameObject, _slotList[1].transform.parent);
            GameObject slot3 = Object.Instantiate(_slotList[2].gameObject, _slotList[2].transform.parent);

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

            AddButtonEvents(button1, i * 3 + 3);
            AddButtonEvents(button2, i * 3 + 4);
            AddButtonEvents(button3, i * 3 + 5);

            _slotList.Add(slot1.GetComponent<SaveSlot>());
            _slotList.Add(slot2.GetComponent<SaveSlot>());
            _slotList.Add(slot3.GetComponent<SaveSlot>());

            _focusList.Add(slot1);
            _focusList.Add(slot2);
            _focusList.Add(slot3);
        }

        Main.BetterSaves.LogWarning($"Added {MAX_SCREENS * 3} more save slots");
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

    private const int MAX_SCREENS = 3;
}
