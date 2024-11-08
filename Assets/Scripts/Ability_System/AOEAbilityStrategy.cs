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
    protected float AbilityDamage;

    public override async UniTask DamageEnemy()
    {
        await UniTask.CompletedTask;
    }

    public override async UniTask InitAbility()
    {
        await UniTask.CompletedTask;
    }

    protected void Init(AOEAbilityConfigItem item)
    {
        AbilityDuration = item.abilityDuration;
        AbilityDamage = entityStatsModifyData.GetStats(EntityStatsType.AbilityQDamage);
    }

    protected override async UniTask SetUpInitializeAbility()
    {
        await UniTask.CompletedTask;
    }
}
