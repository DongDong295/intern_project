using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Cysharp.Threading.Tasks;
using System.Linq;

public class EntityManager : MonoSingleton<EntityManager>
{
    public EntityHolder CurrentCharacter;
    public EntityHolder Dummy;
    public List<IEntityUpdate> UpdateEntity;

    public float DeltaTime;
    protected override void Awake()
    {
        base.Awake();
        UpdateEntity = new List<IEntityUpdate>();
        DeltaTime = Time.deltaTime;
    }

    private void Start()
    {
        InitializeCharacterBehaviour().Forget();
        InitializeEnemyBehaviour().Forget();
    }

    private void Update()
    {
        if(UpdateEntity.Count > 0)
            foreach (var entity in UpdateEntity.ToList()) { 
                if(entity is IEntityUpdate)
                {
                    entity.OnUpdate(DeltaTime);
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
        characterModel.InitMovementData(data.items[0]);
        characterModel.InitStats();
        characterModel.InitEventData();
        characterModel.InitHealthData(5);
        characterModel.InitAbilityPrimary(DataManager.Instance.AbilityConfig[1]);
        characterModel.InitAbilityQ(DataManager.Instance.AbilityConfig[0]);
        characterModel.InitAbilityE(DataManager.Instance.AbilityConfig[2]);
        return characterModel;
    }

    private EntityModel CreateEnemyModel()
    {
        var enemyModel = new EnemyModel();
        enemyModel.InitHealthData(1000);
        enemyModel.InitPathfiding();
        return enemyModel;
    }

    public async UniTaskVoid InitializeCharacterBehaviour()
    {
        await CurrentCharacter.InitializeBehaviour(CreateCharacterModel());
    }

    public async UniTaskVoid InitializeEnemyBehaviour()
    {
        await Dummy.InitializeBehaviour(CreateEnemyModel());
    }
}
