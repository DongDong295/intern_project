using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;
using ZBase.UnityScreenNavigator.Core.Modals;
using ZBase.UnityScreenNavigator.Core.Screens;
using ZBase.UnityScreenNavigator.Core.Views;

public class UIManager : MonoBehaviour
{
    private List<ISubscription> _subscription;
    private Queue<ShowScreenEvent> _screenQueue = new Queue<ShowScreenEvent>();
    private bool _isShowingScreen = false;

    public async UniTask OnStartApplication()
    {
        _subscription = new List<ISubscription>();

        _subscription.Add(Pubsub.Subscriber.Scope<UIEvent>().Subscribe<ShowScreenEvent>(OnShowScreen));
        _subscription.Add(Pubsub.Subscriber.Scope<UIEvent>().Subscribe<ShowModalEvent>(OnShowModal));
        _subscription.Add(Pubsub.Subscriber.Scope<UIEvent>().Subscribe<CloseModalEvent>(
            async (e) => await OnCloseModal(e, false)));
        _subscription.Add(Pubsub.Subscriber.Scope<UIEvent>().Subscribe<CloseScreenEvent>(
            async (e) => await OnCloseScreen(e, false)));

        await UniTask.WaitUntil(() => ScreenLauncher.ContainerManager != null);
        await UniTask.CompletedTask;
    }

    public void OnShowScreen(ShowScreenEvent e)
    {
        _screenQueue.Enqueue(e);
        if (!_isShowingScreen)
        {
            ShowNextScreen().Forget();
        }
    }

    private async UniTaskVoid ShowNextScreen()
    {
        if (_screenQueue.Count == 0)
        {
            _isShowingScreen = false;
            return;
        }

        _isShowingScreen = true;
        var e = _screenQueue.Dequeue();
        var options = new ViewOptions(e.Path, true, loadAsync: e.LoadAsync);
        await ScreenLauncher.ContainerManager.Find<ScreenContainer>().PushAsync(options);
        Debug.Log("Pushed screen " + e.Path);
        await UniTask.DelayFrame(1);
        ShowNextScreen().Forget();
    }

    public async UniTask OnCloseScreen(CloseScreenEvent e, bool isPlayAnim)
    {
        await ScreenLauncher.ContainerManager.Find<ScreenContainer>().PopAsync(isPlayAnim);
    }

    public async void OnShowModal(ShowModalEvent e)
    {
        var options = new ModalOptions(e.Path, true, loadAsync: e.LoadAsync);
        await ScreenLauncher.ContainerManager.Find<ModalContainer>().PushAsync(options);
    }

    public async UniTask OnCloseModal(CloseModalEvent e, bool isPlayAnim)
    {
        await ScreenLauncher.ContainerManager.Find<ModalContainer>().PopAsync(isPlayAnim);
    }
}