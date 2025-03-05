using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainGameplayScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        return UniTask.CompletedTask;
    }
}
