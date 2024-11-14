using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityStrategy : AbilityStrategy
{
    public float projectileSpeed;
    public float projectileRange;
    public float abilityDamage;
    public bool canPierce;
    public bool canSearchEnemy;
    public float knockbackForce;
    protected float searchRange;
    protected GameObject bulletPrefab;
    protected Vector3 moveDirection;

    public virtual async UniTask Init(ProjectileAbilityConfigItem data)
    {
        this.projectileSpeed = data.projectileSpeed;
        this.abilityDamage = entityStatsModifyData.GetStats(EntityStatsType.PrimaryDamage);
        this.projectileRange = data.projectileRange;
        this.canPierce = data.canPierce;
        this.canSearchEnemy = data.canSearchEnemy;
        this.searchRange = data.searchRange;
        this.knockbackForce = data.knockbackForce;
        await UniTask.CompletedTask;
    }

    protected override async UniTask InitAbility()
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
