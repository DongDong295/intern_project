using Runtime.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class PathfindingManager : MonoSingleton<PathfindingManager>
{
    public List<IEntityPathfinding> Entity;
    public AstarPath path;
    public float updateInterval = 0.5f;

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
        //InvokeRepeating("UpdateEntityPath", 0, updateInterval);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEntityPath();
    }

    public void RegisterPathfinding(IEntityPathfinding entity)
    {
        Entity.Add(entity);
    }

    public void UnregisterPathFinding(IEntityPathfinding entity)
    {
        Entity.Remove(entity);
    }

    public void UpdateEntityPath()
    {
        if(Entity.Count > 0)
        {
            foreach(var e in Entity)
            {
                if(EntityManager.Instance.CurrentCharacter != null && e.GetIsPathNull())
                    e.CalculatePath(EntityManager.Instance.CurrentCharacter.transform.position);
            }
        }
    }
}
