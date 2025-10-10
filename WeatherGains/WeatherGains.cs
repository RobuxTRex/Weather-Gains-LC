using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace WeatherGains;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class WeatherGains : BaseUnityPlugin
{
    
    internal static ModConfiguration BoundConfig { get; private set; } = null!;
    
    public static WeatherGains Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;
        
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} is loading...");

        BoundConfig = new ModConfiguration(base.Config);
        
        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}