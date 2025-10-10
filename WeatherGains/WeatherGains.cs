// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using HarmonyLib;

using BepInEx.Logging;
using BepInEx;

namespace WeatherGains;

[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
public class WeatherGains : BaseUnityPlugin
{
    internal static PluginConfig BoundConfig { get; private set; } = null!; 
    private new static ManualLogSource Logger { get; set; } = null!;
    private static Harmony Harmony { get; set; } = null!;

    private void Awake()
    {
        BoundConfig = new PluginConfig(base.Config);
        Logger = base.Logger;
        
        if (!BoundConfig.PluginEnabled.Value)
        {
            Logger.LogWarning($"{PluginInfo.NAME} v{PluginInfo.VERSION} is disabled! If this is not intentional, please enable it in the config file.");
            return;
        }
        
        Logger.LogInfo($"{PluginInfo.NAME} v{PluginInfo.VERSION} is loading...");
        ApplyPatches();
        Logger.LogInfo($"{PluginInfo.NAME} v{PluginInfo.VERSION} has loaded!");
    }

    private static void ApplyPatches()
    {
        Harmony ??= new Harmony(PluginInfo.GUID);
        Logger.LogDebug("Applying patches...");
        Harmony.PatchAll();
        Logger.LogDebug("Successfully applied all patches!");
    }
    
    internal static void RemovePatches()
    {
        if (Harmony is null) return;
        Logger.LogDebug("Removing patches...");
        Harmony.UnpatchAll(Harmony.Id);
        Logger.LogDebug("Successfully removed all patches!");
        Harmony = null;
    }
}