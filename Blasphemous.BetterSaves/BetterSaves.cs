using Blasphemous.BetterSaves.Naming;
using Blasphemous.BetterSaves.Slots;
using Blasphemous.ModdingAPI;
using Gameplay.UI.Others.MenuLogic;
using UnityEngine;

namespace Blasphemous.BetterSaves;

public class BetterSaves : BlasMod
{
    public BetterSaves() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public NameHandler NameHandler { get; } = new();
    public SlotHandler SlotHandler { get; } = new();

    private int _currentMultiplier = 0;

    public int CurrentScreen { get; set; } = 0;

    protected override void OnNewGame()
    {
        string name = System.DateTime.Now.ToString();
        NameHandler.SetSaveName(name);
    }

    protected override void OnUpdate()
    {
        if (!SlotsWidget.IsShowing)
            return;

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
