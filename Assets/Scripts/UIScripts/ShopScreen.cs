using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _homeButton;
    public override UniTask Initialize(Memory<object> args)
    {
        _homeButton.onClick.AddListener(() => OnBackHome());
        return base.Initialize(args);
    }

    public void OnBackHome(){
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _homeButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
