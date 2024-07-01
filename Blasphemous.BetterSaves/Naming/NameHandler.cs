using Framework.Achievements;
using Framework.Managers;

namespace Blasphemous.BetterSaves.Naming;

/// <summary>
/// Handles naming save files
/// </summary>
public class NameHandler
{
    /// <summary>
    /// Set the name of the current save file
    /// </summary>
    public bool TrySetSaveName(string name)
    {
        Achievement ach = new("SAVE_NAME")
        {
            Name = name
        };

        Core.AchievementsManager.Achievements["SAVE_NAME"] = ach;
        return true;
    }
}
