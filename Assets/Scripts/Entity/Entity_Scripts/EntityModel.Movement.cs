using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class EntityModel : IEntityMovementData
{
    protected float speed;

    protected float dashRange;

    protected bool canMove;

    protected bool canDash;

    public float Speed => speed;

    public float DashRange => dashRange;

    public bool CanMove => canMove = true;

    public bool CanDash => canDash = true;  

    public void InitMovementData(float speed, float dashRange)
    {
        this.speed = speed;
        this.dashRange = dashRange;
    }
}
