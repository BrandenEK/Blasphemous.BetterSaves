using Gameplay.UI;

namespace Blasphemous.BetterSaves.Corruption;

public class CorruptHandler
{
    public void TempUpdate()
    {
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.P))
        {
            UIController.instance.ShowConfirmationWidget("These mods are missing [Randomizer, Multiworld]. Are you sure you want to continue?",
                () => LogWarning("Aceept slots"),
                () => LogWarning("Said no to slots"));
        }
    }
}
