using Cysharp.Threading.Tasks;
using Pathfinding;
using Runtime.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Views;

public class PathfindingManager : MonoSingleton<PathfindingManager>
{
    public List<IEntityPathfinding> Entity;
    public Vector3 TargetPosition;

    public AstarPath path;
    public float UpdateInterval;

    protected override void Awake()
    {
        Entity = new List<IEntityPathfinding>();
        if(path == null)
        {
            path = AstarPath.active;
        }
    }

    void Start()
    {
        //TargetTransform = EntityManager.Instance.CurrentCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        TargetPosition = EntityManager.Instance.CurrentCharacter.transform.position;
    }
    private void FixedUpdate()
    {
    }

    public void RegisterPathfinding(IEntityPathfinding entity)
    {
        Entity.Add(entity);
    }

    public void UnregisterPathFinding(IEntityPathfinding entity)
    {
        Entity.Remove(entity);
    }

    public void GetChasePath(Vector3 pos, Vector3 targetPos, OnPathDelegate del)
    {
        Path path = ABPath.Construct(pos, targetPos, del);
        AstarPath.active.heuristic = Heuristic.Manhattan;
        AstarPath.StartPath(path);
    }
}
