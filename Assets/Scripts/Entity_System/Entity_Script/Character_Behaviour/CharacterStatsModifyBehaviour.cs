using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ZBase.Foundation.PubSub;

public class CharacterStatsModifyBehaviour : EntityBehavior<IEntityStatsModifyData>
{
    private List<ISubscription> _subscriptions;
    private IEntityStatsModifyData _entityStatsModifyEventData;

    public override async UniTask InitializeData(IEntityStatsModifyData data)
    {
        _subscriptions = new List<ISubscription>();
        _entityStatsModifyEventData = data;

        _subscriptions.Add(SimpleMessenger.Subscribe<StatsModifyMessage>(OnStatsChange));
        await UniTask.CompletedTask;
    }

    public void OnStatsChange(StatsModifyMessage message)
    {
        /*if (message.statsType == EntityStatsType.Speed)
        {
            _entityStatsModifyEventData.ChangeStatsEvent.Invoke(message);
        }
        if (message.statsType == EntityStatsType.PrimaryDamage)
        {
            _entityStatsModifyEventData.ChangeStatsEvent.Invoke(message);
        }*/

        _entityStatsModifyEventData.ChangeStatsEvent.Invoke(message);
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
