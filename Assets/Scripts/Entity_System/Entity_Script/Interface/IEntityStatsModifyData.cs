using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityStatsModifyData : IEntityData
{
    public Action<StatsModifyMessage> ChangeStatsEvent { get; set; }

    public Dictionary<EntityStatsType, float> EntityStats { get; set; }

    public void RegisterBaseStats(EntityStatsType type, float _baseValue) { EntityStats.Add(type, _baseValue); }

    public float GetStats(EntityStatsType type) { return EntityStats[type]; }

    public void ForceChangeStats(EntityStatsType type, float value) {  EntityStats[type] = value; }

    public void IncreaseStats(EntityStatsType type, float value) { EntityStats[type] += value; }

    public void DecreaseStats(EntityStatsType type, float value) { EntityStats[type] -= value; }
}
