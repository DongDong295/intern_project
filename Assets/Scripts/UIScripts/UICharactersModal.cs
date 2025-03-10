using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;

public class UICharactersModal : BasicModal
{    
    [SerializeField] private GridUnlimitedScroller _scroller;
    private HeroData _heroData;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _heroData = await Singleton.Of<DataManager>().Load<HeroData>(Data.HERO_DATA);
    }

    public async UniTask GenerateHeroInformationButton(){
        var buttonPrefab = await SingleBehaviour.Of<PoolingManager>().Rent("ui-hero-information-button");
            _scroller.Generate(buttonPrefab, _heroData.heroDataItems.Length, (index, iCell) => {
                var regularCell = iCell as RegularCell;
                if (regularCell != null) regularCell.onGenerated?.Invoke(index);
                var button = regularCell.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                });
            });
    }
}
