using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityActionEventData
{
    public Action<CharacterInputAction> ActionEvent { set; get; }

    public void InitEventData()
    {
        ActionEvent = _ => {  };
    }
}
