using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public class CharacterGetInputBehaviours : EntityBehavior<IEntityActionEventData>
{
    private List<ISubscription> _subscriptions;
    private IEntityActionEventData _entityActionEventData;
    public override void InitializeData(IEntityActionEventData data)
    {
        _entityActionEventData = data;
        _subscriptions = new List<ISubscription>();

        _subscriptions.Add(SimpleMessenger.Subscribe<InputActionMessage>(OnReceiveInput));
    }

    private void OnReceiveInput(InputActionMessage message)
    {
        if (message.action == CharacterInputAction.Primary)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.Primary);
        if (message.action == CharacterInputAction.Dash)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.Dash);
    }
}
