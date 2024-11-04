using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAbilityStrategy : AbilityStrategy
{
    protected Collider2D Hitbox;
    protected Vector3 AbilityPosition;
    protected float AbilityDuration;

    protected void Init(AOEAbilityConfigItem item)
    {
        AbilityDuration = item.abilityDuration;
    }

    protected override async UniTask SetUpInitializeAbility()
    {
    }
}
