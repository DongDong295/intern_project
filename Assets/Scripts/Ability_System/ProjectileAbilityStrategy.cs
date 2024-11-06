using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityStrategy : AbilityStrategy
{
    protected float projectileSpeed;
    protected float projectileRange;
    protected float abilityDamage;
    protected bool canPierce;
    protected bool canSearchEnemy;
    protected float searchRange;
    protected GameObject bulletPrefab;
    protected Vector3 moveDirection;

    public virtual async UniTask Init(ProjectileAbilityConfigItem data)
    {
        this.projectileSpeed = data.projectileSpeed;
        this.abilityDamage = data.abilityDamage;
        this.projectileRange = data.projectileRange;
        this.canPierce = data.canPierce;
        this.canSearchEnemy = data.canSearchEnemy;
        this.searchRange = data.searchRange;
        await UniTask.CompletedTask;
    }

    protected override async UniTask SetUpInitializeAbility()
    {

    }
}
