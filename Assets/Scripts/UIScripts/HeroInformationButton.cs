using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;

public class HeroInformationButton : RegularCell
{
    private string _buttonHeroID;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private GameObject equipStatus; 
    public void InitiateButton(Hero heroRef){
        level.text = heroRef.level.ToString();
        _buttonHeroID = heroRef.heroID;
        if(heroRef.isEquipped){
            equipStatus.SetActive(true);
        }
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnPlayerEquipHero>(OnEquipHero);
    }

    public void OnEquipHero(OnPlayerEquipHero e){
        if(_buttonHeroID == e.HeroID){
            Debug.Log("Hi im " + _buttonHeroID + e.IsEquip);
            equipStatus.SetActive(e.IsEquip);
        }
    }
}
