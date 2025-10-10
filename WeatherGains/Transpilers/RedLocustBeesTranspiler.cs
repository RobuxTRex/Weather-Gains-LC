// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace WeatherGains.Transpilers;

[HarmonyPatch(typeof(RedLocustBees))]
public class RedLocustBeesTranspiler
{
    [HarmonyPatch("SpawnHiveNearEnemy")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> SpawnHiveNearEnemyTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        if (WeatherGains.BoundConfig.CompatibilityMode.Value) return instructions;
        
        var codes = new List<CodeInstruction>(instructions);

        var clientMethodCall = AccessTools.Method(typeof(RedLocustBees), "SpawnHiveClientRpc");
        var applyValueMultiplierMethod = AccessTools.Method(typeof(MultiplierManager), nameof(MultiplierManager.ApplyValueMultiplier));
        
        for (var i = 1; i < codes.Count; i++)
        {
            if (!codes[i].Calls(clientMethodCall)) continue;
            
            for (var j = i - 1; j >= 0; j--)
            {
                if (!(codes[j].opcode == OpCodes.Ldloc_3)) continue;
                codes.Insert(j + 1, new CodeInstruction(OpCodes.Call, applyValueMultiplierMethod));
                return codes;
            }
            
            return codes;
        }
        
        return codes;
    }
}