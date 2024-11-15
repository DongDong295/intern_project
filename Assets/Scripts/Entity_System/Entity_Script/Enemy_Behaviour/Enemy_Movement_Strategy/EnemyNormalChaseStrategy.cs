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
        base.Update();
        if (!isFindingPath && !haveReachTarget)
        {
            if (!havePath)
            {
                FindPath();
            }
            else if (UpdateTimeCheck() && PositionCheck())
            {
                FindPath();
            }
        }
    }

    public override void FindPath()
    {
        isFindingPath = true;
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
