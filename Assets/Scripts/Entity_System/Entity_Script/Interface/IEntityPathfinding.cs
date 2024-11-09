using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cysharp.Threading.Tasks;
public interface IEntityPathfinding : IEntityData
{
    public void CalculatePath(Vector3 characterPos) { }

    public async void OnFinishCalculate(Path path) { await UniTask.CompletedTask; }

    public async UniTask FollowPath() { await UniTask.CompletedTask; }

    public abstract bool GetIsPathNull();
}
