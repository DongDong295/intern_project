using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class EntityMoveBehaviour : EntityBehavior<IEntityData>
{
    private float _moveSpeed;
    IEntityData _entityData;
    protected override UniTask<bool> BuildDataAsync(IEntityData data)
    {
        if (data == null)
        {
            return UniTask.FromResult(false);
        }
        _entityData = data;
        _moveSpeed = data.Speed;
        return UniTask.FromResult(true);
    }


    public void OnUpdate(float deltaTime)
    {
    }

    public void OnFixedUpdate(float deltaTime) {
        if(_entityData.CanMove)
            transform.position = InputManager.Instance.MoveDirection * deltaTime * _moveSpeed;
    }
}
