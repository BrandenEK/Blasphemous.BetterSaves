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
        if (Core.Events.GetFlag("NAMED"))
            return false;

        Core.Events.SetFlag("NAMED", true, true);
        Core.Events.SetFlag("NAME_" + name, true, true);
        return true;
    }
}
