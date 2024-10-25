using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class EntityBehaviour : MonoBehaviour, IEntityBehaviour
{
    public virtual void Initialize(IEntityData data) { }
}
public abstract class EntityBehavior<T> : EntityBehaviour where T : IEntityData
{
    public override void Initialize(IEntityData data) {
        InitializeData((T)data);
    }

    public abstract void InitializeData(T data);
}

public abstract class EntityBehavior<T1, T2> : EntityBehaviour where T1 : IEntityData
                                                              where T2 : IEntityData
{
    public override void Initialize(IEntityData data)
    {
         InitializeData((T1)data, (T2)data);
    }

    public abstract void InitializeData(T1 data, T2 data2);
}

