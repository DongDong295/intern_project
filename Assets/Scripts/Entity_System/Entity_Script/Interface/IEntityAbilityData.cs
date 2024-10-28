using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAbilityData : IEntityData
{
    public List<AbilityData> AbilityDatas { get; }
}
