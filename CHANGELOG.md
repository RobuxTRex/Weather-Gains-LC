# WeatherGains Changelog
- A log of every published version in chronological order can be found below.
- Every record includes feature additions, bug fixes, security patches (if applicable), etc.
- Relevant GitHub issues will be mentioned.
- Only releases that are distributed (published to Thunderstore) are recorded here.
- Recording began at version `v2.0.0`. Subsequently, no logs of `v1.x.x` are available.

## v2.0.0
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

### Fixed
- Clear weather multiplier not applying correctly - [#1](https://github.com/RobuxTRex/Weather-Gains-LC/issues/1)