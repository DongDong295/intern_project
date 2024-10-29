using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Runtime.DataConfig;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterAbilityQ1 : AOEAbilityStrategy
{
    public float _mollyMoveSpeed;

    [SerializeField] AOEAbilityConfig config;

    [SerializeField] Collider2D _hitbox;

    [SerializeField] GameObject _mollyPrefab;

    private GameObject _molly;
    private float _arcHeight;

    public override async UniTask InitiateAbility()
    {
        _molly = Instantiate(_mollyPrefab, transform.position, transform.rotation);
        _mollyMoveSpeed = 1;
        _arcHeight = 3;
        await MoveMollyInArc(InputManager.Instance.CursorPosition());
        await OnFinish();
    }
    public async UniTask MoveMollyInArc(Vector3 targetPos)
    {
        Vector3 startPos = _molly.transform.position;
        float time = 0f;
        float duration = 1f;

        while (time < duration)
        {
            time += Time.deltaTime * _mollyMoveSpeed;
            float normalizedTime = Mathf.Clamp01(time / duration);

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, normalizedTime);
            float arc = _arcHeight * Mathf.Sin(Mathf.PI * normalizedTime);
            currentPos.y += arc;

            _molly.transform.position = currentPos;

            await UniTask.Yield();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
    }

}
