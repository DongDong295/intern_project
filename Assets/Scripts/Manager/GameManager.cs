using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {

    }

    public async void Start()
    {
        await StartApplication();
    }

    public async UniTask ChangeToGameplayScene(OnEnterGamePlayScene e)
    {
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOADING_SCREEN, false));
        await SingleBehaviour.Of<StageManager>().InitiateStage(e.StageIndex);
    }

    public async UniTask StartApplication(){
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();
        Singleton.Of<DataManager>().Initiate();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnEnterGamePlayScene>(ChangeToGameplayScene);

#if UNITY_EDITOR
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
#else
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
#endif

        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
    }
}
