using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;
using ZBase.UnityScreenNavigator.Core.Modals;

public class StageSelectionModal : BasicModal
{
    private StageData _stageData;
    [SerializeField] private GridUnlimitedScroller _scroller;
    public override UniTask Initialize(Memory<object> args)
    {
        base.Initialize(args);
        _stageData = Singleton.Of<StageData>();
        return UniTask.CompletedTask;
    }

    public async UniTask GenerateButton(){
        var stageList = _stageData.stageDataItems;
        var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("ui-stage-button");

        // Generate the scroller with the button prefab and the total count of stage data items
        _scroller.Generate(buttonPrefab, stageList.Count(), OnCellGenerate);
    }

    private void OnCellGenerate(int index, ICell cell)
    {
        var stageData = _stageData.stageDataItems[index];
    }
}