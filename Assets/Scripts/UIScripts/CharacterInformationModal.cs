using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;

public class CharacterInformationModal : BasicModal
{
    [SerializeField] private GridUnlimitedScroller _scroller;
    [SerializeField] private Button _gachaButton;

    public override async UniTask Initialize(Memory<object> arg)
    {
        await base.Initialize(arg);
        _gachaButton.onClick.AddListener(async () => 
        {
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnGachaEvent());
            Debug.Log(SingleBehaviour.Of<PlayerDataManager>().OwnedHero.Count);
            await GenerateHeroButton();
        });
        await GenerateHeroButton();
    }

    public async UniTask GenerateHeroButton()
    {
        var heroButtonPref = await SingleBehaviour.Of<PoolingManager>().Rent("ui-hero-information-button");
        var ownedHero = SingleBehaviour.Of<PlayerDataManager>().OwnedHero;

        _scroller.Generate(heroButtonPref, 1, OnGenerateButton);
    }

    public void OnGenerateButton(int index, ICell cell)
    {
    }
}