using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class FirebaseRemoteConfigModule : FirebaseModule
{
    private FirebaseRemoteConfig firebaseRemoteConfig;
    public override UniTask InitModule()
    {
        base.InitModule();
        firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
        FetchRemoteConfig();
        return UniTask.CompletedTask;
    }

    private void FetchRemoteConfig()
    {
        firebaseRemoteConfig.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
        {
            if (fetchTask.IsCompleted && !fetchTask.IsFaulted && !fetchTask.IsCanceled)
            {
                firebaseRemoteConfig.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                {
                    GetBackgroundByPlatform();
                }
                );
            }
            else
            {
                Debug.LogError("Failed to fetch remote config");
            }
        });
    }

    public string GetBackgroundByPlatform(){
        var backgroundColor = "";
        if(SingleBehaviour.Of<GameManager>().DeviceType == "IOS"){
            backgroundColor = firebaseRemoteConfig.GetValue("background_ios").StringValue;
        }
        if(SingleBehaviour.Of<GameManager>().DeviceType == "Android"){
            backgroundColor = firebaseRemoteConfig.GetValue("background_android").StringValue;
        }
        return backgroundColor;
    }
}
