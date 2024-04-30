using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Input;
using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using UnityEngine;

namespace Blasphemous.BetterSaves;

public class BetterSaves : BlasMod
{
    public BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private int _currentMultiplier = 0;

    public int GetRealSlot(int slot)
    {
        return slot + 3 * _currentMultiplier;
    }

    protected override void OnNewGame()
    {
        string name = System.DateTime.Now.ToString();
        SetSaveName(name);
    }

    protected override void OnUpdate()
    {
        if (!SlotsWidget.IsShowing)
            return;

        if (_currentMultiplier > 0 && InputHandler.GetButtonDown(ButtonCode.InventoryLeft))
        {
            LogWarning("Moving left");
            _currentMultiplier--;
            RefreshSlots();
        }
        if (_currentMultiplier < MAX_MULTIPLIER && InputHandler.GetButtonDown(ButtonCode.InventoryRight))
        {
            LogWarning("Moving right");
            _currentMultiplier++;
            RefreshSlots();
        }
    }

    private void RefreshSlots()
    {
        int currentSlot = SlotsWidget.SelectedSlot;

        SlotsWidget.Clear();
        SlotsWidget.SetAllData(MainMenu, SelectSaveSlots.SlotsModes.Normal);
        SlotsWidget.OnSelectedSlots(currentSlot);
    }

    public void SetSaveName(string name)
    {
        Core.Events.SetFlag("NAME_" + name, true, true);
    }

    private NewMainMenu x_mainMenu = null;
    private NewMainMenu MainMenu
    {
        get
        {
            if (x_mainMenu == null)
                x_mainMenu = Object.FindObjectOfType<NewMainMenu>();
            return x_mainMenu;
        }
    }

    private SelectSaveSlots x_slotsWidget = null;
    private SelectSaveSlots SlotsWidget
    {
        get
        {
            if (x_slotsWidget == null)
                x_slotsWidget = Object.FindObjectOfType<SelectSaveSlots>();
            return x_slotsWidget;
        }
    }

    private const int MAX_MULTIPLIER = 3;
}
