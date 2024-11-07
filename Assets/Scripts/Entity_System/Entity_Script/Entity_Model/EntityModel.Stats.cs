using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityStatsModifyEventData
{
    protected Action<StatsModifyMessage> changeStatsEvent;
    public Action<StatsModifyMessage> ChangeStatsEvent { get => changeStatsEvent; set => changeStatsEvent = value; }

    public void InitStatsModifyEvent()
    {
        changeStatsEvent = _ => { };
    }
}
