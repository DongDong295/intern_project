using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterPrimary : AbilityStrategy
{
    protected float cooldown;
    protected CastType castType;
    protected AbilityType abilityType;
    public float Cooldown => cooldown;
    public CastType CastType => castType;
    public AbilityType AbilityType => abilityType;

    public override void OnUse()
    {        
        Debug.Log("Goodjob");
        base.OnUse();
    }
}
