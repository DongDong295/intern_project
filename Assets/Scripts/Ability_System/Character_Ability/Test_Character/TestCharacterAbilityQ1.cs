using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestCharacterAbilityQ1 : AOEAbilityStrategy
{
    [SerializeField] private AOEAbilityConfig config;
    [SerializeField] private GameObject _mollyPrefab;
    
    private GameObject _molly;
    private float _mollyMoveSpeed = 3.5f;
    private float _arcHeight = 3f;
    private Vector3 _originalScale;
    private Vector3 _targetScale = new Vector3(5f, 5f, 0f);

    public override async UniTask InitAbility()
    {
        entityStatsModifyData.RegisterBaseStats(EntityStatsType.AbilityQDamage, config.items[0].abilityDamage);
        await UniTask.CompletedTask;
    }

    protected async override UniTask SetUpInitializeAbility()
    {
        Init(config.items[0]);
        _molly = Instantiate(_mollyPrefab, transform.position, transform.rotation);
        _molly.GetComponent<TestCharacterMolly>().owner = this;
        var targetPos = InputManager.Instance.CursorPosition();
        _molly.GetComponent<TestCharacterMolly>().Init(targetPos);
        await OnFinish();
    }

    public override async UniTask DamageEnemy()
    {
        var time = 0f;
        while(time < AbilityDuration)
        {
            if(hitList != null)
            {
                foreach(var hit in hitList)
                {
                    hit.GetComponent<EnemyHealthBehaviour>().TakeDamage(entityStatsModifyData.GetStats(EntityStatsType.AbilityQDamage));
                }
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            time += 0.5f;
        }
    }
    
}
