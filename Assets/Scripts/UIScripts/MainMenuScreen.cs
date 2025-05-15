using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Modals;

public class MainMenuSreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private TextMeshProUGUI _displayGem;
    public override async UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        await SingleBehaviour.Of<AudioManager>().PlayMusic("music-menu");
        _displayGem.text = "Gem: " +  SingleBehaviour.Of<PlayerDataManager>().PlayerGem.ToString();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnUpdateGem>(OnUpdateGem);
        _playButton.onClick.AddListener(() =>
        {
            CheckForPlayCondition();
        });
        _characterButton.onClick.AddListener(() =>
        {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.CHARACTERS_MODAL, false));
        });
        _settingButton.onClick.AddListener(()=>{
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.SETTINGS, false));
        });
        _shopButton.onClick.AddListener(() => {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.SHOP_SCREEN, false));
        });
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _settingButton.onClick.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
        _characterButton.onClick.RemoveAllListeners();
        _shopButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
    
    private void CheckForPlayCondition(){
        if(SingleBehaviour.Of<PlayerDataManager>().OwnedHeroDict.Count > 0)
        {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.STAGE_SELECTION_MODAL, false));
        }
        else{
            SingleBehaviour.Of<UIManager>().ShowWarningNoHero();
        }
    }

    private void OnUpdateGem(OnUpdateGem e){
        _displayGem.text = SingleBehaviour.Of<PlayerDataManager>().PlayerGem.ToString();
    }
}
