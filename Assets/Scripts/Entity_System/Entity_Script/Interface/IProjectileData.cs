using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileData : IEntityData
{
    public float Speed { get; }
    public Vector3 Direction { get; }

    public EntityHolder Owner { get; }
}
