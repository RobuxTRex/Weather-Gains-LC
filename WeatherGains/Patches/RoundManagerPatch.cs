// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    private static float _appliedMultiplier = 1f;
    
    [HarmonyPatch("SpawnScrapInLevel")]
    [HarmonyPrefix]
    private static void SpawnScrapInLevelPrefix()
    {
        // Retrieves the amount multiplier for the current weather
        var amountMultiplier = MultiplierManager.ApplyAmountMultiplier(RoundManager.Instance.scrapAmountMultiplier);
        
        // Apply scrap amount multiplier
        RoundManager.Instance.scrapAmountMultiplier = amountMultiplier;
        _appliedMultiplier = amountMultiplier;
    }

    [HarmonyPatch("SpawnScrapInLevel")]
    [HarmonyPostfix]
    private static void SpawnScrapInLevelPostfix()
    {
        // Reverts the applied scrap amount multiplier
        RoundManager.Instance.scrapAmountMultiplier /= _appliedMultiplier;
        _appliedMultiplier = 1f;
    }
}