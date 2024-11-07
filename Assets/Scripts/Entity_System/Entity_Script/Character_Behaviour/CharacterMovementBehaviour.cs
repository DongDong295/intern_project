using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.PlayerLoop;

public class CharacterMovementBehaviour : EntityBehavior<IEntityMovementData, IEntityStatsModifyEventData>, IEntityUpdate
{
    [SerializeField] Rigidbody2D _rb;

    private float _speed;

    private IEntityMovementData _data;
    private IEntityStatsModifyEventData _eventData;
    public override async UniTask InitializeData(IEntityMovementData data, IEntityStatsModifyEventData eventData)
    {
        _data = data;
        _eventData = eventData;
        _speed = _data.Speed;

        eventData.ChangeStatsEvent += OnChangeSpeed;
        await UniTask.FromResult(1);
    }

    public void OnUpdate(float deltaTime)
    {

    }

    public void OnFixedUpdate(float deltaTime)
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        if(_data.CanMove)
            _rb.velocity = InputManager.Instance.InputMovement() * _speed;
    }

    public void OnChangeSpeed(StatsModifyMessage message)
    {
        if(message.statsType == EntityStatsType.Speed)
        {
            _speed += message.modifyValue;
        }
            
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
