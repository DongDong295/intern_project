using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FirebaseModule : MonoBehaviour
{
    public virtual UniTask InitModule(){
        return UniTask.CompletedTask;
    }
}
