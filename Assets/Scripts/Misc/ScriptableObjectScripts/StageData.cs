using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StageDataItems
{
    public int StageIndex;

    public int BossVisualID;
    public float RequireDefeatTime;
    public float BossHP;
    public float BossAttackDamage;
    public float BossAttackSpeed;
}

public class StageData : SerializedScriptableObject
{
    public StageDataItems[] stageDataItems;
}
