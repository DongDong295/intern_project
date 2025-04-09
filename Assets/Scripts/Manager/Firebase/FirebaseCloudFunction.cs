using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Functions;
using UnityEngine;

public class FirebaseCloudFunction : FirebaseModule
{
    // Firebase Cloud Functions instance
    private FirebaseFunctions functions;

    // Initialization method for the module
    public override UniTask InitModule()
    {
        base.InitModule();
        InitializeCloudFunctions();
        return UniTask.CompletedTask;
    }

    private void InitializeCloudFunctions()
    {
        functions = FirebaseFunctions.DefaultInstance;
    }

    public FirebaseFunctions GetFunctions()
    {
        return functions;
    }
}
