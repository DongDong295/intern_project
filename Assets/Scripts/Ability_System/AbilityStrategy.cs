using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AbilityStrategyFactory
{
    /*public static IAbilityStrategy CreateStrategy(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.None:
                break;
            case AbilityType.Primary:
                return new AbilityStrategy();
            case AbilityType.AbilityQ:
                break;
            case AbilityType.AbilityE:
                break;
            case AbilityType.Ultimate:
                break;
            default:
                break;
        }
        return null;
    }*/
}

public abstract class AbilityStrategy : MonoBehaviour, IAbilityStrategy
{
    protected CastType castType;
    protected AbilityState state;
    protected AbilityConfigItem abilityData;
    protected float cooldown;
    protected Vector3 castPosition;
    protected List<EntityHolder> hitList;
    protected CancellationTokenSource cts;

    public virtual async UniTask Init(AbilityConfigItem data)
    {
        hitList = new List<EntityHolder>();
        state = AbilityState.Ready;        
        abilityData = data;  
        cooldown = abilityData.cooldown;
        castType = abilityData.castType;
        await UniTask.CompletedTask;
    }

    public virtual void DeInitialize()
    {
        cts?.Cancel();
    }

    public virtual async UniTask OnUse() {
        if (this.state == AbilityState.Ready)
        {
            await InitiateAbility();
        }
        else
        {
        } 
    }

    public virtual async UniTask InitiateAbility() {
        this.state = AbilityState.Using;
        cts?.Cancel();
        cts = new CancellationTokenSource();
        StartAbilityCooldown().Forget();
        await SetUpInitializeAbility();
    }

    protected abstract UniTask SetUpInitializeAbility();

    public virtual async UniTask OnFinish() { 
        this.state = AbilityState.Finish;
    }

    public virtual async UniTask StartAbilityCooldown() { 
        this.state = AbilityState.Cooldown;
        await UniTask.Delay(TimeSpan.FromSeconds(cooldown), cancellationToken: cts.Token);
        OnFinishCooldown();
    }

    public void FinishCooldown() { 
        cts?.Cancel();
    }
    public virtual void OnFinishCooldown() { this.state = AbilityState.Ready; }

    public virtual void AddHitList(EntityHolder enemy) { 
        hitList.Add(enemy);
    }

    public virtual void RemoveHitList(EntityHolder enemy)
    {
        if (hitList.Contains(enemy))
        {
            hitList.Remove(enemy);
        }
    }
}

public enum AbilityState
{
    Ready,
    Using,
    Finish,
    Cooldown
}