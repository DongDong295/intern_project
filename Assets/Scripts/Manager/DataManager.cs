using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;

public class DataManager : MonoSingleton<DataManager>
{
    public List<AbilityData> AbilityData;
    public CharacterData Data;
    protected override void Awake()
    {

    }
}
