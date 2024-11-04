using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthBehaviour : EntityBehavior<IEntityHealthData>
{
    private IEntityHealthData _entityHealthData;
    private float _health;
    public override UniTask InitializeData(IEntityHealthData data)
    {
        _entityHealthData = data;
        _health = _entityHealthData.Health;
        return UniTask.CompletedTask;
    }

    public void TakeDamage(float damage)
    {
    }
}
