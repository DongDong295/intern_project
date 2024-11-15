using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Runtime.DataConfig;

public class DataManager : MonoSingleton<DataManager>
{
    public AbilityConfig CharacterAbilityConfig;
    public AbilityConfigItem[] AbilityData;

    public EntityCoreDataConfig EntityDataConfig;

    public MovementDataConfig Data;

    protected override void Awake()
    {

    }
    protected void Start()
    {
        LoadCharacterAbilityData();
    }

    void LoadCharacterAbilityData()
    {
        switch (EntityManager.Instance.CharacterId)
        {
            case 0:
                AbilityData = CharacterAbilityConfig.testCharacter;
                break;

            default: break;
        }
    }

    void LoadEnemyAbilityData()
    {

    }
}
