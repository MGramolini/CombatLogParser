using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CombatLogParser
{
    public class CombatLogParser
    {
        // This class contains the majority of the functionality for this library.
        // When Parse() is called, it will read the file at filepath given in the
        // constructor and parse the file to its end, one line at a time. Once
        // a line is parsed, any custom event registered by the user using
        // RegisterEvent() is raised. See Events.cs and EventInfo.cs for more
        // about events.

        public bool IsParsing { get; private set; } = true;
        public float ParseCompletionPercent { get; private set; } = 0f;
        public FileInfo FileInfo { get; private set; }

        //These events provide access to the stream which parses combat log before and after parsing, used in Examples/LiveParsing
        public event EventHandler<FileStream> PreParseEvent, PostParseEvent;
        //Raised when CombatLogParser can't find necessary EventInfo - See EventInfo.cs for information on extending the API
        public event EventHandler<EventData> OnUnhandledEvent;

        private Dictionary<string, string> GuidToUnitName;
        private Dictionary<int, string> AuraIdToSpellName;
        private Dictionary<string, EventHandler<EventData>> CustomEvents;

        public CombatLogParser(string filepath)
        {
            IsParsing = false;

            GuidToUnitName = new Dictionary<string, string>();
            CustomEvents = new Dictionary<string, EventHandler<EventData>>();
            AuraIdToSpellName = new Dictionary<int, string>();

            if (File.Exists(filepath))
            {
                FileInfo = new FileInfo(filepath);
            }
            else
                throw new FileNotFoundException("Combat Log not found", filepath);
            
        }
        
        /// <summary>
        /// Registers an event handler for each event in events - This is encountered
        /// </summary>
        /// <param name="OnEvent">The event handler to be raised</param>
        /// <param name="events">A collection of events for which the event handler will be raised</param>
        public void RegisterEvent(EventHandler<EventData> OnEvent, params string[] events)
        {
            if (events != null)
            {
                foreach (string s in events)
                {
                    if (!CustomEvents.ContainsKey(s))
                        CustomEvents.Add(s, null);

                    CustomEvents[s] += OnEvent;
                }
            }
        }

        /// <summary>
        /// Returns the name of a unit based on the given guid, if CombatLogParser hasn't parsed a line with this unit's information it will return null.
        /// </summary>
        public string UnitNameFromGUID(string guid)
        {
            return GuidToUnitName.ContainsKey(guid) ? GuidToUnitName[guid] : null;
        }
        /// <summary>
        /// Returns the name of a spell based on the given aura spell Id, if CombatLogParser hasn't parsed a line with this aura's information it will return null.
        /// </summary>
        public string SpellNameFromAuraId(int auraId)
        {
            return AuraIdToSpellName.ContainsKey(auraId) ? AuraIdToSpellName[auraId] : null;
        }

        /// <summary>
        /// Calls ParseLine() on each line of the file passed to the CombatLogParser's constructor
        /// Use PreParseEvent and PostParseEvent for external access to the FileStream before
        /// and after ParseLine is called - see Examples/LiveParsing.cs for a potential use case
        /// </summary>
        public void ParseToEnd()
        {
            ParseCompletionPercent = 0f;
            IsParsing = true;

            using (FileStream fs = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    PreParseEvent?.Invoke(this, fs);
                    long startPos = fs.Position;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        EventData evt = ParseLine(line);
                        HandleEvent(evt);

                        long cur = fs.Position;
                        long total = fs.Length;
                        long deltaCur = cur - startPos;
                        long deltaTotal = total - startPos;

                        ParseCompletionPercent = (float)deltaCur / (float)deltaTotal;
                    }

                    PostParseEvent?.Invoke(this, fs);
                }
            }
            IsParsing = false;
        }
        // Parses an entire line from the combat log,
        // splitting it into 3 parts - timestamp, eventName,
        // and pre-split eventParamaters as a single string
        private EventData ParseLine(string line)
        {
            Regex r = new Regex(@"(\d{1,2})/(\d{1,2})\s(\d{2}):(\d{2}):(\d{2}).(\d{3})\s\s(\w+),(.+)$"); //matches the date format used in the combat log
            Match m = r.Match(line);
            GroupCollection collection = m.Groups;

            if (collection.Count != 9)
            {
                Exception e = new Exception("Error parsing line");
                e.Data["line"] = line;
                throw e;
            }

            string month = collection[1].Value;
            string day = collection[2].Value;
            string hour = collection[3].Value;
            string minute = collection[4].Value;
            string second = collection[5].Value;
            string millisecond = collection[6].Value;

            string evt = collection[7].Value;
            string data = collection[8].Value;

            DateTime time;

            //This should never error, as the date format is expected to be identical every time
            time = new DateTime(DateTime.Now.Year, int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), int.Parse(millisecond));

            EventData evtData = new EventData(time, evt, ParseEventParameters(data), this);
            return evtData;
        }
        private string[] ParseEventParameters(string unsplitParameters)
        {
            //Because the combat log can have lines like the following, we need to do a custom parse as opposed to a comma split
            //This custom parse will ignore commas that are inside quotes, and will also remove quotation marks from single values
            //ex "\"Invoke Xuen, the White Tiger\"" becomes "Invoke Xuen, the White Tiger"
            //4/9 07:38:46.299  SPELL_SUMMON,Player-61-07B7D5D6,"Kildonne-Zul'jin",0x511,0x0,Creature-0-3019-1153-26151-73967-000008E9BF,"Xuen",0xa28,0x0,132578,"Invoke Xuen, the White Tiger",0x8

            List<string> dataList = new List<string>();
            int index = 0;
            bool inquote = false;
            int startIndex = 0;
            while (index <= unsplitParameters.Length)
            {
                if (index == unsplitParameters.Length)
                {
                    dataList.Add(unsplitParameters.Substring(startIndex, index - startIndex));
                    break;
                }

                if (unsplitParameters[index] == '"')
                {
                    inquote = !inquote;
                }
                else if (unsplitParameters[index] == ',')
                {
                    if (!inquote)
                    {
                        string s = unsplitParameters.Substring(startIndex, index - startIndex);
                        if (s[0] == '"' && s[s.Length - 1] == '"')
                            s = s.Substring(1, s.Length - 2);
                        dataList.Add(s);
                        startIndex = index + 1;
                    }
                }
                index++;
            }

            return dataList.ToArray();
        }

        //Events used internally by the CombatLogParser to provide utility functions to the user
        private void RegisterInternalEvents()
        {
            //Populates AuraIdToSpellName, used to convert spell Ids to spell names, see SpellNameFromAuraId(int)
            this.RegisterEvent((s, e) =>
            {
                int spellId = e.Get(EventInfo.SPELL_AURA_APPLIED.AuraSpellId).ToInt();
                string spellName = e.Get(EventInfo.SPELL_AURA_APPLIED.AuraSpellName);
                if (!AuraIdToSpellName.ContainsKey(spellId))
                    AuraIdToSpellName.Add(spellId, spellName);
            },
            Events.SPELL_AURA_APPLIED,
            Events.SPELL_AURA_REMOVED);
        }

        //Handles an event internally after it's been parsed
        private void HandleEvent(EventData e)
        {
            //Since it is assumed every event we have EventInfo for has a variable 'HasUnitKeys',
            //we know that if hasGenKeys is null, we don't have any EventInfo and should raise
            //this as an UnhandledEvent
            object hasUnitKeys = EventInfo.Get.FromString(e.EventName, "HasUnitKeys");
            if (hasUnitKeys != null)
            {
                if ((bool)hasUnitKeys)
                {
                    HandleUnitKeys(e);
                }
            }
            else
            {
                UnhandledEvent(e);
            }

            //Raise any user-defined events
            if (CustomEvents.ContainsKey(e.EventName))
            {
                CustomEvents[e.EventName](this, e);
            }
        }
        //Populates GuidToUnitName for getting a potentially non-unique unit name from its guid 
        private void HandleUnitKeys(EventData e)
        {
            string sourceGUID = e.Get(EventInfo.UnitKeys.SourceGUID);
            string sourceName = e.Get(EventInfo.UnitKeys.SourceName);

            if (!GuidToUnitName.ContainsKey(sourceGUID))
                GuidToUnitName.Add(sourceGUID, sourceName);

            string destGUID = e.Get(EventInfo.UnitKeys.DestGUID);
            string destName = e.Get(EventInfo.UnitKeys.DestName);

            if (!GuidToUnitName.ContainsKey(destGUID))
                GuidToUnitName.Add(destGUID, destName);
        }
        //Called when CombatLogParser encounters an event it can't find expected information about
        private void UnhandledEvent(EventData e)
        {
            OnUnhandledEvent?.Invoke(this, e);
        }
    }
}
