using Framework.Achievements;
using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Blasphemous.BetterSaves.Naming;

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

            string slotName;
            if (i == 7 || i == 8)
            {
                slotName = "Internal data (Don't touch)";
            }
            else
            {
                slotName = slotData.achievement.achievements.FirstOrDefault(x => x.Id == "SAVE_NAME")?.Name;
                if (string.IsNullOrEmpty(slotName))
                    slotName = "Unamed save file";
            }

            // Send extra info to the slot
            Main.BetterSaves.Log($"Displaying name for slot {i}: {slotName}");
            ___slots[i].SetData("ignore", slotName, 0, false, false, false, 0, SelectSaveSlots.SlotsModes.Normal);
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
            ___ZoneText.text = $"{info}";
            return false;
        }
        return true;
    }
}

// Add save name achievement whenever list is reset
[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.ResetPersistence))]
class AchievementsManager_ResetPersistence_Patch
{
    public static void Postfix(AchievementsManager __instance)
    {
        __instance.Achievements.Add("SAVE_NAME", new Achievement("SAVE_NAME"));
    }
}