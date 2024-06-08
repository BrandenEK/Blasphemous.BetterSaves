using Blasphemous.Framework.Menus;
using Blasphemous.Framework.Menus.Options;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.BetterSaves.Naming;

/// <summary>
/// Allows naming save files through a menu
/// </summary>
public class NameMenu : ModMenu
{
    /// <summary>
    /// Creates a new name menu
    /// </summary>
    public NameMenu() : base("Better Saves Settings", 5) { }

    private TextOption _nameOption;

    protected override void CreateUI(Transform ui)
    {
        TextCreator creator = new(this)
        {
            TextSize = 54,
            LineSize = 500,
        };

        _nameOption = creator.CreateOption("nameoption", ui, Vector2.zero, "Save file name:", false, true, 32);
    }

    public override void OnFinish()
    {
        Main.BetterSaves.LogWarning($"Completing saves menu with text: ({_nameOption.CurrentValue})");

        if (string.IsNullOrEmpty(_nameOption.CurrentValue))
            return;

        Main.BetterSaves.LogError("Current save slot: " + PersistentManager.GetAutomaticSlot());
        Main.BetterSaves.NameHandler.TrySetSaveName(_nameOption.CurrentValue);
    }
}
