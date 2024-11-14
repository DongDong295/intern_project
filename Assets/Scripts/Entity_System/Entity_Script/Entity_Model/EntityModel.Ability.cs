using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityAbilityData
{
    /*protected AbilityConfig qAbilityConfig;
    protected AbilityConfig eAbilityConfig;
    protected AbilityConfig primaryAbilityConfig;

    public AbilityConfig QAbilityConfig => this.qAbilityConfig;

    public AbilityConfig EAbilityConfig => this.eAbilityConfig;
    public AbilityConfig PrimaryAbilityConfig => this.primaryAbilityConfig;

    public void InitAbilityPrimary(AbilityConfig data)
    {
        primaryAbilityConfig = data;
    }

    public void InitAbilityQ(AbilityConfig data)
    {
        qAbilityConfig = data;
    }

    public void InitAbilityE(AbilityConfig data)
    {
        eAbilityConfig = data;
    }*/
    protected List<AbilityStrategy> abilityStrategy;
    public List<AbilityStrategy> AbilityStrategies => abilityStrategy;
    public void InitAbility()
    {
        abilityStrategy = new List<AbilityStrategy>();
    }
}
