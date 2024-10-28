using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStrategy : MonoBehaviour, IEntityUpdate
{

    public void Init(IProjectileData data)
    {
        EntityManager.Instance.UpdateEntitites.Add(this);
    }

    public void OnFixedUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate(float deltaTime)
    {
        transform.position = (transform.position + Vector3.right) * deltaTime;
    }
}
