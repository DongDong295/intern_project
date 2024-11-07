using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public enum EntityStatsType
{
    None,
    Speed,
    DashSpeed,
    DashCounter,
    Health,
    PrimaryCooldown,
    PrimaryDamage,
    AbilityQCooldown,
    AbilityQDamage,
    AbilityECooldown,
    AbilityEDamage
}

public class StatsModifyMessage : IMessage
{
    public EntityStatsType statsType;
    public float modifyValue;
    public AbilityType abilityType;
    public StatsModifyMessage(EntityStatsType type, float value)
    {
        statsType = type;
        modifyValue = value;
    }

    public StatsModifyMessage(EntityStatsType type, float value, AbilityType aType)
    {
        statsType = type;
        modifyValue = value;
        abilityType = aType;
    }
}