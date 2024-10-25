using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityAbilityData
{
    protected List<AbilityStrategy> abilityStrategies;
    
    public List<AbilityStrategy> AbilityStrategies => abilityStrategies;

    public void InitAbility(List<AbilityStrategy> abilityStrategies)
    {
        abilityStrategies = new List<AbilityStrategy>();
    }
}
