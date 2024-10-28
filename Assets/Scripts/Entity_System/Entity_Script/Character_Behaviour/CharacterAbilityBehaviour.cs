using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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
        for(int i = 0; i < _abilityStrategies.Count; i++)
        {
            await _abilityStrategies[i].Init(_entityAbilityData.AbilityDatas[i]);
        }
        await UniTask.FromResult(true);
    }

    void OnUsePrimary(CharacterInputAction action)
    {
        if (action == CharacterInputAction.Primary)
            _abilityStrategies[0].OnUse();
    }
    void OnUseQ()
    {

    }
    void OnUseE()
    {

    }
    void OnUseUltimate()
    {

    }
}
