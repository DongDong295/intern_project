using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using ZBase.Foundation.PubSub;
using ZBase.UnityScreenNavigator.Core.Screens;
using ZBase.UnityScreenNavigator.Core.Views;

public class UIManager : MonoBehaviour
{
    private ISubscription _subscription;
    public async UniTask OnStartApplication()
    {
        _subscription = Pubsub.Subscriber.Scope<UIEvent>().Subscribe<ShowScreenEvent>(OnShowScreen);
        await UniTask.WaitUntil(() => ScreenLauncher.ContainerManager != null);
        await UniTask.CompletedTask;
    }

    public async void OnShowScreen(ShowScreenEvent e){
        Debug.Log("Called, showing now! " + e.Path);
        var options = new ViewOptions(e.Path, true, loadAsync: e.LoadAsync);
        await ScreenLauncher.ContainerManager.Find<ScreenContainer>().PushAsync(options);
    }
}
