using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseDatabase : FirebaseModule
{
    public FirebaseFirestore Database;

    // Initialization method for the module
    public override UniTask InitModule()
    {
        base.InitModule();
        InitializeFirestore();
        return UniTask.CompletedTask;
    }

    private void InitializeFirestore()
    {
        Database = FirebaseFirestore.DefaultInstance;
    }
}
