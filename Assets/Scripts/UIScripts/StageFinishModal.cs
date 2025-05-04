using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;
using DG.Tweening;
public class StageFinishModal : BasicModal
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private TextMeshProUGUI _displayPrize;
    private StageManager _stageManager;
    [SerializeField] private LocalizeStringEvent[] _continueText;
    [SerializeField] private LocalizeStringEvent[] _resultTextLocalize;

    private bool result;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _stageManager = SingleBehaviour.Of<StageManager>();
        await StageResult();
        _mainMenuButton.onClick.AddListener(() => {
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_MENU_SCREEN, false));
            Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        });
        _continueButton.onClick.AddListener(async () =>{
            await ContinueAction();
        });
    }

    private async UniTask StageResult()
    {
        result = _stageManager.IsWin;
        if (result)
        {
            StageData data = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
            int prizeValue = (int)data.stageDataItems[_stageManager.Index].prizeValue;
            AnimatePrizeValue(prizeValue, 1.2f); // animate over 1.2 seconds
        }
        else{
            _displayPrize.text = "";
        }

        var resultLocalizedString = _resultTextLocalize[result ? 0 : 1].StringReference;
        var resultText = await resultLocalizedString.GetLocalizedStringAsync();
        _resultText.text = resultText;

        var continueLocalizedString = _continueText[result ? 0 : 1].StringReference;
        var continueText = await continueLocalizedString.GetLocalizedStringAsync();
        _continueButton.GetComponentInChildren<TextMeshProUGUI>().text = continueText;
    }



    private async UniTask ContinueAction(){
        var stages = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        if(!result){
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnEnterGamePlayScene(_stageManager.Index));
        }
        if(result && _stageManager.Index < stages.stageDataItems.Length){
            Debug.Log(" Index of " + _stageManager.Index + " Next stage " + _stageManager.Index++);
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
    private void AnimatePrizeValue(int targetValue, float duration = 1.0f)
    {
        int displayedValue = 0;
        DOTween.To(() => displayedValue, x =>
        {
            displayedValue = x;
            _displayPrize.text = displayedValue.ToString();
        }, targetValue, duration).SetEase(Ease.OutCubic);
    }
}
