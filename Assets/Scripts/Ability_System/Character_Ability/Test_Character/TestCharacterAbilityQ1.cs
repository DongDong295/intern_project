using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterAbilityQ1 : AOEAbilityStrategy
{
    [SerializeField] private AOEAbilityConfig config;
    [SerializeField] private GameObject _mollyPrefab;
    [SerializeField] private GameObject _mollyFieldPrefab;

    private GameObject _molly;
    private float _mollyMoveSpeed = 3.5f;
    private float _arcHeight = 3f;
    private Vector3 _originalScale;
    private Vector3 _targetScale = new Vector3(5f, 5f, 0f);

    protected async override UniTask SetUpInitializeAbility()
    {
        Init(config.items[0]);
        _molly = Instantiate(_mollyPrefab, transform.position, transform.rotation);
        await OnFinish();
        await MoveMollyInArc(InputManager.Instance.CursorPosition());
    }

    public async UniTask MoveMollyInArc(Vector3 targetPos)
    {
        Vector3 startPos = _molly.transform.position;
        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime * _mollyMoveSpeed;
            float normalizedTime = Mathf.Clamp01(timeElapsed / duration);

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, normalizedTime);
            float arc = _arcHeight * Mathf.Sin(Mathf.PI * normalizedTime);
            currentPos.y += arc;

            _molly.transform.position = currentPos;
            await UniTask.Yield();
        }

        var _activeField = Instantiate(_mollyFieldPrefab, _molly.transform.position, transform.rotation);
        _activeField.GetComponent<MollyDamageField>().owner = this;
        _originalScale = _activeField.transform.localScale;
        await ScaleFieldOverTime(_activeField);
        await DamageEnemy();
        await DestroyActiveField(_activeField);
    }

    public async UniTask ScaleFieldOverTime(GameObject field)
    {
        float scaleDuration = 0.75f;
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            float normalizedTime = elapsedTime / scaleDuration;
            field.transform.localScale = Vector3.Lerp(_originalScale, _targetScale, normalizedTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        field.transform.localScale = _targetScale;
    }

    public async UniTask DamageEnemy()
    {
        var time = 0f;
        while(time < AbilityDuration)
        {
            if(hitList != null)
            {
                foreach(var hit in hitList)
                {
                    hit.GetComponent<EnemyHealthBehaviour>().TakeDamage(AbilityDamage);
                }
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            time += 0.5f;
        }
    }
    
    async UniTask DestroyActiveField(GameObject activeField)
    {
        Destroy(activeField);
        await UniTask.CompletedTask;
    }
}
