using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;

public class StageFinishModal : BasicModal
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _replayButton;
    private StageManager _stageManager;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _stageManager = SingleBehaviour.Of<StageManager>();
        StageResult();
        _mainMenuButton.onClick.AddListener(() => {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
            Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        });
        _replayButton.onClick.AddListener(() =>{
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnEnterGamePlayScene(_stageManager.Index));
            Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        });
    }

    private void StageResult(){
        var result = _stageManager.IsWin;
        if(result){
            _resultText.text = "WIN!";
        }
        else{
            _resultText.text = "LOOSE";
        }
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _mainMenuButton.onClick.RemoveAllListeners();
        _replayButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
