using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves;

// Display name of save slot on select screen
[HarmonyPatch(typeof(SelectSaveSlots), "SetAllData")]
class SelectSaveSlotsData_Patch
{
    public static void Postfix(List<SaveSlot> ___slots)
    {
        for (int i = 0; i < ___slots.Count; i++)
        {
            PersistentManager.PublicSlotData slotData = Core.Persistence.GetSlotData(i);
            if (slotData == null)
                continue;

            string nameFlag = slotData.flags.flagsPreserve.Keys.FirstOrDefault(f => f.StartsWith("NAME_"));

            if (nameFlag == null)
                continue;

            // Send extra info to the slot
            Main.BetterSaves.Log($"Displaying name for slot {i}: {nameFlag.Substring(5)}");
            ___slots[i].SetData("ignore", nameFlag.Substring(5), 0, false, false, false, 0, SelectSaveSlots.SlotsModes.Normal);
        }
    }
}
[HarmonyPatch(typeof(SaveSlot), "SetData")]
class SaveSlotData_Patch
{
    public static bool Prefix(string zoneName, string info, ref Text ___ZoneText)
    {
        if (zoneName == "ignore")
        {
            ___ZoneText.text += $"   ({info})";
            return false;
        }
        return true;
    }
}
