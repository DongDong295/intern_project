using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityData
{
    public bool CanMove { get; set; }
    public bool IsDead { get; set; }

    public float Speed { get; set; }
}
