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
        return UniTask.CompletedTask;
    }
}
