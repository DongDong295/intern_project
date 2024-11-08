using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAbilityBehaviour : EntityBehavior<IEntityAbilityData, IEntityActionEventData, IEntityStatsModifyData>
{
    private IEntityAbilityData _entityAbilityData;
    private IEntityActionEventData _entityActionEventData;
    private IEntityStatsModifyData _entityStatsModifyData;

    [SerializeField] private List<AbilityStrategy> _abilityStrategies;

    private AbilityType _abilityType;
    public override async UniTask InitializeData(IEntityAbilityData entityAbilityData, IEntityActionEventData entityActionEventData, IEntityStatsModifyData entityStatsModifyData)
    {
        _entityAbilityData = entityAbilityData;
        _entityActionEventData = entityActionEventData;
        _entityStatsModifyData = entityStatsModifyData;
        //_abilityStrategies = entityAbilityData.AbilityStrategies;        
        //foreach (var type in types) {
            //_abilityStrategies.Add(AbilityStrategyFactory.CreateStrategy(type));
        //}
        _entityActionEventData.ActionEvent += OnUsePrimary;
        _entityActionEventData.ActionEvent += OnUseQ;
        _entityActionEventData.ActionEvent += OnUseE;

        await _abilityStrategies[0].Init(_entityAbilityData.PrimaryAbilityConfig.items[0], entityStatsModifyData);
        await _abilityStrategies[1].Init(_entityAbilityData.QAbilityConfig.items[1], entityStatsModifyData);
        await _abilityStrategies[2].Init(_entityAbilityData.EAbilityConfig.items[2], entityStatsModifyData);
        await UniTask.FromResult(true);
    }

    void OnUsePrimary(CharacterInputAction action)
    {
        if (action == CharacterInputAction.Primary)
            _abilityStrategies[0].OnUse().Forget();
    }
    void OnUseQ(CharacterInputAction action)
    {
        if (action == CharacterInputAction.AbilityQ)
        {
           _abilityStrategies[1].OnUse().Forget();
        }
    }
    void OnUseE(CharacterInputAction action)
    {
        if (action == CharacterInputAction.AbilityE)
        {
            _abilityStrategies[2].OnUse().Forget();
        }
    }

    void OnUseUltimate()
    {

    }

    public void SetAbility(AbilityStrategy ability)
    {

    }

    public override UniTask DeInitialize()
    {
        foreach (var abilityStrategy in _abilityStrategies)
        {
            abilityStrategy.DeInitialize();
        }
        return UniTask.CompletedTask;
    }
}
