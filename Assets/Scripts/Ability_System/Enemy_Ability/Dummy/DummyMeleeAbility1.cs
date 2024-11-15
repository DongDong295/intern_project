using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMeleeAbility1 : MeleeAbilityStrategy
{
    [SerializeField] MeleeAbilityConfig config;
    
    protected override async UniTask InitAbility()
    {
        entityStatsModifyData.RegisterBaseStats(EntityStatsType.PrimaryDamage, config.dummy[0].abilityDamage);
        entityStatsModifyData.RegisterBaseStats(EntityStatsType.KnockbackForce, config.dummy[0].knockbackForce);
        hitBox.GetComponent<MeleeHitbox>().InitHitbox(this);
        hitBox.SetActive(true);
        await UniTask.CompletedTask;
    }

    protected async override UniTask SetUpInitializeAbility()
    {
        Init(config.dummy[0]);
        await OnFinish();
    }

    protected override async UniTask OnFinish()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
        await DamageEnemy();
        await base.OnFinish();
    }

    public override async UniTask DamageEnemy()
    {
        foreach (var e in hitList) {
            if (hitList != null)
            {
                foreach (var hit in hitList)
                {
                    hit.GetComponent<CharacterHealthBehaviour>().GetHit(new DamageInformation(entityStatsModifyData.GetStats(EntityStatsType.PrimaryDamage), 
                        entityStatsModifyData.GetStats(EntityStatsType.KnockbackForce)));
                }
            }
        }
        await UniTask.CompletedTask;
    }
}
