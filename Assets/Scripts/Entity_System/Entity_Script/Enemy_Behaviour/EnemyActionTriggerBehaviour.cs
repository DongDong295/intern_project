using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public class EnemyActionTriggerBehaviour : EntityBehavior<IEntityActionEventData>
{
    private IEntityActionEventData _entityActionEventData;
    private List<ISubscription> _subscriptions;

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }

    public override async UniTask InitializeData(IEntityActionEventData data)
    {
        _entityActionEventData = data;
        _subscriptions = new List<ISubscription>();
        //_subscriptions.Add(SimpleMessenger.Subscribe<EnemeyActionTriggerMessage>(OnReceiveMessage));
        await UniTask.FromResult(1);
    }

    /*private void OnReceiveMessage(EnemeyActionTriggerMessage message)
    {
        if(message.action == EnemyTriggerAction.Attack)
            _entityActionEventData.EnemyActionEvent.Invoke(EnemyTriggerAction.Attack);
    }*/
}
