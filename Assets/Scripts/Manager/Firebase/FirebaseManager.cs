using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
public class FirebaseManager : MonoBehaviour
{
    private Firebase.FirebaseApp _app;

    public async UniTask OnStartApplication()
    {
        // Wait for Firebase dependencies to be resolved asynchronously
        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            // Firebase is available, create and hold a reference to your FirebaseApp
            _app = Firebase.FirebaseApp.DefaultInstance;

            // Initialize Firebase Modules
            await InitFirebaseModules();
        }
        else
        {
            UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here
        }
    }

    private async UniTask InitFirebaseModules(){
        var modules = GetComponentsInChildren<FirebaseModule>();
        foreach (var module in modules)
        {
            await module.InitModule();
        }
    }
}
