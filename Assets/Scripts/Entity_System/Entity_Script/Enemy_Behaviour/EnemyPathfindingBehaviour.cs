using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingBehaviour : EntityBehavior<IEntityMovementData>, IEntityPathfinding
{
    private Seeker _seeker;
    private Rigidbody2D _rb;
    private Path _currentPath;

    public float moveSpeed = 5f;  
    

    public override async UniTask InitializeData(IEntityMovementData data)
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        PathfindingManager.Instance.RegisterPathfinding(this);
        await UniTask.CompletedTask;
    }

    public void CalculatePath(Vector3 characterPos)
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(transform.position, characterPos, OnFinishCalculate);
        }
    }

    private async void OnFinishCalculate(Path path)
    {
        if (!path.error)
        {
            _currentPath = path;
        }
        await FollowPath();
    }

    private async UniTask FollowPath()
    {
        if (_currentPath == null || _currentPath.vectorPath.Count == 0)
        {
            return;
        }

        float distanceThreshold = 0.1f;
        int waypointIndex = 0;

        while (waypointIndex < _currentPath.vectorPath.Count)
        {
            Vector3 nextWaypoint = _currentPath.vectorPath[waypointIndex];
            Vector3 direction = (nextWaypoint - transform.position).normalized;

            _rb.MovePosition(_rb.position + (Vector2)direction * moveSpeed * Time.deltaTime);
            //Debug.Log(_rb.position + (Vector2)direction * moveSpeed);

            if (Vector3.Distance(transform.position, nextWaypoint) < distanceThreshold)
            {
                waypointIndex++;
            }
            await UniTask.Yield();
        }
        _rb.velocity = Vector2.zero;
        _currentPath = null;
    }


    private void OnDestroy()
    {
        DeInitialize().Forget();
    }

    public override async UniTask DeInitialize()
    {
        PathfindingManager.Instance.UnregisterPathFinding(this);
        await UniTask.CompletedTask;
    }

    public bool GetIsPathNull()
    {
        return _currentPath == null;
    }
}
