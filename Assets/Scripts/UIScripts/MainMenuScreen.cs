using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZBase.UnityScreenNavigator.Core.Modals;

public class MainMenuSreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _settingButton;


    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        _playButton.onClick.AddListener(() =>
        {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.STAGE_SELECTION_MODAL, false));
        });
        _characterButton.onClick.AddListener(() =>
        {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.CHARACTERS_MODAL, false));
        });
        _settingButton.onClick.AddListener(()=>{
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.SETTINGS, false));
        });
        return UniTask.CompletedTask;
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _settingButton.onClick.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
        _characterButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
