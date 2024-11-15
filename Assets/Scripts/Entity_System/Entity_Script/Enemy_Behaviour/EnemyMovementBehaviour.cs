using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehaviour : EntityBehavior<IEntityMovementData, IEntityStatsModifyData, IEntityActionEventData>, IEntityUpdate
{
    private IEntityMovementData _data;
    private IEntityStatsModifyData _stats;
    private IEntityActionEventData _eventData;
    private IMovementStrategy _strategy;

    private float _moveSpeed;

    public override async UniTask InitializeData(IEntityMovementData data, IEntityStatsModifyData stats, IEntityActionEventData eventData)
    {
        _data = data;
        _stats = stats;
        _eventData = eventData;

        _stats.RegisterBaseStats(EntityStatsType.Speed, data.Speed);
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
        _data.CurrentPosition = transform.position;
        if (_data.MoveDirection != Vector3.zero && _data.CanMove)
        {
            Move(deltaTime);
        }
        if (_data.IsReachTarget)
        {
            _eventData.EnemyActionEvent.Invoke(EnemyTriggerAction.Attack);
        }
        _strategy.Update();
    }

    private void Move(float deltaTime)
    {
        // Calculate the target position using MoveDirection and speed
        Vector3 targetPosition = _data.CurrentPosition + _data.MoveDirection.normalized * _moveSpeed * deltaTime;

        // Smooth movement using Lerp for interpolation
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _strategy.FixedUpdate();
    }
}
