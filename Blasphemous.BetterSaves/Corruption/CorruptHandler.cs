using Blasphemous.ModdingAPI;
using Gameplay.UI;

namespace Blasphemous.BetterSaves.Corruption;

public class CorruptHandler
{
    public bool IsShowingConfirmation { get; private set; }

    public void TempUpdate()
    {
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.P))
        {
            IsShowingConfirmation = true;
            UIController.instance.ShowConfirmationWidget("These mods are missing [Randomizer, Multiworld]. Are you sure you want to continue?", OnAccept, OnDissent);
        }
    }

    private void OnAccept()
    {
        ModLog.Warn("Aceept slots");
        IsShowingConfirmation = false;
    }

    private void OnDissent()
    {
        ModLog.Warn("Said no to slots");
        IsShowingConfirmation = false;
    }
}
