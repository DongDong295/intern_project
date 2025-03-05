using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.PubSub;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Screens;

public class LoginScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _loginButton;
    private ISubscription _subscription;    
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        _loginButton.onClick.AddListener(() =>
        {
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnPlayerLoginEvent());
        }); 
        _subscription = Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnFinishInitializeEvent>(OnFinishInitialize);
        return UniTask.CompletedTask;
    }

    public override void DidPushEnter(Memory<object> args)
    {
        base.DidPushEnter(args);
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
    }

    public async void OnFinishInitialize(OnFinishInitializeEvent e)
    {
        // Wait for player authentication if it's not already authenticated
        if (!SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated)
        {
            await WaitForAuthentication();
        }

        // After authentication (or already authenticated), show Main Menu Screen
        ShowMainMenuScreen();
    }

    // Wait for player authentication if it's not done yet
    private async UniTask WaitForAuthentication()
    {
        await UniTask.WaitUntil(() => SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated);
    }

    // Show the Main Menu Screen and transition away from Login Screen
    private void ShowMainMenuScreen()
    {
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(UI.MAIN_MENU_SCREEN, false));
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        base.Cleanup(args);
        _subscription?.Dispose();
        return UniTask.CompletedTask;
    }
}
