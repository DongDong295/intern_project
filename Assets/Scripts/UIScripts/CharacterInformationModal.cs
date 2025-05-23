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

    private Dictionary<string, Hero> _ownedHeroDict;
    private Hero _referenceHero;

    public override async UniTask Initialize(Memory<object> arg)
    {
        // Access the dictionary of owned heroes from PlayerDataManager
        _ownedHeroDict = SingleBehaviour.Of<PlayerDataManager>().OwnedHero;

        await base.Initialize(arg);
        
        // Set up the Gacha button listener
        _gachaButton.onClick.AddListener(async () => 
        {
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnGachaEvent());
            Debug.Log(SingleBehaviour.Of<PlayerDataManager>().OwnedHero.Count);
            //await GenerateHeroButton();
        });

        // Generate the hero buttons
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnGenerateHero>(RefreshHeroListWhenGenerate);
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnUpgradeHero>(RefreshHeroList);
        await GenerateHeroButton();
    }

    public async UniTask GenerateHeroButton()
    {
        // Rent the button prefab from the PoolingManager
        if(_scroller.Generated)
            _scroller.Clear();
        var heroButtonPref = await SingleBehaviour.Of<PoolingManager>().Rent("ui-hero-information-button");
        int heroCount = _ownedHeroDict.Count;
        _scroller.Generate(heroButtonPref, heroCount, OnGenerateButton);
    }

    public async UniTask RefreshHeroList(OnUpgradeHero e)
    {
        await GenerateHeroButton();
    }

    public async UniTask RefreshHeroListWhenGenerate(OnGenerateHero e)
    {
        await GenerateHeroButton();
    }

    public void OnGenerateButton(int index, ICell cell)
    {
        // Get the button cell and the hero ID at the current index
        var button = cell as RegularCell;
        var heroButton = button.gameObject;
        string heroID = new List<string>(_ownedHeroDict.Keys)[index];
        _referenceHero = _ownedHeroDict[heroID];
        heroButton.GetComponent<HeroInformationButton>().InitiateButton(_referenceHero);
        heroButton.GetComponent<Button>().onClick.AddListener(async () => {
            // Show the hero information modal and wait for the modal to be displayed
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.HERO_INFORMATION_MODAL, false));       
            await WaitForHeroModal(index);
        });
    }

    public async UniTask WaitForHeroModal(int index)
    {
        await UniTask.WaitUntil(() =>
        {
            var currentModal = SingleBehaviour.Of<UIManager>().GetCurrentModal();
            return currentModal is HeroInformationModal;
        });
        string heroID = new List<string>(_ownedHeroDict.Keys)[index];
        var referenceHero = _ownedHeroDict[heroID];
        Pubsub.Publisher.Scope<UIEvent>().Publish(new OnShowHeroInformationEvent(referenceHero));
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _scroller.Clear();
        _gachaButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
