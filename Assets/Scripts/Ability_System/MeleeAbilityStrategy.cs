using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityStrategy : AbilityStrategy
{
    [SerializeField] protected GameObject hitBox;
    public float AbilityDamage;
    public override async UniTask DamageEnemy()
    {
        await UniTask.CompletedTask;
    }
    protected void Init(MeleeAbilityConfigItem item)
    {
        //entityStatsModifyData.RegisterBaseStats(EntityStatsType.PrimaryDamage, item.abilityDamage);
        AbilityDamage = entityStatsModifyData.GetStats(EntityStatsType.PrimaryDamage);
    }

    protected override async UniTask InitAbility()
    {
        await UniTask.CompletedTask;
    }

    protected override async UniTask SetUpInitializeAbility()
    {
        await UniTask.CompletedTask;
    }
}
