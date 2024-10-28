using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEntityMovementData : IEntityData
{
    public float Speed { get; }

    public float DashRange { get; }

    public float DashDuration { get; }

    public float DashSpeed { get; }

    public bool CanMove { get; } 

    public bool CanDash {  get; }
}
