using System;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;
using GameNetcodeStuff;
using Unity.Collections;
using Unity.Netcode;

namespace WeatherGains;

[Serializable]
public class ModConfiguration : SyncedInstance<ModConfiguration>
{
    public class MultiplierDictType
    {
        public ConfigEntry<float> ValueMultiplier { get; set; }
        public ConfigEntry<float> AmountMultiplier { get; set; }
    }
    
    public readonly ConfigEntry<bool>
        Enabled;

    public readonly ConfigEntry<float>
        FogValueMultiplier,
        FogAmountMultiplier,
        RainValueMultiplier,
        RainAmountMultiplier,
        ClearValueMultiplier,
        ClearAmountMultiplier,
        StormValueMultiplier,
        StormAmountMultiplier,
        FloodValueMultiplier,
        FloodAmountMultiplier,
        EclipsedValueMultiplier,
        EclipsedAmountMultiplier;

    public readonly Dictionary<LevelWeatherType, MultiplierDictType> Multipliers;

    public ModConfiguration(ConfigFile cfg)
    {
        InitInstance(this);
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), "ConnectClientToPlayerObject")]
        static void InitializeLocalPlayer() {
            if (IsHost) {
                MessageManager.RegisterNamedMessageHandler("ModName_OnRequestConfigSync", OnRequestSync);
                Synced = true;

                return;
            }

            Synced = false;
            MessageManager.RegisterNamedMessageHandler("ModName_OnReceiveConfigSync", OnReceiveSync);
            RequestSync();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameNetworkManager), "StartDisconnect")]
        static void PlayerLeave() {
            RevertSync();
        }
        
        static void RequestSync() {
            if (!IsClient) return;

            using FastBufferWriter stream = new(IntSize, Allocator.Temp);
            MessageManager.SendNamedMessage("ModName_OnRequestConfigSync", 0uL, stream);
        }
        
        static void OnRequestSync(ulong clientId, FastBufferReader _) {
            if (!IsHost) return;

            WeatherGains.Logger.LogInfo($"Config sync request received from client: {clientId}");

            byte[] array = SerializeToBytes(Instance);
            int value = array.Length;

            using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

            try {
                stream.WriteValueSafe(in value, default);
                stream.WriteBytesSafe(array);

                MessageManager.SendNamedMessage("ModName_OnReceiveConfigSync", clientId, stream);
            } catch(Exception e) {
                WeatherGains.Logger.LogInfo($"Error occurred syncing config with client: {clientId}\n{e}");
            }
        }
        
        static void OnReceiveSync(ulong _, FastBufferReader reader) {
            if (!reader.TryBeginRead(IntSize)) {
                WeatherGains.Logger.LogError("Config sync error: Could not begin reading buffer.");
                return;
            }

            reader.ReadValueSafe(out int val, default);
            if (!reader.TryBeginRead(val)) {
                WeatherGains.Logger.LogError("Config sync error: Host could not sync.");
                return;
            }

            byte[] data = new byte[val];
            reader.ReadBytesSafe(ref data, val);

            SyncInstance(data);

            WeatherGains.Logger.LogInfo("Successfully synced config with host.");
        }
        
        cfg.SaveOnConfigSet = false;

        Enabled = cfg.Bind(
            "General",
            "Enabled",
            true,
            "Whether the mod is enabled."
        );
        
        FogValueMultiplier = cfg.Bind(
            "Multipliers",
            "FogValueMultiplier",
            1.2f,
            "Scrap value multiplier during foggy weather on a moon. Set to 1 for no change."
        );
        
        FogAmountMultiplier = cfg.Bind(
            "Multipliers",
            "FogAmountMultiplier",
            1.1f,
            "Scrap amount multiplier during foggy weather on a moon. Set to 1 for no change."
        );
        
        RainValueMultiplier = cfg.Bind(
            "Multipliers",
            "RainValueMultiplier",
            1.2f,
            "Scrap value multiplier during rain weather on a moon. Set to 1 for no change."
        );
        
        RainAmountMultiplier = cfg.Bind(
            "Multipliers",
            "RainAmountMultiplier",
            1.1f,
            "Scrap amount multiplier during rain weather on a moon. Set to 1 for no change."
        );

        ClearValueMultiplier = cfg.Bind(
            "Multipliers",
            "ClearValueMultiplier",
            1f,
            "Scrap value multiplier during clear/no weather on a moon. Set to 1 for no change."
        );
        
        ClearAmountMultiplier = cfg.Bind(
            "Multipliers",
            "ClearAmountMultiplier",
            1f,
            "Scrap amount multiplier during clear/no weather on a moon. Set to 1 for no change."
        );
        
        FloodValueMultiplier = cfg.Bind(
            "Multipliers",
            "FloodValueMultiplier",
            1.5f,
            "Scrap value multiplier during floods on a moon. Set to 1 for no change."
        );
        
        FloodAmountMultiplier = cfg.Bind(
            "Multipliers",
            "FloodAmountMultiplier",
            1.25f,
            "Scrap amount multiplier during floods on a moon. Set to 1 for no change."
        );
        
        StormValueMultiplier = cfg.Bind(
            "Multipliers",
            "StormValueMultiplier",
            1.55f,
            "Scrap value multiplier during storms on a moon. Set to 1 for no change."
        );
        
        StormAmountMultiplier = cfg.Bind(
            "Multipliers",
            "StormAmountMultiplier",
            1.3f,
            "Scrap amount multiplier during storms on a moon. Set to 1 for no change."
        );
        
        EclipsedValueMultiplier = cfg.Bind(
            "Multipliers",
            "EclipsedValueMultiplier",
            2.1f,
            "Scrap value multiplier during eclipsed weather on a moon. Set to 1 for no change."
        );
        
        EclipsedAmountMultiplier = cfg.Bind(
            "Multipliers",
            "EclipsedAmountMultiplier",
            1.4f,
            "Scrap amount multiplier during eclipsed weather on a moon. Set to 1 for no change."
        );
        
        ClearOrphanedEntries(cfg);
        cfg.Save();
        cfg.SaveOnConfigSet = true;
        
        Multipliers = new Dictionary<LevelWeatherType, MultiplierDictType>
        {
            { LevelWeatherType.Foggy, new MultiplierDictType { ValueMultiplier = FogValueMultiplier, AmountMultiplier = FogAmountMultiplier } },
            { LevelWeatherType.Rainy, new MultiplierDictType { ValueMultiplier = RainValueMultiplier, AmountMultiplier = RainAmountMultiplier } },
            { LevelWeatherType.None, new MultiplierDictType { ValueMultiplier = ClearValueMultiplier, AmountMultiplier = ClearAmountMultiplier } },
            { LevelWeatherType.Stormy, new MultiplierDictType { ValueMultiplier = StormValueMultiplier, AmountMultiplier = StormAmountMultiplier } },
            { LevelWeatherType.Flooded, new MultiplierDictType { ValueMultiplier = FloodValueMultiplier, AmountMultiplier = FloodAmountMultiplier } },
            { LevelWeatherType.Eclipsed, new MultiplierDictType { ValueMultiplier = EclipsedValueMultiplier, AmountMultiplier = EclipsedAmountMultiplier } }
        };
    }

    static void ClearOrphanedEntries(ConfigFile cfg)
    {
        ((Dictionary<ConfigDefinition, string>)AccessTools.Property(typeof(ConfigFile), "OrphanedEntries").GetValue(cfg)).Clear();
    }
}