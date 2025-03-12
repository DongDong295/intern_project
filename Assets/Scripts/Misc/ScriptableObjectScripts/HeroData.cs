using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class HeroDataItems
{
    public float hpStep;
    public float attackDamageStep;
    public float critChance;
    public float cooldownGenerate;
    public float attackSpeed;
    public float moveSpeed;
    public float killDamage;

    public float expStep;
    public float expBasic;
}

public class HeroData : SerializedScriptableObject{
    public HeroDataItems[] heroDataItems;
}
