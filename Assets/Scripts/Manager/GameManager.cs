using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;
        await StartApplication();
    }

    public void Update()
    {
    }

    public async UniTask ChangeToGameplayScene(OnEnterGamePlayScene e)
    {
        Debug.Log("Stage is " + e.StageIndex);
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.GetLoadingScreen(
        SingleBehaviour.Of<UIManager>().Platform)
        , false));
        await SingleBehaviour.Of<StageManager>().InitiateStage(e.StageIndex);
    }

    public async UniTask StartApplication(){
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();
        await SingleBehaviour.Of<StageManager>().OnStartApplication();
        Singleton.Of<DataManager>().Initiate();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnEnterGamePlayScene>(ChangeToGameplayScene);
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));

#if UNITY_EDITOR
        //Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
#else
        Debug.Log("This line is running");
#endif
        SingleBehaviour.Of<FirebaseEventTracking>().PushEventTracking("Player login");

        //SingleBehaviour.Of<GetServerTime>().GetTime();
    }
}
