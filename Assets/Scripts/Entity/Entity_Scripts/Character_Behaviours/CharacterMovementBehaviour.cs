using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.PlayerLoop;

public class CharacterMovementBehaviour : EntityBehavior<IEntityMovementData>, IEntityUpdate
{
    [SerializeField] Rigidbody2D _rb;

    private float _speed;

    private IEntityMovementData _data;
    public override void InitializeData(IEntityMovementData data)
    {
        _data = data;
        _speed = _data.Speed;
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
}
