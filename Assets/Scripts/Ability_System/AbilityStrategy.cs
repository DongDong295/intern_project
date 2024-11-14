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
    protected EntityHolder owner;
    protected CastType castType;
    protected AbilityState state;
    protected AbilityConfigItem abilityData;
    protected float cooldown;
    protected Vector3 castPosition;
    protected List<EntityHolder> hitList;
    protected CancellationTokenSource cts;
    protected IEntityStatsModifyData entityStatsModifyData;

    public virtual async UniTask Init(AbilityConfigItem data, IEntityStatsModifyData modifyData)
    {
        owner = GetComponent<EntityHolder>();
        hitList = new List<EntityHolder>();
        state = AbilityState.Ready;        
        abilityData = data;  
        entityStatsModifyData = modifyData;
        cooldown = abilityData.cooldown;
        castType = abilityData.castType;
        RegisterAbilityCooldow();
        await InitAbility();
        await UniTask.CompletedTask;
    }

    private void RegisterAbilityCooldow()
    {
        switch (abilityData.abilityType)
        {
            case AbilityType.Primary:
                entityStatsModifyData.RegisterBaseStats(EntityStatsType.PrimaryCooldown, cooldown);
                break;
            case AbilityType.AbilityQ:
                entityStatsModifyData.RegisterBaseStats(EntityStatsType.AbilityQCooldown, cooldown);
                break;
            case AbilityType.AbilityE:
                entityStatsModifyData.RegisterBaseStats(EntityStatsType.AbilityECooldown, cooldown);
                break;
            default: break;
        }
    }

    public float GetAbilityCooldown(AbilityType type)
    {
        switch(type){
            case AbilityType.Primary:
                return entityStatsModifyData.GetStats(EntityStatsType.PrimaryCooldown);
            case AbilityType.AbilityQ:
                return entityStatsModifyData.GetStats(EntityStatsType.AbilityQCooldown);
            case AbilityType.AbilityE:
                return entityStatsModifyData.GetStats(EntityStatsType.AbilityECooldown);
            default: return 0;
        }
    }
    protected abstract UniTask InitAbility();

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

    protected virtual async UniTask InitiateAbility() {
        this.state = AbilityState.Using;
        cts?.Cancel();
        cts = new CancellationTokenSource();
        await SetUpInitializeAbility();
    }

    protected abstract UniTask SetUpInitializeAbility();

    protected virtual async UniTask OnFinish() { 
        this.state = AbilityState.Finish;
        await StartAbilityCooldown();
    }

    protected virtual async UniTask StartAbilityCooldown() { 
        this.state = AbilityState.Cooldown;
        await UniTask.Delay(TimeSpan.FromSeconds(GetAbilityCooldown(abilityData.abilityType)), cancellationToken: cts.Token);
        OnFinishCooldown();
    }

    protected void FinishCooldown() { 
        cts?.Cancel();
    }
    protected virtual void OnFinishCooldown() { this.state = AbilityState.Ready; }

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

    public abstract UniTask DamageEnemy();

    public AbilityType GetAbilityType()
    {
        return abilityData.abilityType;
    }
}

public enum AbilityState
{
    Ready,
    Using,
    Finish,
    Cooldown
}