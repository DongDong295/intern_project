using Cysharp.Threading.Tasks;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalChaseStrategy : EnemyMovementStrategy
{
    public EnemyNormalChaseStrategy(IEntityStatsModifyData stats, IEntityMovementData movement) : base(stats, movement)
    {
        targetPosition = PathfindingManager.Instance.TargetPosition;
    }

    public override void Update()
    {
        if (!isFindingPath)
        {
            if (!havePath)
            {
                isFindingPath = true;
                FindPath();
            }
            else if (PositionCheck() && UpdateTimeCheck())
            {
                isFindingPath = true;
                FindPath();
            }
        }
        base.Update();
    }

    public override void FindPath()
    {
        //Debug.Log("Find");
        //haveReachTarget = false;
        LockMovement();
        PathfindingManager.Instance.GetChasePath(entityMovementData.CurrentPosition, PathfindingManager.Instance.TargetPosition, OnFinishFindPath);
    }

    public override void OnFinishFindPath(Path path)
    {
        if (!path.error && path.vectorPath.Count > 0)
        {
            CurrentPath = path.vectorPath;
            CompletePathfinding();
        }
    }

    public bool PositionCheck()
    {
        if(Vector3.Distance(targetPosition, PathfindingManager.Instance.TargetPosition) > TargetDistanceThreshold)
        {
            targetPosition = PathfindingManager.Instance.TargetPosition;
            return true;
        }
        if(Vector3.Distance(entityMovementData.CurrentPosition, PathfindingManager.Instance.TargetPosition) > TargetDistanceThreshold && haveReachTarget)
        {
            //haveReachTarget = false;
            return true;
        }
        return false;
    }

    public bool UpdateTimeCheck()
    {
        if(MinTimeToNextFind < 0)
        {
            return true;
        }
        return false;
    }
}
