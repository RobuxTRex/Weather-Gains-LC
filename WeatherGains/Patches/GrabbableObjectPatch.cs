// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using HarmonyLib;

namespace WeatherGains.Patches;

[HarmonyPatch(typeof(GrabbableObject))]
public class GrabbableObjectPatch
{
    [HarmonyPatch("SetScrapValue")]
    [HarmonyPrefix]
    private static void SetScrapValuePrefix(ref int setValueTo)
    {
        // Apply the multiplier to the scrap value
        setValueTo = MultiplierManager.ApplyValueMultiplier(setValueTo);
    }
}