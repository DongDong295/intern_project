using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityStatsModifyData
{
    protected Action<StatsModifyMessage> changeStatsEvent;
    public Action<StatsModifyMessage> ChangeStatsEvent { get => changeStatsEvent; set => changeStatsEvent = value; }
    public Dictionary<EntityStatsType, float> EntityStats { get => entityStats; set => entityStats = value; }

    protected Dictionary<EntityStatsType, float> entityStats;

    public void InitStats()
    {
        EntityStats = new Dictionary<EntityStatsType, float>();
        changeStatsEvent = _ => { };
    }

    public void RegisterStats(EntityStatsType type, float _baseValue)
    {
        EntityStats.Add(type, _baseValue);
    }

    /*public float GetStats(EntityStatsType type)
    {
        var value = entityStats[type];
        return value;
    }*/
}
