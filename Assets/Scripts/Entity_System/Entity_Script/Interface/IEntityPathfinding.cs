using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cysharp.Threading.Tasks;
public interface IEntityPathfinding : IEntityData
{
    public async UniTask CalculatePath(Vector3 characterPos) { }

    public void OnFinishCalculatePath() { }

    //public async void OnFinishCalculatePath(Path path) { await UniTask.CompletedTask; }

    public async UniTask FollowPath() { await UniTask.CompletedTask; }

    public abstract bool GetIsPathNull();

    public abstract Vector3 GetPosition();
}
