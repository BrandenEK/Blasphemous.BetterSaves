using Blasphemous.Framework.Menus;
using Blasphemous.Framework.Menus.Options;
using Blasphemous.ModdingAPI;
using UnityEngine;

namespace Blasphemous.BetterSaves.Naming;

/// <summary>
/// Allows naming save files through a menu
/// </summary>
public class NameMenu : ModMenu
{
    private TextOption _nameOption;

    /// <summary>
    /// This menu should always appear first
    /// </summary>
    protected override int Priority { get; } = int.MinValue;

    /// <summary>
    /// Create menu with a single text prompt
    /// </summary>
    protected override void CreateUI(Transform ui)
    {
        TextCreator creator = new(this)
        {
            TextSize = 54,
            LineSize = 500,
        };

        string text = Main.BetterSaves.LocalizationHandler.Localize("menuop");
        _nameOption = creator.CreateOption("nameoption", ui, Vector2.zero, $"{text}:", false, true, 32);
    }

    /// <summary>
    /// When menu is opened, clear the text box
    /// </summary>
    public override void OnStart()
    {
        _nameOption.CurrentValue = string.Empty;
    }

    /// <summary>
    /// When menu is closed, save the file name somewhere
    /// </summary>
    public override void OnFinish()
    {
        ModLog.Warn($"Completing saves menu with text: ({_nameOption.CurrentValue})");

        if (string.IsNullOrEmpty(_nameOption.CurrentValue))
            return;

        Main.BetterSaves.NameHandler.TrySetSaveName(_nameOption.CurrentValue);
    }
}
