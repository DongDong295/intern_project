using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBuffAbilityStrategy : AbilityStrategy { 
    protected float speedBuffAmount;
    protected float speedDuration;
    protected float primaryBuffAmount;
    protected float primaryBuffDuration;

    public void Init(SelfBuffAbilityConfigItem config)
    {
        speedBuffAmount = config.speedBuffAmount;
        speedDuration = config.speedDuration;
        primaryBuffAmount = config.primaryBuffAmount;
        primaryBuffDuration = config.primaryDuration;
    }

    public override async UniTask InitAbility()
    {
        await UniTask.CompletedTask;
    }

    public override async UniTask DamageEnemy()
    {
        await UniTask.CompletedTask;
    }

    protected override async UniTask SetUpInitializeAbility()
    {
        await UniTask.CompletedTask;
    }
}
