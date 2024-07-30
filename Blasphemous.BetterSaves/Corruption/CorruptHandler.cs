using Blasphemous.ModdingAPI;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using UnityEngine;

namespace Blasphemous.BetterSaves.Corruption;

public class CorruptHandler
{
    public bool IsShowingConfirmation => _isShowing;

    private bool _isShowing = false;
    private bool _pressedAccept = false;
    private int _currentSlot = -1;

    public void TempUpdate()
    {
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.P))
        {
        }
    }

    public bool ShouldDisplayBox(int slot)
    {
        if (_pressedAccept)
        {
            _pressedAccept = false;
            return false;
        }

        ModLog.Error("Opening slot " + slot);
        _isShowing = true;
        _currentSlot = slot;
        UIController.instance.ShowConfirmationWidget("These mods are missing [Randomizer, Multiworld]. Are you sure you want to continue?", OnAccept, OnDissent);
        return true;
    }

    private void OnAccept()
    {
        ModLog.Warn("Accept slots");
        _isShowing = false;
        _pressedAccept = true;
        Object.FindObjectOfType<SelectSaveSlots>().OnAcceptSlots(_currentSlot);
    }

    private void OnDissent()
    {
        ModLog.Warn("Said no to slots");
        _isShowing = false;
    }
}
