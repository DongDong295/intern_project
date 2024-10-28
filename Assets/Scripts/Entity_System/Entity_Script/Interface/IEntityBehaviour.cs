using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityBehaviour
{
    public async UniTask Initialize(IEntityData data) { await Initialize(data); }
}
