using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class EntityModel : IEntityMovementData 
{
    protected float speed;

    protected float dashRange;

    protected float dashDuration;

    protected float dashSpeed;

    protected bool canMove;

    protected bool canDash;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float DashSpeed => dashSpeed;

    public float DashRange => dashRange;

    public float DashDuration => dashDuration;



    public bool CanMove => canMove = true;

    public bool CanDash => canDash = true;

    float IEntityMovementData.Speed {
        get => speed;
        set => Speed = value;
    }

    public void InitMovementData(float speed, float dashRange, float dashSpeed, float dashDuration)
    {
        this.speed = speed;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashRange = dashRange;
    }

}
