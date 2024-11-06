using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterModel : IEntityAbilityData
{
    protected AbilityConfig qAbilityConfig;
    protected AbilityConfig eAbilityConfig;
    protected AbilityConfig primaryAbilityConfig;
    public AbilityConfig QAbilityConfig => this.qAbilityConfig;

    public AbilityConfig EAbilityConfig => this.eAbilityConfig;
    public AbilityConfig PrimaryAbilityConfig => this.primaryAbilityConfig;

    public void InitAbilityPrimary(AbilityConfig data)
    {
        primaryAbilityConfig = data;
        Debug.Log(primaryAbilityConfig.items[0].cooldown);
    }

    public void InitAbilityQ(AbilityConfig data)
    {
        qAbilityConfig = data;
        Debug.Log(qAbilityConfig.items[1].cooldown);
    }

    public void InitAbilityE(AbilityConfig data)
    {
        eAbilityConfig = data;
        Debug.Log(eAbilityConfig.items[2].cooldown);
    }
}
