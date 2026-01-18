// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;
using WeatherGains.Types;

namespace WeatherGains;

public class PluginConfig
{
    private const string SectionMultipliers = "Multipliers";
    private const string SectionGeneral = "General";

    private const int ConfigVersion = 2;

    public ConfigEntry<bool>
        LungValueMultiEnabled,
        AmountMultiEnabled,
        ValueMultiEnabled,
        CompatibilityMode,
        PluginEnabled;

    public ConfigEntry<float>
        ValueMultiVariation;

    public readonly Dictionary<LevelWeatherType, MultiValueGroup> Multipliers = new();

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = true;
        ApplyConfig(config);
        ClearOrphanedEntries(config);
        config.Save();
    }

    private void ApplyConfig(ConfigFile config)
    {
        // Important config version binding
        // Ensures the config is compatible with the current version of the plugin
        // If the config isn't, the plugin will attempt to migrate on a best-effort basis
        // This excludes Config Version 1 (v1.x.x), because the Plugin GUID changed
        config.Bind(
            "Meta",
            "ConfigVersion",
            ConfigVersion,
            "!!! DO NOT CHANGE THIS VALUE !!!"
        );
        
        // General settings
        LungValueMultiEnabled = config.Bind(
            SectionGeneral,
            "ApparatusMultiplierEnabled",
            true,
            "Toggles the value multiplier on the apparatus (lung)."
        );
        ValueMultiVariation = config.Bind(
            SectionGeneral,
            "ValueMultiVariation",
            5f,
            "Sets the maximum random variation (in percent) for the value multiplier (makes scrap values more natural). Set to 0 to disable."
        );
        AmountMultiEnabled = config.Bind(
            SectionGeneral,
            "AmountMultiplierEnabled",
            true,
            "Toggles the amount multiplier on scrap."
        );
        ValueMultiEnabled = config.Bind(
            SectionGeneral,
            "ValueMultiplierEnabled",
            true,
            "Toggles the value multiplier on scrap."
        );
        
        CompatibilityMode = config.Bind(
            SectionGeneral,
            "CompatibilityMode",
            false,
            "Disables all transpilers for compatibility with other mods. This will prevent multipliers from being applied to bee hives."
        );
        
        PluginEnabled = config.Bind(
            SectionGeneral,
            "PluginEnabled",
            true,
            "Toggles the functionality of the plugin; an easy way to turn everything off!"
        );

        // Dynamically adds multipliers for each weather type
        foreach (LevelWeatherType weather in Enum.GetValues(typeof(LevelWeatherType)))
        {
            // Assigns weather types with default multipliers to Multipliers dictionary
            Multipliers[weather] = new MultiValueGroup
            {
                AmountMultiplier = BindMultiplier(config, weather, "Amount", DefaultAmount(weather)),
                ValueMultiplier = BindMultiplier(config, weather, "Value", DefaultValue(weather))
            };
        }
    }

    private static ConfigEntry<float> BindMultiplier(
        ConfigFile config,
        LevelWeatherType weather,
        string type,
        float defaultValue
    )
    {
        return config.Bind(
            SectionMultipliers,
            $"{weather}{type}Multiplier",
            defaultValue,
            $"{type} multiplier applied during {weather.ToString().ToLower()} weather. Set to 1 to disable."
        );
    }

    private static float DefaultAmount(LevelWeatherType weather) => weather switch
    {
        LevelWeatherType.Foggy => 1.10f,
        LevelWeatherType.Rainy => 1.10f,
        LevelWeatherType.Stormy => 1.20f,
        LevelWeatherType.Flooded => 1.20f,
        LevelWeatherType.Eclipsed => 1.30f,
        _ => 1f
    };

    private static float DefaultValue(LevelWeatherType weather) => weather switch
    {
        LevelWeatherType.Foggy => 1.20f,
        LevelWeatherType.Rainy => 1.20f,
        LevelWeatherType.Stormy => 1.35f,
        LevelWeatherType.Flooded => 1.40f,
        LevelWeatherType.Eclipsed => 1.75f,
        _ => 1f
    };

    private static void ClearOrphanedEntries(ConfigFile config)
    {
        var orphanedProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        if (orphanedProp?.GetValue(config) is IDictionary orphaned)
            orphaned.Clear();
    }
}