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
        _equipButton.onClick.AddListener(() => { 
            SingleBehaviour.Of<PlayerDataManager>().EquipHero(_currentHero);});
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
        _currentHero = e.HeroRef;
         _heroOption.Remove(_currentHero.heroID);
        _damage.text = "Damage step: " + e.HeroRef.attackDamageStep.ToString();
        _critChance.text = "Crit chance: " + e.HeroRef.critChance.ToString();
        _attackSpeed.text = "Attack speed: " + e.HeroRef.attackSpeed.ToString();
        _cooldownGenerate.text = "CD generate: " + e.HeroRef.cooldownGenerate.ToString();
        _expDisplay.text = $"{_currentHero.exp}/{_currentHero.GetHeroRequireExp()}";
        _expFillBar.fillAmount = _currentHero.exp / _currentHero.GetHeroRequireExp();
        await GenerateMaterialButton();
    }

    public async UniTask LevelUpHero(){
        float totalExp = 0;
        if(_materials.IsNullOrEmpty())
            return;
        foreach(var h in _materials){
            totalExp += h.GetHeroExpValue();
            _dataManager.RemoveHero(h);
        }
        _currentHero.LevelUpHero(totalExp);
        await GenerateMaterialButton();
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
        _equipButton.onClick.RemoveAllListeners();
        _materials.Clear();
        return base.Cleanup(args);
    }
}
