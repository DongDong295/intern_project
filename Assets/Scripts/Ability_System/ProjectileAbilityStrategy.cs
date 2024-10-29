using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityStrategy : AbilityStrategy
{
    protected float projectileSpeed;
    protected float projectileRange;
    protected bool canPiercer;
    protected bool canSearchEnemy;
    protected float searchRange;

    protected GameObject bulletPrefab;
    protected Vector3 moveDirection;
}
