using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;

public class SettingsModal : BasicModal
{
    [SerializeField] private Button _logoutButton;
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        _logoutButton.onClick.AddListener(() => {Logout();});
        return UniTask.CompletedTask;
    }

    public void Logout(){
        SingleBehaviour.Of<FirebaseAuthentication>().SignOut();
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.LOGIN_SCREEN, false));
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _logoutButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
