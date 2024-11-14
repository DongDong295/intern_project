using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Runtime.DataConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCharacterPrimary1 : ProjectileAbilityStrategy
{
    [SerializeField] ProjectileAbilityConfig _projectiledConfig;

    [SerializeField] private GameObject _projectileTestPrefab;

    [SerializeField] private Transform _shootPoint;

    protected override async UniTask InitAbility()
    {
        entityStatsModifyData.RegisterBaseStats(EntityStatsType.PrimaryDamage, _projectiledConfig.items[0].abilityDamage);
        entityStatsModifyData.RegisterBaseStats(EntityStatsType.KnockbackForce, _projectiledConfig.items[0].knockbackForce);
        await UniTask.CompletedTask;
    }

    protected override async UniTask SetUpInitializeAbility()
    {
        await Init(_projectiledConfig.items[0]);

        moveDirection = (InputManager.Instance.CursorPosition() - transform.position).normalized;
        castPosition = transform.position;

        var bullet = Instantiate(_projectileTestPrefab, _shootPoint.position, transform.rotation);
        await bullet.GetComponent<TestCharacterPrimaryProjectile>().InitProjectile(_projectiledConfig.items[0], castPosition, moveDirection, this);
        await OnFinish();
    }
    public override async UniTask DamageEnemy()
    {
        if (hitList != null)
        {
            foreach (var hit in hitList.ToList())
            {
                hit.GetComponent<EnemyHealthBehaviour>().GetHit(new DamageInformation(entityStatsModifyData.GetStats(EntityStatsType.PrimaryDamage), 
                    entityStatsModifyData.GetStats(EntityStatsType.KnockbackForce)), owner.transform.position);
                hitList.Remove(hit);
            }
        }
        await UniTask.CompletedTask;
    }
}
