using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EntityModel : IEntityHealthData
{
    private float health;
    public bool isAlive;
    public float Health { get => health; set => health = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    public void InitHealthData(float health)
    {
        isAlive = true;
        this.health = health;
    }
}
