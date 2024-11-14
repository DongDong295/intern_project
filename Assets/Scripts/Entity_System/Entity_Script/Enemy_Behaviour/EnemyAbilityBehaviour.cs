using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBehaviour : EntityBehavior<IEntityActionEventData>
{
    private IEntityActionEventData _entityActionEventData;

    [SerializeField] private List<AbilityStrategy> _abilityStrategies;

    public override async UniTask InitializeData(IEntityActionEventData eventData)
    {
        _entityActionEventData = eventData;
        eventData.EnemyActionEvent += OnAttack;
        await UniTask.CompletedTask;
    }    

    public void OnAttack(EnemyTriggerAction action)
    {
        if(action == EnemyTriggerAction.Attack)
        {
            _abilityStrategies[Random.Range(0, _abilityStrategies.Count)].OnUse().Forget();
        }
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
