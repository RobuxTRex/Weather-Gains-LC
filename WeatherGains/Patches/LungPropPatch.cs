using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(LungProp))]
public class LungPropPatch
{
    [HarmonyPatch("DisconnectFromMachinery")]
    [HarmonyPrefix]
    private static void DisconnectFromMachineryPatch(LungProp __instance)
    {
        if (!WeatherGains.BoundConfig.LungValueMultiEnabled.Value) return;
        __instance.SetScrapValue((int)(__instance.scrapValue*WeatherGains.BoundConfig.Multipliers[TimeOfDay.Instance.currentLevelWeather].ValueMultiplier.Value));
    }
}