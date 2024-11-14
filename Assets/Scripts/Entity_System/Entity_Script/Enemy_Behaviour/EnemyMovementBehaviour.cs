using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehaviour : EntityBehavior<IEntityMovementData, IEntityStatsModifyData>, IEntityUpdate
{
    private IEntityMovementData _data;
    private IEntityStatsModifyData _stats;
    private Rigidbody2D _rb;

    private IMovementStrategy _strategy;

    private float _moveSpeed;

    public override async UniTask InitializeData(IEntityMovementData data, IEntityStatsModifyData stats)
    {
        _data = data;
        _stats = stats;
        _stats.RegisterBaseStats(EntityStatsType.Speed, data.Speed);
        _rb = GetComponent<Rigidbody2D>();
        _moveSpeed = _stats.GetStats(EntityStatsType.Speed);

        _strategy = new EnemyNormalChaseStrategy(stats, data);
        await UniTask.CompletedTask;
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate(float deltaTime)
    {
        _data.CurrentPosition = _rb.transform.position;
        _strategy.Update();
    }

    private void Move(float deltaTime)
    {
        Vector3 targetPosition = _data.CurrentPosition + _data.MoveDirection.normalized * _moveSpeed * deltaTime;
        _rb.MovePosition(Vector3.MoveTowards(_rb.position, targetPosition, _moveSpeed * deltaTime));
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (_data.MoveDirection != Vector3.zero && _data.CanMove)
        {
            Move(fixedDeltaTime);
        }
        _strategy.FixedUpdate();
    }
}
