using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZBase.UnityScreenNavigator.Core.Modals;

public class BasicModal : Modal
{
    [SerializeField] protected Button _closeButton;
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        _closeButton.onClick.AddListener(CloseModal);   
        return UniTask.CompletedTask;
    }

    public void CloseModal(){
        Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent(true));
    }
}
