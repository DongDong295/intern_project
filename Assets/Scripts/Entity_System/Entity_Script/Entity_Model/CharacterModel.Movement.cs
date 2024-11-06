using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class CharacterModel : IEntityMovementData 
{
    protected float speed;

    protected float dashRange;

    protected int dashCounter;

    protected float dashCooldown;

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

    public int DashCounter => dashCounter;

    public float DashCooldown => dashCooldown;

    float IEntityMovementData.Speed {
        get => speed;
        set => Speed = value;
    }

    public void InitMovementData(CharacterMovementDataConfigItem data)
    {
        this.speed = data.characterSpeed;
        this.dashSpeed = data.characterDashSpeed;
        this.dashDuration = data.characterDashDuration;
        this.dashRange = data.characterDashRange;
        this.dashCooldown = data.characterDashCooldown;
        this.dashCounter = data.characterDashCounter;
    }
}
