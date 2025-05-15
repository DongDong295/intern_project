using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;

public class ShopScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _buyGem;
    [SerializeField] private Button _buyRemoveAd;
    public override UniTask Initialize(Memory<object> args)
    {
        _homeButton.onClick.AddListener(() => OnBackHome());
        //_buyGem.onClick.AddListener(() => SingleBehaviour.Of<IAPManager>().BuyGemPack());
        //_buyRemoveAd.onClick.AddListener(() => SingleBehaviour.Of<IAPManager>().BuyRemoveAd());

        return base.Initialize(args);
    }

    public void OnBackHome(){
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
    }

    public void PurchaseGem(){
        SingleBehaviour.Of<IAPManager>().BuyGemPack();
    }

    public void PurchaseNoAd(){
        SingleBehaviour.Of<IAPManager>().BuyRemoveAd();
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _buyGem.onClick.RemoveAllListeners();
        _buyRemoveAd.onClick.RemoveAllListeners();
        _homeButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
