using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{ 
    public async UniTask<bool> BuildAsync(IEntityData entityData)
    {

        return true;
    }
}
