using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBehaviour : EntityBehavior<IEntityActionEventData, IEntityStatsModifyData, IEntityMovementData>
{
    private IEntityActionEventData _entityActionEventData;
    private IEntityMovementData _entityMovementData;

    [SerializeField] private List<AbilityStrategy> _abilityStrategies;

    public override async UniTask InitializeData(IEntityActionEventData eventData, IEntityStatsModifyData entityStatsModifyData, IEntityMovementData movementData)
    {
        _entityActionEventData = eventData;
        _entityMovementData = movementData;
        await _abilityStrategies[0].Init(DataManager.Instance.CharacterAbilityConfig.dummy[0], entityStatsModifyData);
        _entityActionEventData.EnemyActionEvent += CheckForAttack;
        await UniTask.CompletedTask;
    }    

    public void CheckForAttack(EnemyTriggerAction action)
    {
        if(action == EnemyTriggerAction.Attack)
        {
            _abilityStrategies[0].OnUse().Forget();
        }
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
