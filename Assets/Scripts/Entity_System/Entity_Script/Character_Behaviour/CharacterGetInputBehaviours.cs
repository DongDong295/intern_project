using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using ZBase.Foundation.PubSub;

public class CharacterGetInputBehaviours : EntityBehavior<IEntityActionEventData>
{
    private List<ISubscription> _subscriptions;
    private IEntityActionEventData _entityActionEventData;

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }

    public override async UniTask InitializeData(IEntityActionEventData data)
    {
        _entityActionEventData = data;
        _subscriptions = new List<ISubscription>();

        _subscriptions.Add(SimpleMessenger.Subscribe<InputActionMessage>(OnReceiveInput));
        await UniTask.FromResult(1);
    }

    private void OnReceiveInput(InputActionMessage message)
    {
        if (message.action == CharacterInputAction.Primary)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.Primary);
        if (message.action == CharacterInputAction.AbilityQ)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.AbilityQ);
        if (message.action == CharacterInputAction.AbilityE)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.AbilityE);
        if (message.action == CharacterInputAction.Dash)
            _entityActionEventData.ActionEvent.Invoke(CharacterInputAction.Dash);
    }
}
