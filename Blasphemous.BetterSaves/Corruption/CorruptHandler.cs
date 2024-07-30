using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Helpers;
using Framework.Managers;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using System.Collections.Generic;
using System.Linq;
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
    /// Stores all loaded mods used with this save file
    /// </summary>
    public void LoadGame()
    {
        string[] modIds = ModHelper.LoadedMods.Select(x => x.Id).ToArray();
        ModLog.Info($"Storing info for {modIds.Length} loaded mods");

        Core.AchievementsManager.Achievements["SAVE_NAME"].Description = string.Join("~~~", modIds);
    }

    /// <summary>
    /// Whether the confirmation box is about to display
    /// </summary>
    public bool ShouldDisplayBox(int slot)
    {
        // If the box is closing, immediately start the game
        if (_pressedAccept)
        {
            _pressedAccept = false;
            return false;
        }

        // Ensure save data for this slot exists
        var slotData = Core.Persistence.GetSlotData(slot);
        if (slotData == null)
            return false;

        // Get mod list from the achievement description
        string modText = slotData.achievement.achievements.FirstOrDefault(x => x.Id == "SAVE_NAME")?.Description ?? string.Empty;

        // Get list of mod ids from the save file
        IEnumerable<string> savedModIds = modText.Split(new string[] { "~~~" }, System.StringSplitOptions.RemoveEmptyEntries);
        ModLog.Debug($"Saved mods: {string.Join(", ", savedModIds.ToArray())}");

        // Get list of mod ids that are currently loaded
        IEnumerable<string> currentModIds = ModHelper.LoadedMods.Select(x => x.Id);
        ModLog.Debug($"Current mods: {string.Join(", ", currentModIds.ToArray())}");

        // Get list of mod ids that are in the save but not currently loaded
        IEnumerable<string> missingModIds = savedModIds.Where(x => !currentModIds.Any(y => x == y));
        ModLog.Debug($"Missing mods: {string.Join(", ", missingModIds.ToArray())}");

        // Get list of mod ids that are currently loaded but not in the save
        IEnumerable<string> addedModIds = currentModIds.Where(x => !savedModIds.Any(y => x == y));
        ModLog.Debug($"Added mods: {string.Join(", ", addedModIds.ToArray())}");


        // Check if mods are invalid

        _isShowing = true;
        _currentSlot = slot;
        ModLog.Info($"Displaying confirmation box for slot {slot}");
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
