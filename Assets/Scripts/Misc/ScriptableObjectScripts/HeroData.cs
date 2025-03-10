using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HeroDataItem
{
    public float attackDamageStep;
    public float critChance;
    public float cooldownGenerate;
    public float attackSpeed;
    public float moveSpeed;
    public float killDamage;
    public float expBasic;
    public float expStep;
    public int level;
}

public class HeroData : SerializedScriptableObject
{
    public HeroDataItem[] heroDataItems;
}