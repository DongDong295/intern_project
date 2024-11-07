using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterDashBehaviour : EntityBehavior<IEntityMovementData, IEntityActionEventData>, IEntityUpdate
{
    [SerializeField] Rigidbody2D _rb;
    private bool _canDash;
    private float _dashCounter;
    private float _dashCooldown;
    private float _dashRange;
    private float _dashSpeed;
    private float _dashDuration;
    private bool _isDashing;
    private IEntityMovementData _movementData;
    private IEntityActionEventData _entityActionEventData;
    private CancellationTokenSource _cts;

    public override async UniTask InitializeData(IEntityMovementData movementData, IEntityActionEventData actionEventData)
    {    
        _movementData = movementData;
        _entityActionEventData = actionEventData;

        _canDash = _movementData.CanDash;
        _dashRange = _movementData.DashRange;
        _dashSpeed = _movementData.DashSpeed;
        _dashDuration = _movementData.DashDuration;
        _dashCounter = _movementData.DashCounter;
        _dashCooldown = _movementData.DashCooldown;

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
        if(action == CharacterInputAction.Dash && _dashCounter > 0)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _isDashing = true;
            _dashCounter -= 1;
            await FinishDash();
            Cooldown().Forget();
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

    async UniTask Cooldown()
    {
        while(_dashCounter < 3)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_dashCooldown), cancellationToken: _cts.Token);
            _dashCounter += 1;
        }
    }

    public void OnDashCounterChange(StatsModifyMessage message)
    {
        if(message.statsType == EntityStatsType.DashCounter)
        {
            _dashCounter += (int)message.modifyValue;
        }
    }

    public override UniTask DeInitialize()
    {
        throw new NotImplementedException();
    }
}
