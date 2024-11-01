using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    [HarmonyPatch("SpawnScrapInLevel")]
    [HarmonyPrefix]
    private static void SpawnScrapInLevelPrefix()
    {
        if (WeatherGains.BoundConfig.ValueMultiEnabled.Value) RoundManager.Instance.scrapValueMultiplier *= WeatherGains.BoundConfig.Multipliers[TimeOfDay.Instance.currentLevelWeather].ValueMultiplier.Value;
        if (WeatherGains.BoundConfig.AmountMultiEnabled.Value)  RoundManager.Instance.scrapAmountMultiplier *= WeatherGains.BoundConfig.Multipliers[TimeOfDay.Instance.currentLevelWeather].AmountMultiplier.Value;
        
        WeatherGains.Logger.LogInfo(
            $"Successfully modified the scrap generation values for weather type {TimeOfDay.Instance.currentLevelWeather} on moon (level) {TimeOfDay.Instance.currentLevel}!\n\n" + 
            $"Modded Value Multiplier: {RoundManager.Instance.scrapValueMultiplier}\n" +
            $"Modded Amount Multiplier: {RoundManager.Instance.scrapAmountMultiplier}"
        );
    }

    [HarmonyPatch("SpawnScrapInLevel")]
    [HarmonyPostfix]
    private static void SpawnScrapInLevelPostfix()
    {
        if (WeatherGains.BoundConfig.ValueMultiEnabled.Value) RoundManager.Instance.scrapValueMultiplier = 1f;
        if (WeatherGains.BoundConfig.AmountMultiEnabled.Value)  RoundManager.Instance.scrapAmountMultiplier = 1f;
        
        WeatherGains.Logger.LogInfo(
            $"Successfully reverted the scrap generation values for weather type {TimeOfDay.Instance.currentLevelWeather} on moon (level) {TimeOfDay.Instance.currentLevel}!\n\n" + 
            $"Reverted Value Multiplier: {RoundManager.Instance.scrapValueMultiplier}\n" +
            $"Reverted Amount Multiplier: {RoundManager.Instance.scrapAmountMultiplier}\n\n" +
            $"This is intended functionality... the mod isn't breaking!"
        );
    }
}