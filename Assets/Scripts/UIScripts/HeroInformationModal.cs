using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;

public class HeroInformationModal : BasicModal
{
    [SerializeField] private GridUnlimitedScroller _scroller;
    [SerializeField] private Button EquipButton;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _critChance;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _cooldownGenerate;

    private Hero _currentHero;
    public override async UniTask Initialize(Memory<object> arg)
    {
        await base.Initialize(arg);
        EquipButton.onClick.AddListener(() => SingleBehaviour.Of<StageManager>().EquipHero(_currentHero));
        Pubsub.Subscriber.Scope<UIEvent>().Subscribe<OnShowHeroInformationEvent>(DisplayHeroInformation);
    }

    public void DisplayHeroInformation(OnShowHeroInformationEvent e){
        _currentHero = e.HeroRef;
        _damage.text = "Damage step: " + e.HeroRef.attackDamageStep.ToString();
        _critChance.text = "Crit chance: " + e.HeroRef.critChance.ToString();
        _attackSpeed.text = "Attack speed: " + e.HeroRef.attackSpeed.ToString();
        _cooldownGenerate.text = "Cooldown generate: " + e.HeroRef.cooldownGenerate.ToString();
    }
}
