using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseEventTracking : FirebaseModule
{
    public override UniTask InitModule()
    {
        base.InitModule();
        return UniTask.CompletedTask;
    }

    public void PushEventTracking(string e){
        FirebaseAnalytics.LogEvent(e);
    }

    public void PushEventTracking(string e, string parameter1, float value){
        FirebaseAnalytics.LogEvent(e, new Parameter(parameter1, value));
    }
}

