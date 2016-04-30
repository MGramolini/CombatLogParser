# CombatLogParser
A C# library for parsing the World of Warcraft combat log

#The Combat Log
The world of warcraft combat log is a line-separated file containing information about combat related events that occur in the game. This data can be aggregated to show statistics about an individual player, or about the overall balance of the game as a whole. This web tool demonstrates what kind of data can be collected from the combat log: https://www.warcraftlogs.com/statistics/8

A small sample of the combat log can be found in ExampleCombatLog.txt

To use CombatLogParser, you must first register event handlers to be raised when a certain combat log event is found while parsing. For example, to get a callback when any SPELL_AURA_APPLIED event occurs, call CombatLogParser.RegisterEvent(handler, Events.SPELL_AURA_APPLIED) - more in depth examples can be found in the Examples folder.

To extend the API with new events, see the documentation in EventInfo.cs
