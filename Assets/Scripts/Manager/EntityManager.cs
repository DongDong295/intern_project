using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Cysharp.Threading.Tasks;

public class EntityManager : MonoSingleton<EntityManager>
{
    public EntityHolder CurrentCharacter;
    public List<IEntityUpdate> UpdateEntitites;
    protected override void Awake()
    {
        base.Awake();
        UpdateEntitites = new List<IEntityUpdate>();
    }

    private void Start()
    {
        InitializeCharacterBehaviour();
    }

    private void Update()
    {
        foreach (var entity in UpdateEntitites)
        {
            if(entity is IEntityUpdate)
            {
                entity.OnUpdate(Time.deltaTime);
            }
        }
    }

    private CharacterModel CreateCharacterModel()
    {
        var data = DataManager.Instance.Data;
        var characterModel = new CharacterModel();
        characterModel.InitMovementData(data.Speed, data.DashRange, data.DashSpeed, data.DashDuration);
        Debug.Log(characterModel.Speed);
        characterModel.InitEventData();
        characterModel.InitAbility(DataManager.Instance.AbilityData);
        return characterModel;
    }

    public async UniTask InitializeCharacterBehaviour()
    {
        await CurrentCharacter.InitializeBehaviour(CreateCharacterModel());
    }
}
