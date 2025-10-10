// WeatherGains © 2025 RobuxTRex/SulphurDev
// AGPL-3.0-or-later – https://www.gnu.org/licenses/agpl-3.0.html
using UnityEngine;

using WeatherGains.Types;

namespace WeatherGains;

public static class MultiplierManager
{
    public static int ApplyValueMultiplier(int initialValue)
    {
        if (!WeatherGains.BoundConfig.ValueMultiEnabled.Value) return initialValue; // Return if value multi isn't enabled
        
        // Retrieves the raw value multiplier for the current weather
        var valueMultiplier = GetMultiplierGroupForCurrentWeather().ValueMultiplier.Value;
        
        // Applies variation if enabled
        if (WeatherGains.BoundConfig.ValueMultiVariation.Value > 0)
        {
            var variationPercent = WeatherGains.BoundConfig.ValueMultiVariation.Value;
            var variationFactor = 1 + (UnityEngine.Random.Range(-variationPercent, variationPercent) / 100f);
            valueMultiplier *= variationFactor;
        }
        
        // Apply the multiplier to the scrap value
        var newValue = (int) Mathf.Max(initialValue * valueMultiplier, 1f); // Ensure at least 1 scrap value
        return newValue;
    }
    
    public static float ApplyAmountMultiplier(float initialAmount)
    {
        if (!WeatherGains.BoundConfig.AmountMultiEnabled.Value) return initialAmount; // Return if amount multi isn't enabled
        
        // Retrieves the amount multiplier for the current weather
        var amountMultiplier = GetMultiplierGroupForCurrentWeather().AmountMultiplier.Value;
        
        // Apply the multiplier to the scrap amount
        var newAmount = Mathf.Max(initialAmount * amountMultiplier, 1f); // Ensure at least 1 scrap is dropped
        return newAmount;
    }
    
    private static MultiValueGroup GetMultiplierGroupForCurrentWeather()
    {
        return WeatherGains.BoundConfig.Multipliers
            [TimeOfDay.Instance.currentLevelWeather];
    }
}