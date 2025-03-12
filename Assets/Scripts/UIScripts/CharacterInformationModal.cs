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
            await GenerateHeroButton();
        });

        // Generate the hero buttons
        await GenerateHeroButton();
    }

    public async UniTask GenerateHeroButton()
    {
        // Rent the button prefab from the PoolingManager
        var heroButtonPref = await SingleBehaviour.Of<PoolingManager>().Rent("ui-hero-information-button");

        // Get the count of heroes from the dictionary (key-value pairs)
        int heroCount = _ownedHeroDict.Count;

        // Generate the buttons based on the count of owned heroes
        _scroller.Generate(heroButtonPref, heroCount, OnGenerateButton);
    }

    public void OnGenerateButton(int index, ICell cell)
    {
        // Get the button cell and the hero ID at the current index
        var button = cell as RegularCell;
        var heroButton = button.gameObject;
        //heroButton.GetComponent<HeroInformationButton>().InitiateButton(_referenceHero);

        // Set up the button click listener
        heroButton.GetComponent<Button>().onClick.AddListener(async () => {
            // Show the hero information modal and wait for the modal to be displayed
            Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.HERO_INFORMATION_MODAL, false));       
            await WaitForHeroModal(index);
        });
    }

    public async UniTask WaitForHeroModal(int index)
    {
        // Wait until the current modal is of type HeroInformationModal
        await UniTask.WaitUntil(() =>
        {
            var currentModal = SingleBehaviour.Of<UIManager>().GetCurrentModal();
            return currentModal is HeroInformationModal;
        });

        // Publish the event to show hero information
        string heroID = new List<string>(_ownedHeroDict.Keys)[index];
        _referenceHero = _ownedHeroDict[heroID];
        Pubsub.Publisher.Scope<UIEvent>().Publish(new OnShowHeroInformationEvent(_referenceHero));

        // Debugging information
    }
}
