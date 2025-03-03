using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IManager 
{
    public async UniTask OnApplicationStart(){ await UniTask.CompletedTask;}
}
