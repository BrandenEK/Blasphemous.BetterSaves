using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Helpers;
using Framework.Managers;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using Newtonsoft.Json;
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
    public void StoreModInfo()
    {
        var mods = ModHelper.LoadedMods.Select(x => new SerializedModInfo(x.Name, x.Version));
        string json = JsonConvert.SerializeObject(mods, Formatting.None);

        ModLog.Info($"Storing info for {mods.Count()} loaded mods");
        Core.AchievementsManager.Achievements["SAVE_NAME"].Description = json;
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

        // Get list of mods from the save file
        string json = slotData.achievement.achievements.FirstOrDefault(x => x.Id == "SAVE_NAME")?.Description ?? "[]";
        IEnumerable<SerializedModInfo> savedMods = JsonConvert.DeserializeObject<SerializedModInfo[]>(json);
        ModLog.Debug($"Saved mods: {savedMods.FormatList(true)}");

        // Get list of mods that are currently loaded
        IEnumerable<SerializedModInfo> currentMods = ModHelper.LoadedMods.Select(x => new SerializedModInfo(x.Name, x.Version));
        ModLog.Debug($"Current mods: {currentMods.FormatList(true)}");

        // Get lists of invalidities
        IEnumerable<SerializedModInfo> missingMods = Invalidities.GetMissing()(savedMods, currentMods);
        IEnumerable<SerializedModInfo> addedMods = Invalidities.GetAdded()(savedMods, currentMods);
        IEnumerable<SerializedModInfo> updatedMods = Invalidities.GetUpdated()(savedMods, currentMods);
        IEnumerable<SerializedModInfo> downgradedMods = Invalidities.GetDowngraded()(savedMods, currentMods);

        // Ensure there is at least one invalidity
        if (!missingMods.Any() && !addedMods.Any() && !updatedMods.Any() && !downgradedMods.Any())
            return false;

        // Create display text
        StringBuilder sb = new();
        if (missingMods.Any())
            sb.AppendLine($"Mods missing since last save: {missingMods.FormatList(false)}");
        if (addedMods.Any())
            sb.AppendLine($"Mods added since last save: {addedMods.FormatList(false)}");
        if (updatedMods.Any())
            sb.AppendLine($"Mods updated since last save: {updatedMods.FormatList(false)}");
        if (downgradedMods.Any())
            sb.AppendLine($"Mods downgraded since last save: {downgradedMods.FormatList(false)}");
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
