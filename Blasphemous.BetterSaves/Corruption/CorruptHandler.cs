using Blasphemous.BetterSaves.Extensions;
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Helpers;
using Framework.Managers;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        string[] mods = ModHelper.LoadedMods.Select(x => x.Name).ToArray();
        ModLog.Info($"Storing info for {mods.Length} loaded mods");

        Core.AchievementsManager.Achievements["SAVE_NAME"].Description = string.Join("~~~", mods);
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
        IEnumerable<string> savedMods = modText.Split(new string[] { "~~~" }, System.StringSplitOptions.RemoveEmptyEntries);
        ModLog.Debug($"Saved mods: {savedMods.FormatList()}");

        // Get list of mod ids that are currently loaded
        IEnumerable<string> currentMods = ModHelper.LoadedMods.Select(x => x.Name);
        ModLog.Debug($"Current mods: {currentMods.FormatList()}");

        // Get list of mod ids that are in the save but not currently loaded
        IEnumerable<string> missingMods = savedMods.Where(x => !currentMods.Any(y => x == y));

        // Get list of mod ids that are currently loaded but not in the save
        IEnumerable<string> addedMods = currentMods.Where(x => !savedMods.Any(y => x == y));

        // Ensure there are either missing or added mods
        if (!missingMods.Any() && !addedMods.Any())
            return false;

        // Create display text
        StringBuilder sb = new();
        if (missingMods.Any())
            sb.AppendLine($"Mods missing since last save: {missingMods.FormatList()}");
        if (addedMods.Any())
            sb.AppendLine($"Mods added since last save: {addedMods.FormatList()}");
        sb.AppendLine("Are you sure you wish to continue?");

        _isShowing = true;
        _currentSlot = slot;
        ModLog.Info($"Displaying confirmation box for slot {slot}");
        UIController.instance.ShowConfirmationWidget(sb.ToString(), OnAccept, OnDissent);
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
