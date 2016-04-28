using System;
using System.Reflection;

namespace CombatLogParser.EventInfo
{
    // For any given event, the corresponding class in this file contains the 
    // indexes for variables in EventData.EventParameters
    //
    // Each event has a bool, HasUnitKeys - if true, that event's first 8
    // EventParameters will be the same, as found in EventInfo.UnitKeys
    //
    // CombatLogParser expects that each combat log event (Events.cs) has a class
    // in this file, and that each event in this file has "bool HasUnitKeys"
    //
    // When adding new events to this file, keep variable names consistent with
    // other events which have the same parameter (regardless of value). This
    // is because EventInfo.Get.FromString can be used to get variables with the
    // same name from different classes. For example, a call to 
    // CombatLogParser.RegisterEvent() with SPELL_AURA_APPLIED and 
    // SPELL_AURA_REMOVED can use 
    // Get.FromString(eventName, "AuraSpellId").ToInt() to retrieve the value of
    // AuraSpellId from the event, regardless of which event was raised

    public static class Get
    {
        public static object FromString(string eventName, string fieldName)
        {
            Type t = Type.GetType("CombatLogParser.EventInfo." + eventName);
            if (t != null)
            {
                FieldInfo info = t.GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
                if (info != null)
                {
                    return info.GetValue(null);
                }
            }
            return null;
        }
    }
    
    public class UnitKeys
    {
        public const int SourceGUID = 0;
        public const int SourceName = 1;
        public const int SourceFlags = 2;
        public const int SourceFlags2 = 3;
        public const int DestGUID = 4;
        public const int DestName = 5;
        public const int DestFlags = 6;
        public const int DestFlags2 = 7;
    }

    public class ENCOUNTER_START
    {
        public const int EncounterId = 0;
        public const int EncounterName = 1;
        public const int DifficultyId = 2;
        public const int RaidSize = 3;
        public const bool HasUnitKeys = false;
    }

    public class ENCOUNTER_END
    {
        public const int EncounterId = 0;
        public const int EncounterName = 1;
        public const int DifficultyId = 2;
        public const int RaidSize = 3;
        public const int Wiped = 4;
        public const bool HasUnitKeys = false;
    }

    public class SPELL_CAST_SUCCESS
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_CAST_START
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = false;
    }

    public class SPELL_CAST_FAILED
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int CastFailedReason = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_ENERGIZE
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int ResourceGain = 24;
        public const int ResourceType = 25;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_PERIODIC_ENERGIZE //unsure of 17
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int ResourceGain = 24;
        public const int ResourceType = 25;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_SUMMON
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_APPLIED
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_APPLIED_DOSE
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const int AuraDosesAdded = 12;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_REMOVED
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_REFRESH
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_REMOVED_DOSE
    {
        public const int AuraSpellId = 8;
        public const int AuraSpellName = 9;
        public const int AuraSpellSchool = 10;
        public const int AuraBuffType = 11;
        public const int AuraDosesRemoved = 12;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_AURA_BROKEN_SPELL
    {
        public const int CastAuraSpellId = 8;
        public const int CastAuraSpellName = 9;
        public const int CastAuraSpellSchool = 10;
        public const int RemovedAuraSpellId = 11;
        public const int RemovedAuraSpellName = 12;
        public const int RemovedAuraSpellSchool = 13;
        public const int CastAuraBuffType = 14;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_MISSED //12-13 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int MissedReason = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_DAMAGE //27-33 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int DamageDone = 24;
        public const int Overkill = 25;
        public const int DamageSpellSchool = 26;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_PERIODIC_DAMAGE //27-33 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int DamageDone = 24;
        public const int Overkill = 25;
        public const int DamageSpellSchool = 26;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_PERIODIC_MISSED //12-14 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int MissedReason = 11;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_ABSORBED //8-15 unknown
    {
        public const bool HasUnitKeys = true;
    }

    public class SPELL_HEAL //27-28 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int HealAmount = 24;
        public const int Overheal = 25;
        public const int HealSpellSchool = 26;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_PERIODIC_HEAL //27-28 unknown
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int HealAmount = 24;
        public const int Overheal = 25;
        public const int HealSpellSchool = 26;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_CREATE
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_INTERRUPT
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int InterruptedSpellId = 11;
        public const int InterruptedSpellName = 12;
        public const int InterruptedSpellSpellSchool = 13;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_RESURRECT
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = true;
    }

    public class SPELL_INSTAKILL
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const bool HasUnitKeys = true;
    }

    public class RANGE_DAMAGE //unknown 26-33
    {
        public const int CastSpellId = 8;
        public const int CastSpellName = 9;
        public const int CastSpellSchool = 10;
        public const int TargetGUID = 11;
        public const int TargetGUIDOwner = 12;
        public const int TargetHP = 13;
        public const int TargetMaxHP = 14;
        public const int TargetAttackPower = 15;
        public const int TargetSpellPower = 16;
        public const int TargetResolve = 17;
        public const int CastSpellResource = 18;
        public const int CurrentResource = 19;
        public const int MaxResource = 20;
        public const int TargetPosX = 21;
        public const int TargetPosY = 22;
        public const int TargetItemLvl = 23;
        public const int DamageDone = 24;
        public const int Overkill = 25;
        //26 is probably damage spell school
        public const bool HasUnitKeys = true;
    }

    public class SWING_DAMAGE //24-30 unknown
    {
        public const int TargetGUID = 8;
        public const int TargetGUIDOwner = 9;
        public const int TargetHP = 10;
        public const int TargetMaxHP = 11;
        public const int TargetAttackPower = 12;
        public const int TargetSpellPower = 13;
        public const int TargetResolve = 14;
        public const int CastSpellResource = 15;
        public const int CurrentResource = 16;
        public const int MaxResource = 17;
        public const int TargetPosX = 18;
        public const int TargetPosY = 19;
        public const int TargetItemLvl = 20;
        public const int DamageDone = 21;
        public const int Overkill = 22;
        public const int DamageSpellSchool = 23;
        public const bool HasUnitKeys = true;
    }
    public class SWING_DAMAGE_LANDED //24-30 unknown
    {
        public const int TargetGUID = 8;
        public const int TargetGUIDOwner = 9;
        public const int TargetHP = 10;
        public const int TargetMaxHP = 11;
        public const int TargetAttackPower = 12;
        public const int TargetSpellPower = 13;
        public const int TargetResolve = 14;
        public const int CastSpellResource = 15;
        public const int CurrentResource = 16;
        public const int MaxResource = 17;
        public const int TargetPosX = 18;
        public const int TargetPosY = 19;
        public const int TargetItemLvl = 20;
        public const int DamageDone = 21;
        public const int Overkill = 22;
        public const int DamageSpellSchool = 23;
        public const bool HasUnitKeys = true;
    }
    public class SWING_MISSED //9-11 missing
    {
        public const int MissedReason = 8;
        public const bool HasUnitKeys = true;
    }

    public class UNIT_DIED
    {
        public const int UnitGUID = 4;
        public const int UnitName = 5;
        public const int UnitSourceFlags = 6;
        public const int UnitSourceFlags2 = 7;
        public const bool HasUnitKeys = true;
    }

    public class UNIT_DESTROYED
    {
        public const int UnitGUID = 4;
        public const int UnitName = 5;
        public const int UnitFlags = 6;
        public const int UnitFlags2 = 7;
        public const bool HasUnitKeys = true;
    }

    public class PARTY_KILL
    {
        public const int FriendlyGUID = 0;
        public const int FriendlyName = 1;
        public const int FriendlyFlags = 2;
        public const int FriendlyFlags2 = 3;
        public const int EnemyGUID = 4;
        public const int EnemyName = 5;
        public const int EnemyFlags = 6;
        public const int EnemyFlags2 = 7;
        public const bool HasUnitKeys = true;
    }

    public class ENVIRONMENTAL_DAMAGE //22-31 unknown
    {
        public const int TargetGUID = 8;
        public const int TargetGUIDOwner = 9;
        public const int TargetHP = 10;
        public const int TargetMaxHP = 11;
        public const int TargetAttackPower = 12;
        public const int TargetSpellPower = 13;
        public const int TargetResolve = 14;
        public const int CastSpellResource = 15;
        public const int CurrentResource = 16;
        public const int MaxResource = 17;
        public const int TargetPosX = 18;
        public const int TargetPosY = 19;
        public const int TargetItemLvl = 20;
        public const int DamageName = 21;
        public const bool HasUnitKeys = true;
    }

    public class RANGE_MISSED //12-13 unknown
    {
        public const int SpellId = 8;
        public const int SpellName = 9;
        public const int SpellSchool = 10;
        public const int MissedReason = 11;
        public const bool HasUnitKeys = true;
    }
}// namespace CombatLogEvent.EventInfo
