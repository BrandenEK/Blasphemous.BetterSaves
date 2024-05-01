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
    public void SetSaveName(string name)
    {
        Core.Events.SetFlag("NAME_" + name, true, true);
    }
}
