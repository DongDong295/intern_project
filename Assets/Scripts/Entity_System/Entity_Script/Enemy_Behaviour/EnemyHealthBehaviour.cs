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
    private Rigidbody2D _rb;

    public override async UniTask InitializeData(IEntityHealthData data, IEntityMovementData movement)
    {
        _entityHealthData = data;
        _entityMovementData = movement;
        _rb = GetComponent<Rigidbody2D>();
        _health = _entityHealthData.Health;
        _isAlive = data.IsAlive;
        await UniTask.CompletedTask;
    }

    public void GetHit(DamageInformation data, Vector3 ownerPos)
    {
        //Kiem tra buff/Debuff => apply Damage
        var force = (-ownerPos + _rb.transform.position).normalized * data.KnockbackForce;
        TakeDamage(data);
        ApplyKnockback(force).Forget();
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
        _entityMovementData.CanMove = false;
        //_entityMovementData.MoveDirection = Vector3.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        _entityMovementData.CanMove = true;
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
