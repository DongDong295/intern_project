using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashBehaviour : EntityBehavior<IEntityMovementData, IEntityActionEventData>, IEntityUpdate
{
    [SerializeField] Rigidbody2D _rb;

    private bool _canDash;
    private float _dashRange;
    private float _dashSpeed;
    private float _dashDuration;
    private bool _isDashing;
    private IEntityMovementData _movementData;
    private IEntityActionEventData _entityActionEventData;

    public override async UniTask InitializeData(IEntityMovementData movementData, IEntityActionEventData actionEventData)
    {    
        Debug.Log("Xx");
        _movementData = movementData;
        _entityActionEventData = actionEventData;

        _canDash = _movementData.CanDash;
        _dashRange = _movementData.DashRange;
        _dashSpeed = _movementData.DashSpeed;
        _dashDuration = _movementData.DashDuration;

        _isDashing = false;
        actionEventData.ActionEvent += OnDash;
        await UniTask.FromResult(1);
    }

    public void OnUpdate(float deltaTime)
    {
    }

    public void OnFixedUpdate(float deltaTime)
    {
        Dash();
    }

    async void OnDash(CharacterInputAction action)
    {
        if(action == CharacterInputAction.Dash)
        {
            Debug.Log("Dash");
            _isDashing = true;
            await FinishDash();
        }
    }

    void Dash()
    {
        if (_isDashing)
        {
            _rb.MovePosition(transform.position + InputManager.Instance.InputMovement() * _dashSpeed * _dashRange * Time.fixedDeltaTime);
        }
    }

    async UniTask FinishDash()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_dashDuration));
        _isDashing = false;
    }
}
