using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityAbilityData
{
    protected AbilityConfig qAbilityConfig;
    protected AbilityConfig primaryAbilityConfig;
    public AbilityConfig QAbilityConfig => this.qAbilityConfig;
    public AbilityConfig PrimaryAbilityConfig => this.primaryAbilityConfig;

    public void InitAbilityQ(AbilityConfig data)
    {
        qAbilityConfig = data;
        Debug.Log(qAbilityConfig.items[1].cooldown);
    }
    public void InitAbilityPrimary(AbilityConfig data)
    {
        primaryAbilityConfig = data;
        Debug.Log(primaryAbilityConfig.items[0].cooldown);
    }
}
