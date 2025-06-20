using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;

public class HeroInformationModal : BasicModal
{
    [SerializeField] private Image _display;
    [SerializeField] private Sprite[] _icon;
    [SerializeField] private GridUnlimitedScroller _scroller;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _critChance;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _cooldownGenerate;
    [SerializeField] private Image _expFillBar;
    [SerializeField] private TextMeshProUGUI _expDisplay;

    private Hero _currentHero;
    private Dictionary<string, Hero> _heroOption;
    private List<Hero> _materials;

    private PlayerDataManager _dataManager;
    public override async UniTask Initialize(Memory<object> arg)
    {
        _dataManager = SingleBehaviour.Of<PlayerDataManager>();
        _heroOption = _dataManager.GetUnequippedHeroDict();
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnSelectMaterialHeroForUpgrade>(SelectMaterial);
        Pubsub.Subscriber.Scope<UIEvent>().Subscribe<OnShowHeroInformationEvent>(DisplayHeroInformation);
        await base.Initialize(arg);
        _materials = new List<Hero>();
        _equipButton.onClick.AddListener(async () => { 
            await SingleBehaviour.Of<PlayerDataManager>().EquipHero(_currentHero);});
        ; 
        _levelUpButton.onClick.AddListener(() => {LevelUpHero().Forget();});
    }

    public async UniTask GenerateMaterialButton(){
        var heroButtonPref = await SingleBehaviour.Of<PoolingManager>().Rent("ui-hero-material-button");
        _heroOption = _dataManager.GetUnequippedHeroDict(_currentHero);
        _scroller.Clear();
        _scroller.Generate(heroButtonPref, _heroOption.Count, OnGenerateButton);
        await UniTask.CompletedTask;
    }

    public void OnGenerateButton(int index, ICell cell)
    {
        var button = cell as RegularCell;
        var matButton = button.gameObject.GetComponent<HeroMaterialButton>();
        string heroID = new List<string>(_heroOption.Keys)[index];
        var referenceHero = _heroOption[heroID];
        matButton.InitiateButton(referenceHero);
    }

    public async UniTask DisplayHeroInformation(OnShowHeroInformationEvent e){
        switch(e.HeroRef.heroVisualID){
            case 0:
                _display.sprite = _icon[0];
                break;
            case 1:
                _display.sprite = _icon[1];
                break;
            case 2:
                _display.sprite = _icon[2];
                break;
        }
        _currentHero = e.HeroRef;
         _heroOption.Remove(_currentHero.heroID);
        _damage.text = _damage.text + ": " + e.HeroRef.attackDamageStep.ToString();
        _critChance.text = _critChance.text + ": " + e.HeroRef.critChance.ToString();
        _attackSpeed.text = _attackSpeed.text + ": " + e.HeroRef.attackSpeed.ToString();
        _cooldownGenerate.text = _cooldownGenerate.text + ": " + e.HeroRef.cooldownGenerate.ToString();
        _expDisplay.text = $"{_currentHero.exp}/{_currentHero.GetHeroRequireExp()}";
        _expFillBar.fillAmount = _currentHero.exp / _currentHero.GetHeroRequireExp();
        await GenerateMaterialButton();
    }

    public async UniTask LevelUpHero()
    {
        float totalExp = 0;
        if (_materials.IsNullOrEmpty())
            return;

        // Create a temporary list to store the heroes to be removed
        var heroesToRemove = new List<Hero>(_materials);

        // Iterate through the temporary list
        foreach (var h in heroesToRemove)
        {
            totalExp += h.GetHeroExpValue();
            await _dataManager.RemoveHero(h);  // Remove hero from the original collection
        }

        _currentHero.LevelUpHero(totalExp);
        _materials.Clear();  // Clear the original materials list
        UpdateHeroExpUI();
        await GenerateMaterialButton();
        Pubsub.Publisher.Scope<PlayerEvent>().Publish<OnUpgradeHero>(new OnUpgradeHero());
    }

    private void UpdateHeroExpUI(){
        _expDisplay.text = $"{_currentHero.exp}/{_currentHero.GetHeroRequireExp()}";
        _expFillBar.fillAmount = _currentHero.exp / _currentHero.GetHeroRequireExp();
    }

    public void SelectMaterial(OnSelectMaterialHeroForUpgrade e){
        if(!_materials.Contains(e.MaterialHero) && e.IsSelected)
            _materials.Add(e.MaterialHero);
        else
            _materials.Remove(e.MaterialHero);
    }

    public override UniTask Cleanup(Memory<object> args)
    {
        _scroller.Clear();
        _materials.Clear();
        _levelUpButton.onClick.RemoveAllListeners();
        _equipButton.onClick.RemoveAllListeners();
        return base.Cleanup(args);
    }
}
