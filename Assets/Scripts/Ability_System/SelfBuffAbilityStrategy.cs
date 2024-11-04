using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBuffAbilityStrategy : AbilityStrategy
{
    protected float speedBuffAmount;
    protected float speedDuration;

    public void Init(SelfBuffAbilityConfigItem config)
    {
        speedBuffAmount = config.speedBuffAmount;
        speedDuration = config.speedDuration;   
    }

    protected override async UniTask SetUpInitializeAbility()
    {
    }
}
