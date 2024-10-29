using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAbilityBehaviour : EntityBehavior<IEntityAbilityData, IEntityActionEventData>
{
    private IEntityAbilityData _entityAbilityData;
    private IEntityActionEventData _entityActionEventData;
    //[SerializeField] private List<AbilityType> types;

    [SerializeField] private List<AbilityStrategy> _abilityStrategies;

    public override async UniTask InitializeData(IEntityAbilityData entityAbilityData, IEntityActionEventData entityActionEventData)
    {
        _entityAbilityData = entityAbilityData;
        _entityActionEventData = entityActionEventData;
        //_abilityStrategies = entityAbilityData.AbilityStrategies;        
        //foreach (var type in types) {
            //_abilityStrategies.Add(AbilityStrategyFactory.CreateStrategy(type));
        //}
        _entityActionEventData.ActionEvent += OnUsePrimary;
        _entityActionEventData.ActionEvent += OnUseQ;
        await _abilityStrategies[0].Init(_entityAbilityData.PrimaryAbilityConfig.items[0]);
        await _abilityStrategies[1].Init(_entityAbilityData.QAbilityConfig.items[1]);
        await UniTask.FromResult(true);
    }

    void OnUsePrimary(CharacterInputAction action)
    {
        if (action == CharacterInputAction.Primary)
            _abilityStrategies[0].OnUse();
    }
    void OnUseQ(CharacterInputAction action)
    {
        if (action == CharacterInputAction.AbilityQ)
        {
            _abilityStrategies[1].OnUse();
        }

    }
    void OnUseE()
    {

    }
    void OnUseUltimate()
    {

    }
}
