using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStrategy : MonoBehaviour, IProjectile, IEntityUpdate
{
    protected float speed;
    protected float range;
    protected Vector3 direction;
    protected bool canPierce;
    protected bool canSearch;
    protected float searchRange;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float Range
    {
        get => range;
        set => range = value;
    }

    public Vector3 Direction
    {
        get => direction;
        set => direction = value;
    }

    public bool CanPierce
    {
        get => canPierce;
        set => canPierce = value;
    }

    public bool CanSearch
    {
        get => canSearch;
        set => canSearch = value;
    }

    public float SearchRange
    {
        get => searchRange;
        set => searchRange = value;
    }

    public virtual async UniTask Init(ProjectileAbilityConfigItem data, Vector3 direction)
    {
        this.speed = data.projectileSpeed;
        this.range = data.projectileRange;
        this.direction = direction.normalized;
        this.canPierce = data.canPierce;
        this.canSearch = data.canSearchEnemy;
        this.searchRange = data.searchRange;
        EntityManager.Instance.UpdateEntity.Add(this);
        await UniTask.CompletedTask;
    }

    public virtual void OnFixedUpdate(float fixedDeltaTime){}

    public virtual void OnUpdate(float deltaTime){}
}
