using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityData
{
    public float Cooldown { get; }
    public CastType CastType { get; }

    public AbilityType AbilityType { get; }

    public Target Target { get; }
}

public enum CastType
{
    None,
    SingleCast,
    Multicast,
    Channel
}

public enum AbilityType
{
    None,
    Primary,
    AbilityQ,
    AbilityE,
    Ultimate
}

public enum Target
{
    None,
    Self,
    Other
}