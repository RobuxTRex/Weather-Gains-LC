# WeatherGains Changelog
- A log of every published version in chronological order can be found below.
- Every record includes feature additions, bug fixes, security patches (if applicable), etc.
- Relevant GitHub issues will be mentioned.
- Only releases that are distributed (published to Thunderstore) are recorded here.

---

## v2.0.1
**Target Version: v73**

- Removed the item value and item quantity clamp in the multiplier applier - [#2](https://github.com/RobuxTRex/Weather-Gains-LC/issues/2)

## v2.0.0
**Target Version: v73**

Complete rewrite of WeatherGains.

### Added
- Bee hives are now impacted by scrap value multipliers.
- Safety guards to ensure scrap value is at least `1`.
- Compatibility Mode in config which guarantees compatibility with all other mods at the cost of disabling bee hive multipliers (any transpilers).
- Config Version field to config, allowing future versions to allow backwards compatibility and migration to new formats.
- Clear security policy, changelog, code of conduct, and CLA for maximum transparency.

### Removed
- Host syncing has been removed because WeatherGains is now a **purely server-side mod**!

### Changes
- **WeatherGains has been relicensed under the AGPL-3.0-or-later!**
- Should be compatible with other mods that modify the moon scrap value multiplier due to modifications on how scrap multipliers are applied.
- Better README!

### Fixes
- Clear weather multiplier not applying correctly - [#1](https://github.com/RobuxTRex/Weather-Gains-LC/issues/1)

---

## v1.1.0
**Target Version: v67**

General good changes this time around! I have no idea what to add now.
Besides from minor changes + bug fixes, this could be the final major update!

### Added
- 'Value Multiplier Enabled' configuration setting added, which allows you to toggle the scrap 
value multipliers throughout the mod.
- 'Amount Multiplier Enabled' configuration setting added, which allows you to toggle the scrap 
amount multipliers throughout the mod.
- 'Apparatus Multiplier Enabled' configuration setting added, which allows you to toggle the scrap 
value multiplier for the apparatus prop.

### Removed
- 'Enabled' configuration setting, which allowed you toggle the mod. It has been replaced with
three separate settings for value/amount/apparatus multipliers. You may also disable the mod
in mod managers to entirely disable functionality.

### Changes
- The Apparatus (Lung) prop is now affected by the value multiplier (toggleable in the configuration).

### Fixes
- Fixed an issue where the multiplier would add up over time, eventually leading to scrap worth
thousands of credits.
- Fixed an issue where the mod would completely overwrite the moon's default scrap value
and amount multipliers. New implementation of how the mod applies the multipliers fixed
this issue and also improved compatibility with other mods.

---

## v1.0.0
**Target Version: v65**

First release!