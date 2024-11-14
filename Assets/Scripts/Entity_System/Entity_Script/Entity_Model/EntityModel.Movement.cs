using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class EntityModel : IEntityMovementData 
{
    protected float speed;

    protected float dashRange;

    protected int dashCounter;

    protected float dashCooldown;

    protected float dashDuration;

    protected float dashSpeed;

    protected bool canMove;

    protected bool canDash;

    protected Vector3 moveDirection;

    protected Vector3 currentPosition;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float DashSpeed => dashSpeed;

    public float DashRange => dashRange;

    public float DashDuration => dashDuration;

    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }

    public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }

    public bool CanMove { get => canMove; set => canMove = value; }

    public bool CanDash => canDash = true;

    public int DashCounter => dashCounter;

    public float DashCooldown => dashCooldown;


    public void InitMovementData(MovementDataConfigItem data)
    {
        canMove = true;
        this.speed = data.speed;
        this.dashSpeed = data.dashSpeed;
        this.dashDuration = data.dashDuration;
        this.dashRange = data.dashRange;
        this.dashCooldown = data.dashCooldown;
        this.dashCounter = data.dashCounter;
    }
}
