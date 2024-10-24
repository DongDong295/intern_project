using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public class CharacterGetInputBehaviours : EntityBehaviour
{
    private List<ISubscription> _subscriptions;
    public override void Initialize(IEntityData data)
    {
        _subscriptions = new List<ISubscription>();
    }
    

}
