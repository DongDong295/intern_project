using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class EntityBehaviour : MonoBehaviour, IEntityBehaviour
{
    protected CancellationToken cancellationToken;

    public virtual UniTask<bool> BuildAsync(IEntityData data, CancellationToken token)
    {
        cancellationToken = token;
        return UniTask.FromResult(true);
    }
}

public abstract class EntityBehavior<T1> : EntityBehaviour where T1 : IEntityData
{
    public async override UniTask<bool> BuildAsync(IEntityData data, CancellationToken cancellationToken)
    {
        await base.BuildAsync(data, cancellationToken);
        return await BuildDataAsync((T1)data);
    }
    protected abstract UniTask<bool> BuildDataAsync(T1 data);
}
