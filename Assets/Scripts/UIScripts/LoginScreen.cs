using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using ZBase.Foundation.PubSub;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Screens;

public class LoginScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private TextMeshProUGUI _tapToContinueText;

    private ISubscription _subscription;

    public override async UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        Debug.Log("LoginScreen: Initialize");
        _tapToContinueText.gameObject.SetActive(false);
        _subscription = Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnFinishInitializeEvent>(OnFinishInitialize);

        if (!SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated)
        {
            // Player is not logged in yet
  
            _loginButton.gameObject.SetActive(true);

            _loginButton.onClick.AddListener(() =>
            {
                Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnPlayerLoginEvent());
            });

        }
        else
        {
            //ShowMainMenuScreen();
           
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
        }
    }

    public async void OnFinishInitialize(OnFinishInitializeEvent e)
    {
        Debug.Log("Whao i reached");
        await UniTask.WaitUntil(() => SingleBehaviour.Of<PlayerDataManager>().FinishLoadData);
        if (!SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated)
        {
            await WaitForAuthentication();
        }
        StartBlinkingText();
        ShowMainMenuScreen(); // Waits for tap
    }

    private async UniTask WaitForAuthentication()
    {
        await UniTask.WaitUntil(() => SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated);
    }

    private async void ShowMainMenuScreen()
    {
        Debug.Log("Waiting for player tap to continue...");
        await WaitForPlayerClick();

        _tapToContinueText.DOKill();
        _tapToContinueText.gameObject.SetActive(false);
        var bg = await SingleBehaviour.Of<PoolingManager>().Rent("main-menu-background");
        bg.gameObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
        _loginButton.onClick.RemoveAllListeners();
        _subscription?.Dispose();
    }

    private async UniTask WaitForPlayerClick()
    {
        await UniTask.WaitUntil(() =>
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
            || Input.GetMouseButtonDown(0));
    }

    private void StartBlinkingText()
    {
        _loginButton.gameObject.SetActive(false);
        _tapToContinueText.gameObject.SetActive(true);
        _tapToContinueText.alpha = 1f;
        _tapToContinueText.DOFade(0f, 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        base.Cleanup(args);
        _tapToContinueText.DOKill();
        return UniTask.CompletedTask;
    }
}
