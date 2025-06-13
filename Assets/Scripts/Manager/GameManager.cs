using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class GameManager : MonoBehaviour
{
    public string DeviceType;
    private bool _hasShownLoginScreen = false;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    public async void Start()
    {
#if UNITY_ANDROID
        DeviceType = "Android";
#elif UNITY_IOS
        DeviceType = "IOS";
#endif
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;

        await StartApplication();

        if (!_hasShownLoginScreen)
        {
            _hasShownLoginScreen = true;
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
        }
            //SingleBehaviour.Of<PlayerDataManager>().PlayerGem += (int)1000;
            //Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnUpdateGem(1000));
    }

    public async UniTask StartApplication()
    {
        await UnityServices.InitializeAsync();
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();
        await SingleBehaviour.Of<AudioManager>().OnStartApplication();
        await SingleBehaviour.Of<StageManager>().OnStartApplication();
        await SingleBehaviour.Of<IAPManager>().OnStartApplication();
        Singleton.Of<DataManager>().Initiate();

        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnEnterGamePlayScene>(ChangeToGameplayScene);

#if !UNITY_EDITOR
        Debug.Log("This line is running");
#endif

        SingleBehaviour.Of<FirebaseEventTracking>().PushEventTracking("Player login");
    }

    public async UniTask ChangeToGameplayScene(OnEnterGamePlayScene e)
    {
        Debug.Log("Stage is " + e.StageIndex);
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(
            ScreenUI.GetLoadingScreen(SingleBehaviour.Of<UIManager>().Platform), false));
        await SingleBehaviour.Of<StageManager>().InitiateStage(e.StageIndex);
    }
}
