using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAbilityData : IEntityData
{
    public AbilityConfig PrimaryAbilityConfig { get; }
    public AbilityConfig QAbilityConfig { get; }
    public AbilityConfig EAbilityConfig { get; }
}
