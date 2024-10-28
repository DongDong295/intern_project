using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityAbilityData
{
    protected List<AbilityData> abilityDatas;
    
    public List<AbilityData> AbilityDatas => abilityDatas;

    public void InitAbility(List<AbilityData> data)
    {
        abilityDatas = data;
    }
}
