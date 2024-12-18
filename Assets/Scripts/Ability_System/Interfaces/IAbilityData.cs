using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityData
{
    public CastType CastType { get; }

    public AbilityType AbilityType { get; }

    public Target Target { get; }
}

public enum CastType
{
    None,
    SingleCast = 1,
    Multicast = 2,
    Channel = 3
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