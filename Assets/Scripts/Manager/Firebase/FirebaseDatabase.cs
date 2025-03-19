using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabase : FirebaseModule
{
    public override UniTask InitModule()
    {
        base.InitModule();
        return UniTask.CompletedTask;
    }
}
