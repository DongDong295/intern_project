using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class EntityBehaviour : MonoBehaviour, IEntityBehaviour
{
    public virtual async UniTask Initialize(IEntityData data) { await UniTask.FromResult(1); }
}
public abstract class EntityBehavior<T> : EntityBehaviour where T : IEntityData
{
    public override async UniTask Initialize(IEntityData data) {
        await InitializeData((T)data);
    }

    public abstract UniTask InitializeData(T data);

    public abstract UniTask DeInitialize();
}

public abstract class EntityBehavior<T1, T2> : EntityBehaviour where T1 : IEntityData
                                                               where T2 : IEntityData
{
    public override async UniTask Initialize(IEntityData data)
    {
         await InitializeData((T1)data, (T2)data);
    }

    public abstract UniTask InitializeData(T1 data, T2 data2);

    public abstract UniTask DeInitialize();
}

