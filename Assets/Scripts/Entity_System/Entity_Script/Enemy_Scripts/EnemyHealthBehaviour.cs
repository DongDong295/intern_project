using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : EntityBehavior<IEntityHealthData>
{
    private IEntityHealthData _entityHealthData;
    [SerializeField] private float _health;
    public override async UniTask InitializeData(IEntityHealthData data)
    {
        _entityHealthData = data;
        _health = _entityHealthData.Health;

        await UniTask.CompletedTask;
    }

    public void GetHit(float damage)
    {
        //Kiem tra buff/Debuff => apply Damage
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
