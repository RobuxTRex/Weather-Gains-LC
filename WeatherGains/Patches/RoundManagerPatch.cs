using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    [HarmonyPatch("SpawnScrapInLevel")]
    [HarmonyPrefix]
    private static void SpawnScrapInLevelPatch()
    {
        RoundManager.Instance.scrapValueMultiplier = WeatherGains.BoundConfig.Multipliers[TimeOfDay.Instance.currentLevelWeather].ValueMultiplier.Value;
        RoundManager.Instance.scrapAmountMultiplier = WeatherGains.BoundConfig.Multipliers[TimeOfDay.Instance.currentLevelWeather].AmountMultiplier.Value;
        WeatherGains.Logger.LogInfo(
            $"Successfully attempted to modify the scrap generation values for weather type {TimeOfDay.Instance.currentLevelWeather} on moon (level) {TimeOfDay.Instance.currentLevel}!\n\n" + 
            $"Value Multiplier: {RoundManager.Instance.scrapValueMultiplier}\n" +
            $"Amount Multiplier: {RoundManager.Instance.scrapAmountMultiplier}"
        );;
    }
}