using Cysharp.Threading.Tasks;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCharacterMolly : MonoBehaviour
{    
    public AbilityStrategy owner;

    [SerializeField] private GameObject _mollyFieldPrefab;
    private float _mollyMoveSpeed = 3.5f;
    private float _arcHeight = 3f;
    private Vector3 _targetPos;
    private Vector3 _originalScale;
    private Vector3 _targetScale = new Vector3(5f, 5f, 0f);

    public void Init(Vector3 targetPos)
    {
        _targetPos = targetPos;
        MoveMollyInArc().Forget();
    }

    public async UniTask MoveMollyInArc()
    {
        Vector3 startPos = transform.position;
        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime * _mollyMoveSpeed;
            float normalizedTime = Mathf.Clamp01(timeElapsed / duration);

            Vector3 currentPos = Vector3.Lerp(startPos, _targetPos, normalizedTime);
            float arc = _arcHeight * Mathf.Sin(Mathf.PI * normalizedTime);
            currentPos.y += arc;

            transform.position = currentPos;
            await UniTask.Yield();
        }
        ActiveField().Forget();
    }

    public async UniTask ActiveField()
    {
        var _activeField = Instantiate(_mollyFieldPrefab, _targetPos, transform.rotation);
        _activeField.GetComponent<MollyDamageField>().owner = owner;
        _originalScale = _activeField.transform.localScale;
        await ScaleFieldOverTime(_activeField);
        await owner.DamageEnemy();
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

    async UniTask DestroyActiveField(GameObject activeField)
    {
        Destroy(activeField);
        Destroy(this.gameObject);
        await UniTask.CompletedTask;
    }
}

