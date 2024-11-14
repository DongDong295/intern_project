using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityActionEventData : IEntityData
{
    public Action<CharacterInputAction> ActionEvent { get; set; }
    
    public Action<EnemyTriggerAction> EnemyActionEvent { get; set; }
}
