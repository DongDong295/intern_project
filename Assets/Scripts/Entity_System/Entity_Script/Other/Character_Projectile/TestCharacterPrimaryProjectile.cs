using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterPrimaryProjectile : ProjectileStrategy
{
    private Vector3 _startPosition;
    public override async UniTask Init(ProjectileAbilityConfigItem data, Vector3 direction)
    {
        await base.Init(data, direction);
        _startPosition = transform.position;

    }

    public override void OnUpdate(float deltaTime)
    {             
        Move(deltaTime);
        Check();
    }
    
    public void Move(float deltaTime)
    {
        transform.position = transform.position + direction.normalized * speed * deltaTime;
    }

    public void Check()
    {
        if(Vector3.Distance(_startPosition, transform.position) > range)
        {
            EntityManager.Instance.UpdateEntity.Remove(this);
            Destroy(gameObject);
        }
    }
}
