using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IEntityBehaviour
{
    public UniTask<bool> BuildAsync(IEntityData data, CancellationToken token);
}
