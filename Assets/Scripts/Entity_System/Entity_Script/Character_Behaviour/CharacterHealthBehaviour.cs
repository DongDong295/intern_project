using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthBehaviour : EntityBehavior<IEntityHealthData>
{
    private IEntityHealthData _entityHealthData;
    private float _health;

    public override async UniTask InitializeData(IEntityHealthData data)
    {
        _entityHealthData = data;
        _health = _entityHealthData.Health;
        await UniTask.FromResult(1);
    }

    public void TakeDamage(float damage)
    {
    }

    public override UniTask DeInitialize()
    {
        throw new System.NotImplementedException();
    }
}
