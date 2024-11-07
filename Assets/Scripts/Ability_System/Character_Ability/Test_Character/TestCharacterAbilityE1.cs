using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestCharacterAbilityE1 : SelfBuffAbilityStrategy
{
    [SerializeField] SelfBuffAbilityConfig configs;

    private CancellationTokenSource _cts;
    protected async override UniTask SetUpInitializeAbility()
    {
        Init(configs.items[0]);
        SimpleMessenger.Publish(new StatsModifyMessage(EntityStatsType.Speed, speedBuffAmount));
        await UniTask.Delay(TimeSpan.FromSeconds(speedDuration));
        OnFinish().Forget();
    }
    public override async UniTask OnFinish()
    {
        SimpleMessenger.Publish(new StatsModifyMessage(EntityStatsType.Speed, -speedBuffAmount));
        await base.OnFinish();
        Debug.Log("Finish Cooldown");
    }
}
