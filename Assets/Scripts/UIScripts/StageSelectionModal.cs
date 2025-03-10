using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Modals;

public class StageSelectionModal : BasicModal
{
    [SerializeField] private GridUnlimitedScroller _scroller;
    private StageData _stageData;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _stageData = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        GenerateButton().Forget();
    }

public async UniTask GenerateButton()
    {
        var buttonPrefab = await SingleBehaviour.Of<PoolingManager>().Rent("ui-stage-button");
        _scroller.Generate(buttonPrefab, _stageData.stageDataItems.Length, OnCellGenerate);
        Debug.Log("Button Generated");
    }

    private void OnCellGenerate(int index, ICell cell)
    {
        //var stageData = _stageData.stageDataItems[index];
        
        var button = cell as RegularCell;

        var buttonObj = button.gameObject.GetComponent<Button>();
        buttonObj.onClick.AddListener(() =>
        {
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnEnterGamePlayScene(index));
            Pubsub.Publisher.Scope<UIEvent>().Publish(new CloseModalEvent());
        });

        var regularCell = cell as RegularCell;
        if (regularCell != null)
        {
            regularCell.onGenerated.AddListener((cellIndex) =>
            {
                var stageButton = cell as StageButton;
                if (stageButton != null)
                {
                    stageButton.OnGenerated(cellIndex);
                }
            });
            regularCell.onGenerated.Invoke(index);
        }
    }
}