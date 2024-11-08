using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityStrategy : AbilityStrategy
{
    public override async UniTask DamageEnemy()
    {
        await UniTask.CompletedTask;
    }

    public override async UniTask InitAbility()
    {
        await UniTask.CompletedTask;
    }

    protected override async UniTask SetUpInitializeAbility()
    {
    }
}
