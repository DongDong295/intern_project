using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public virtual async UniTask Init(AbilityConfigItem data)
    {
        hitList = new List<EntityHolder>();
        state = AbilityState.Ready;        
        abilityData = data;  
        cooldown = abilityData.cooldown;
        castType = abilityData.castType;
        await UniTask.CompletedTask;
    }

    public virtual async UniTask OnUse() {
        if (this.state == AbilityState.Ready)
        {
            await InitiateAbility();
        }
        else
        {
            Debug.Log(state);
        } 
    }

    public virtual async UniTask InitiateAbility() {
        this.state = AbilityState.Using;
        await SetUpInitializeAbility();
    }

    protected abstract UniTask SetUpInitializeAbility();

    public virtual async UniTask OnFinish() { 
        this.state = AbilityState.Finish;
        await StartAbilityCooldown();
    }

    public virtual async UniTask StartAbilityCooldown() { 
        this.state = AbilityState.Cooldown;
        await UniTask.Delay(TimeSpan.FromSeconds(cooldown));
        OnFinishCooldown();
    }

    public virtual void OnFinishCooldown() { this.state = AbilityState.Ready; }

}

public enum AbilityState
{
    Ready,
    Using,
    Finish,
    Cooldown
}