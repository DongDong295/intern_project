using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEntityMovementData : IEntityData
{
    public float Speed { get; set; }

    public bool IsReachTarget {  get; set; }

    public float DashRange { get; }

    public float DashDuration { get; }

    public float DashSpeed { get; }

    public bool CanMove { get; set; } 

    public bool CanDash {  get; }

    public int DashCounter {  get; }

    public float DashCooldown { get; }

    public Vector3 MoveDirection { get; set; }

    public Vector3 CurrentPosition { get; set; }

    public void SetMoveDirection(Vector3 direction)
    {
        MoveDirection = direction;
    }

    public Vector3 GetPosition()
    {
        return CurrentPosition;
    }
}
