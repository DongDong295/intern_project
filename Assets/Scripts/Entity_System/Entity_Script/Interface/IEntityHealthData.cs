using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityHealthData : IEntityData
{
    public float Health {  get; }
    public bool IsAlive { get; }
}
