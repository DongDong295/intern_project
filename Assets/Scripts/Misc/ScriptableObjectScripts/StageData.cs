using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StageDataItems
{
    public int stageIndex;

    public int bossVisualId;
    public float requireDefeatTime;
    public float bossHp;
    public float bossAttackDamage;
    public float bossAttackSpeed;
}

public class StageData : SerializedScriptableObject
{
    public StageDataItems[] stageDataItems;
}
