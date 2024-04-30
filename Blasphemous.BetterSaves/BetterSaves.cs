using Blasphemous.ModdingAPI;
using Framework.Managers;
using System;
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

    protected override void OnInitialize()
    {
        LogError($"{ModInfo.MOD_NAME} has been initialized");
    }

    protected override void OnNewGame()
    {
        string name = DateTime.Now.ToString();
        SetSaveName(name);
    }

    protected override void OnLateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
            _currentMultiplier++;
        if (Input.GetKeyDown(KeyCode.Keypad8))
            _currentMultiplier--;
    }

    public void SetSaveName(string name)
    {
        Core.Events.SetFlag("NAME_" + name, true, true);
    }
}
