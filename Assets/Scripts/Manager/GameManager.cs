using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    public async UniTask StartApplication(){
        await SingleBehaviour.Of<FirebaseManager>().OnStartApplication();
        await SingleBehaviour.Of<PlayerDataManager>().OnStartApplication();
        await SingleBehaviour.Of<UIManager>().OnStartApplication();
        Debug.Log("Finish Initialize");
        SingleBehaviour.Of<UIManager>().OnShowScreen(new ShowScreenEvent(UI.LOGIN_SCREEN, false));
    }
}
