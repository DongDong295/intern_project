using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using Runtime.DataConfig;

public class DataManager : MonoSingleton<DataManager>
{
    public List<AbilityConfig> AbilityConfig;
    public CharacterMovementDataConfig Data;

    protected override void Awake()
    {
    }
}
