using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Cysharp.Threading.Tasks;
using System.Linq;
using Runtime.DataConfig;

public class EntityManager : MonoSingleton<EntityManager>
{
    public EntityHolder CurrentCharacter;
    public int CharacterId;
    public EntityHolder Dummy;
    public List<IEntityUpdate> UpdateEntity;

    public float DeltaTime;
    public float FixedDeltaTime;
    protected override void Awake()
    {
        base.Awake();
        UpdateEntity = new List<IEntityUpdate>();
        DeltaTime = Time.deltaTime;
        FixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        CharacterId = 0;
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
    private void FixedUpdate()
    {
        if (UpdateEntity.Count > 0)
            foreach (var entity in UpdateEntity.ToList())
            {
                if (entity is IEntityUpdate)
                {
                    entity.OnFixedUpdate(FixedDeltaTime);
                }
                if (entity == null)
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
        characterModel.InitAbility();
        return characterModel;
    }

    private EntityModel CreateEnemyModel()
    {
        var data = DataManager.Instance.Data;
        var enemyModel = new EnemyModel();
        enemyModel.InitHealthData(1000);
        enemyModel.InitMovementData(data.items[1]);
        enemyModel.InitAbility();
        enemyModel.InitStats();
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

    public Vector3 GetCharacterPosition()
    {
        return CurrentCharacter.transform.position;
    }
}
