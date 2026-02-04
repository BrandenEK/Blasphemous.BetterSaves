using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves.Naming;

/// <summary>
/// Display name of save slot on select screen
/// </summary>
[HarmonyPatch(typeof(SelectSaveSlots), nameof(SelectSaveSlots.SetAllData))]
class SelectSaveSlots_SetAllData_Patch
{
    public static void Postfix(List<SaveSlot> ___slots)
    {
        for (int i = 0; i < ___slots.Count; i++)
        {
            PersistentManager.PublicSlotData slotData = Core.Persistence.GetSlotData(i);
            if (slotData == null)
                continue;

            string slotName;
            if (i == 7 || i == 8)
            {
                slotName = Main.BetterSaves.LocalizationHandler.Localize("svint");
            }
            else
            {
                slotName = slotData.achievement.achievements.FirstOrDefault(x => x.Id == "SAVE_NAME")?.Name;
                if (string.IsNullOrEmpty(slotName))
                    slotName = Main.BetterSaves.LocalizationHandler.Localize("svunm");
            }

            // Send extra info to the slot
            ModLog.Info($"Displaying name for slot {i}: {slotName}");
            ___slots[i].SetData("ignore", slotName, 0, false, false, false, 0, SelectSaveSlots.SlotsModes.Normal);
        }
    }
}
[HarmonyPatch(typeof(SaveSlot), nameof(SaveSlot.SetData))]
class SaveSlot_SetData_Patch
{
    public static bool Prefix(string zoneName, string info, ref Text ___ZoneText)
    {
        if (zoneName == "ignore")
        {
            ___ZoneText.text = $"{info}";
            return false;
        }
        return true;
    }
}
