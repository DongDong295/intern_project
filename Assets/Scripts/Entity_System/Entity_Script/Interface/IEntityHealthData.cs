using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEntityHealthData : IEntityData
{
    public float Health {  get; private set; }
    public bool isAlive { get; private set; }
}
