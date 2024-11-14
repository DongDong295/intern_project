using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class TestCharacterAbilityE1 : SelfBuffAbilityStrategy
{
    [SerializeField] SelfBuffAbilityConfig configs;

    private CancellationTokenSource _cts;

    protected override async UniTask InitAbility()
    {
        _cts = new CancellationTokenSource();
        await UniTask.CompletedTask;
    }

    protected async override UniTask SetUpInitializeAbility()
    {
        Init(configs.items[0]);
        cts?.Cancel();
        cts = new CancellationTokenSource();    
        entityStatsModifyData.IncreaseStats(EntityStatsType.PrimaryDamage, primaryBuffAmount);
        entityStatsModifyData.IncreaseStats(EntityStatsType.Speed, speedBuffAmount);
        //SimpleMessenger.Publish(new StatsModifyMessage(EntityStatsType.Speed, speedBuffAmount));
        await UniTask.Delay(TimeSpan.FromSeconds(speedDuration), cancellationToken: _cts.Token);
        OnFinish().Forget();
    }
    protected override async UniTask OnFinish()
    {
        entityStatsModifyData.DecreaseStats(EntityStatsType.PrimaryDamage, primaryBuffAmount);
        entityStatsModifyData.DecreaseStats(EntityStatsType.Speed, speedBuffAmount);
        //SimpleMessenger.Publish(new StatsModifyMessage(EntityStatsType.Speed, -speedBuffAmount));
        await base.OnFinish();
    }
}
