using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class GameManager : MonoBehaviour
{
    public async void Awake()
    {
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();

        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(UI.LOGIN_SCREEN, false));
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnFinishInitializeEvent());
    }

    public void Start()
    {
    }
}
