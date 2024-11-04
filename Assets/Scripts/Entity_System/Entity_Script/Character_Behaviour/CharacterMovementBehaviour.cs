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
    public override async UniTask InitializeData(IEntityMovementData data)
    {
        _data = data;
        _speed = _data.Speed;
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

    public void IncreaseCharacterSpeed(float speed)
    {
        _speed += speed;
    }

    public void DecreaseCharacterSpeed(float speed)
    {
        _speed -= speed;
    }
}
