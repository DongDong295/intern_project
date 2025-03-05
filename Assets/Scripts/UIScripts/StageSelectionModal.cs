using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Modals;

public class StageSelectionModal : BasicModal
{
    [SerializeField] private Transform _stageButtonHolder;
    private List<Button> _stageButtons;
    public override async UniTask Initialize(Memory<object> args)
    {
        _stageButtons = new List<Button>();
        
        await base.Initialize(args);
        await LoadStageList();
    }

    public async UniTask LoadStageList()
    {
        StageData data = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        var stageDataItems = data.stageDataItems;
        var index = 0;
        for(int i = 0; i < stageDataItems.Count(); i++)
        {
            index = i;
            GameObject buttonObject = await SingleBehaviour.Of<PoolingManager>().Rent("ui-stage-button", true);
            buttonObject.transform.localScale = Vector3.one;
            Button button = buttonObject.GetComponent<Button>();
            if (button != null)
            {
                TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = index.ToString();
                }
                 button.onClick.AddListener(() =>
                {
                    Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent(true));

                    Pubsub.Publisher.Scope<PlayerEvent>().
                        Publish<OnEnterGamePlayScene>(new OnEnterGamePlayScene(index));               
                });
            }
            buttonObject.transform.SetParent(_stageButtonHolder, false);
            _stageButtons.Add(button);
        }
    }

    public override async void DidPopExit(Memory<object> args)
    {
        base.DidPopExit(args);

        var buttonsToRemove = new List<Button>(_stageButtons);

        foreach (var button in buttonsToRemove)
        {
            SingleBehaviour.Of<PoolingManager>().Return(button.gameObject);
            _stageButtons.Remove(button);
        }

        buttonsToRemove.Clear();
        await UniTask.CompletedTask;
    }
}