// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using BepInEx.Configuration;

namespace WeatherGains.Types;

public class MultiValueGroup
{
    public ConfigEntry<float> ValueMultiplier { get; set; }
    public ConfigEntry<float> AmountMultiplier { get; set; }
}