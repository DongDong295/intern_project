using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private Button _continueButton;
    private StageManager _stageManager;
    private bool result;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _stageManager = SingleBehaviour.Of<StageManager>();
        StageResult();
        _mainMenuButton.onClick.AddListener(() => {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
            Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        });
        _continueButton.onClick.AddListener(async () =>{
            await ContinueAction();
        });
    }

    private void StageResult(){
        result = _stageManager.IsWin;
        if(result){
            _resultText.text = "WIN!";
             _continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "CONTINUE";
        }
        else{
            _resultText.text = "LOOSE!";
            _continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "REPLAY";
        }
    }

    private async UniTask ContinueAction(){
        var stages = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        if(!result){
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnEnterGamePlayScene(_stageManager.Index));
        }
        if(result && _stageManager.Index < stages.stageDataItems.Length){
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnEnterGamePlayScene(_stageManager.Index++));
        }
        Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _mainMenuButton.onClick.RemoveAllListeners();
        _continueButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
