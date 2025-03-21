using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class GameManager : MonoBehaviour
{
    public string DeviceType;
    public void Awake()
    {

    }

    public async void Start()
    {
        #if UNITY_ANDROID
            DeviceType = "Android";
        #elif UNITY_IOS
            DeviceType = "IOS";
        #endif
        await StartApplication();
    }

    public async UniTask ChangeToGameplayScene(OnEnterGamePlayScene e)
    {
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.GetLoadingScreen(
        SingleBehaviour.Of<UIManager>().Platform)
        , false));
        await SingleBehaviour.Of<StageManager>().InitiateStage(e.StageIndex);
    }

    public async UniTask StartApplication(){
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        Debug.Log("Is authenticated: " + SingleBehaviour.Of<PlayerDataManager>().IsAuthenticated);
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();
        Singleton.Of<DataManager>().Initiate();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnEnterGamePlayScene>(ChangeToGameplayScene);

#if UNITY_EDITOR
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
        
#else
        Debug.Log("This line is running");
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
#endif
        SingleBehaviour.Of<FirebaseEventTracking>().PushEventTracking("Player login");
    }
}
