using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Cysharp.Threading.Tasks;

public class EntityManager : MonoSingleton<EntityManager>
{
    public EntityHolder CurrentCharacter;
    public List<IEntityUpdate> UpdateEntity;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        UpdateEntity = new List<IEntityUpdate>();
        InitializeCharacterBehaviour();
    }

    private void Update()
    {
        if(UpdateEntity.Count > 0)
            foreach (var entity in UpdateEntity) { 
                if(entity is IEntityUpdate)
                {
                    entity.OnUpdate(Time.deltaTime);
                }
                if(entity == null)
                {
                    UpdateEntity.Remove(entity);
                }
            }
    }

    private CharacterModel CreateCharacterModel()
    {
        var data = DataManager.Instance.Data;
        var characterModel = new CharacterModel();
        characterModel.InitMovementData(data.Speed, data.DashRange, data.DashSpeed, data.DashDuration);
        characterModel.InitEventData();
        characterModel.InitAbilityQ(DataManager.Instance.AbilityConfig[0]);
        characterModel.InitAbilityPrimary(DataManager.Instance.AbilityConfig[1]);
        return characterModel;
    }

    public async UniTaskVoid InitializeCharacterBehaviour()
    {
        await CurrentCharacter.InitializeBehaviour(CreateCharacterModel());
    }
}
