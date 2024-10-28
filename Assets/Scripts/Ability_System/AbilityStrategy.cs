using Cysharp.Threading.Tasks;
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

public class AbilityStrategy : MonoBehaviour, IAbilityStrategy
{
    private CastType _castType;
    private AbilityState _state;
    private IAbilityData _abilityData;
    private float _cooldown;

    public virtual async UniTask Init(IAbilityData data)
    {
        _state = AbilityState.Ready;        
        _abilityData = data;  
        _cooldown = _abilityData.Cooldown;
        _castType = _abilityData.CastType;
        await UniTask.CompletedTask;
    }

    public virtual async UniTask OnUse() {
        if (_state == AbilityState.Ready)
        {
            await InitiateAbility();
        }
        else
            return;
    }

    public virtual async UniTask InitiateAbility() { _state = AbilityState.Using; await UniTask.CompletedTask; }

    public virtual async UniTask OnFinish() { 
        _state = AbilityState.Finish;
        await StartAbilityCooldown();
    }

    public virtual async UniTask StartAbilityCooldown() { 
        _state = AbilityState.Cooldown;
        await UniTask.Delay(TimeSpan.FromSeconds(_cooldown));
        OnFinishCooldown();
    }

    public virtual void OnFinishCooldown() { _state = AbilityState.Ready; }

}
public enum AbilityState
{
    Ready,
    Using,
    Finish,
    Cooldown
}