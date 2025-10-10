// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(LungProp))]
public class LungPropPatch
{
    [HarmonyPatch("DisconnectFromMachinery")]
    [HarmonyPrefix]
    private static void DisconnectFromMachineryPatch(LungProp __instance)
    {
        if (!WeatherGains.BoundConfig.LungValueMultiEnabled.Value) return; // Return if lung multi isn't enabled
        
        // Apply the multiplier to the scrap value
        __instance.SetScrapValue(MultiplierManager.ApplyValueMultiplier(__instance.scrapValue));
    }
}