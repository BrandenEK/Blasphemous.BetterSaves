using Framework.Managers;

namespace Blasphemous.BetterSaves.Naming;

public class NameHandler
{
    public void SetSaveName(string name)
    {
        Core.Events.SetFlag("NAME_" + name, true, true);
    }
}
