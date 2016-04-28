using System;

namespace CombatLogParser.Examples
{
    public class UnhandledEventFinder
    {
        CombatLogParser _clp;

        public UnhandledEventFinder(string path)
        {
            _clp = new CombatLogParser(path);

            // Will only be raised if the combat log at path has a line with an unrecognized event
            // Mostly used for extending the Events API
            _clp.OnUnhandledEvent += (s, e) =>
            {
                Console.WriteLine("Unhandled Event - " + e.ToString());
            };

            _clp.ParseToEnd();
        }
    }
}
