using BepInEx.Configuration;

namespace WeatherGains.Types;

public class MultiValueGroup
{
    public ConfigEntry<float> ValueMultiplier { get; set; }
    public ConfigEntry<float> AmountMultiplier { get; set; }
}