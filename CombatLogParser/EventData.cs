using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatLogParser
{
    public class EventData
    {
        public DateTime Timestamp { get; private set; }
        public string EventName { get; private set; }
        public CombatLogParser CombatLogParser { get; private set; }

        private string[] EventParameters;

        public EventData(DateTime timestamp, string eventName, string[] eventData, CombatLogParser clp)
        {
            this.Timestamp = timestamp;
            this.EventName = eventName;
            this.EventParameters = eventData;
            this.CombatLogParser = clp;
        }

        public string Get(int ind)
        {
            if (ind < EventParameters.Length && ind >= 0)
                return EventParameters[(int)ind];
            else
                throw new IndexOutOfRangeException("Invalid index passed to EventData.Get()");
        }

        // Example function call:
        //        
        // ToString(EventInfo.SPELL_SUMMON.CastSpellId, EventInfo.SPELL_SUMMON.CastSpellName)
        //
        // (7:38:46.299) Event: SPELL_SUMMON
        //          8: 132578
        //          9: Invoke Xuen, the White Tiger
        public string ToString(params int[] paramIndexes)
        {
            List<int> printParamList = new List<int>(paramIndexes);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"({Timestamp.ToCustomString()}) Event: {EventName}");
            int index = 0;
            foreach (string s in EventParameters)
            {
                if (printParamList.Count == 0 || printParamList.Contains(index))
                {
                    string val = s;
                    string unitname = CombatLogParser.UnitNameFromGUID(val);
                    if (unitname != null)
                        val = $"{val} ({unitname})";

                    sb.AppendLine($"\t{index,2}: {val}");
                }
                index++;
            }
            return sb.ToString();
        }
    }
}
