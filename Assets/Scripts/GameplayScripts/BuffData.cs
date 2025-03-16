using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuffDataItems
{
    public int buffId;
    public BuffType buffName;
    public float buffValue;
    public float buffTime;
}

public class BuffData : SerializedScriptableObject{
    public BuffDataItems[] items;
}

public enum BuffType{
    None,
    AttackSpeed,
    MovementSpeed
}