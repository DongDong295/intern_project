using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsModifier
{
    public StatsType Type { get; }
    
    public void IncreaseStats() { }
    public void DecreaseStats() { }
    public async UniTask IncreaseStatsTemp(float duration) { }

    public async UniTask DecreaseStatsTemp(float duration) { } 
}

public enum StatsType
{
    None,
    Health,
    Damage,
    Speed
}
