using Blasphemous.ModdingAPI;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using UnityEngine;

namespace Blasphemous.BetterSaves.Corruption;

/// <summary>
/// Handles verifying if a save file will become corrupted
/// </summary>
public class CorruptHandler
{
    /// <summary>
    /// Is the mod confirmation box currently showing
    /// </summary>
    public bool IsShowingConfirmation => _isShowing;

    private bool _isShowing = false;
    private bool _pressedAccept = false;
    private int _currentSlot = -1;

    /// <summary>
    /// Whether the confirmation box is about to display
    /// </summary>
    public bool ShouldDisplayBox(int slot)
    {
        if (_pressedAccept)
        {
            _pressedAccept = false;
            return false;
        }

        // Check if mods are invalid

        ModLog.Info($"Displaying confirmation box for slot {slot}");
        _isShowing = true;
        _currentSlot = slot;
        UIController.instance.ShowConfirmationWidget("These mods are missing [Randomizer, Multiworld]. Are you sure you want to continue?", OnAccept, OnDissent);
        return true;
    }

    private void OnAccept()
    {
        ModLog.Warn("Corruption confirmation: accept");

        _isShowing = false;
        _pressedAccept = true;
        Object.FindObjectOfType<SelectSaveSlots>().OnAcceptSlots(_currentSlot);
    }

    private void OnDissent()
    {
        ModLog.Warn("Corruption confirmation: dissent");

        _isShowing = false;
    }
}
