using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : EntityBehavior<IEntityHealthData, IEntityMovementData>
{
    private IEntityHealthData _entityHealthData;
    private IEntityMovementData _entityMovementData;
    [SerializeField] private float _health;
    private bool _isAlive;

    public override async UniTask InitializeData(IEntityHealthData data, IEntityMovementData movement)
    {
        _entityHealthData = data;
        _entityMovementData = movement;
        _health = _entityHealthData.Health;
        _isAlive = data.IsAlive;
        await UniTask.CompletedTask;
    }

    public void GetHit(DamageInformation data, Vector3 ownerPos)
    {
        //Kiem tra buff/Debuff => apply Damage
        TakeDamage(data);
        //ApplyKnockback(force).Forget();
    }

    public void TakeDamage(DamageInformation data)
    {
        _health -= data.Damage;
        if(_health <= 0)
        {
            _isAlive = false;
        }
    }

    public async UniTask ApplyKnockback(Vector3 force)
    {
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
