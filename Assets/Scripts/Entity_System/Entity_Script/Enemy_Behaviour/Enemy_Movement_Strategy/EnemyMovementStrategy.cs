using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyMovementStrategy : IMovementStrategy
{
    protected IEntityMovementData entityMovementData;
    protected IEntityStatsModifyData entityStatsModifyData;

    protected Vector3 targetPosition;

    public bool havePath;
    public bool haveReachTarget;
    public bool isFindingPath;
    public List<Vector3> CurrentPath;

    public float TargetDistanceThreshold;
    public float TargetReachThreshold;
    public float TargetNodeReachThreshold;
    public float MinTimeToNextFind;
    public float BalanceRate;
    public int CurrentIndexPosition;

    public EnemyMovementStrategy(IEntityStatsModifyData stats, IEntityMovementData movement)
    {
        havePath = false;
        isFindingPath = false;

        entityMovementData = movement; 
        entityStatsModifyData = stats;

        
        CurrentIndexPosition = 0;
        TargetDistanceThreshold = 2f;
        TargetReachThreshold = 2.5f;
        TargetNodeReachThreshold = 0.5f;
        MinTimeToNextFind = 0.65f;
        BalanceRate = 5f;
    }
    public virtual void Update()
    { 
        MinTimeToNextFind -= Time.deltaTime;
        if (havePath && !haveReachTarget)
        {
            Move();
        }
        if(Vector3.Distance(entityMovementData.CurrentPosition, PathfindingManager.Instance.TargetPosition) > TargetReachThreshold)
        {
            haveReachTarget = false;
            entityMovementData.IsReachTarget = false;
        }
    }

    public virtual void FixedUpdate()
    {
    }


    public void Move() { 
        if(CurrentIndexPosition == CurrentPath.Count || Vector3.Distance(entityMovementData.CurrentPosition, PathfindingManager.Instance.TargetPosition) <= TargetReachThreshold)
        {
            haveReachTarget = true;
            entityMovementData.IsReachTarget = true;
            LockMovement();
        }
        else
            if (Vector3.Distance(entityMovementData.CurrentPosition, CurrentPath[CurrentIndexPosition]) <= TargetNodeReachThreshold)
            {
                CurrentIndexPosition++;
                if (CurrentIndexPosition < CurrentPath.Count)
                {
                    entityMovementData.SetMoveDirection((CurrentPath[CurrentIndexPosition] - entityMovementData.CurrentPosition).normalized);
                }
                else
                {
                    //LockMovement();
                }
            }
            else
            {
                entityMovementData.SetMoveDirection((CurrentPath[CurrentIndexPosition] - entityMovementData.CurrentPosition).normalized);
            }
    }

    public void LockMovement()
    {
        entityMovementData.SetMoveDirection(Vector3.zero);
    }

    public virtual void FindPath(){ }
    public abstract void OnFinishFindPath(Path path);

    public void CompletePathfinding()
    {
        havePath = true;
        isFindingPath = false;
        MinTimeToNextFind = 0.65f;
        CurrentIndexPosition = 0;
    }
}
