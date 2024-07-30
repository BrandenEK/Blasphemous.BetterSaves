using Framework.Achievements;
using Framework.Managers;
using HarmonyLib;

namespace Blasphemous.BetterSaves;

// Add save name achievement whenever list is reset
[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.ResetPersistence))]
class AchievementsManager_ResetPersistence_Patch
{
    public static void Postfix(AchievementsManager __instance)
    {
        __instance.Achievements.Add("SAVE_NAME", new Achievement("SAVE_NAME"));
    }
}
