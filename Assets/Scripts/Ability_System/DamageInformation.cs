using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInformation
{
    public float Damage;
    public float KnockbackForce;
    public EffectApply[] EffectApply;

    public DamageInformation(float damage, float knockbackForce)
    {
        Damage = damage;
        KnockbackForce = knockbackForce;
    }

    public DamageInformation(float damage, float knockbackForce, EffectApply[] effects)
    {
        Damage = damage;
        KnockbackForce = knockbackForce;
        EffectApply = effects;
    }
}

public enum EffectApply{

}
