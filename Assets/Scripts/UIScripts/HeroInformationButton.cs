using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;
using ZBase.Foundation.PubSub;

public class HeroInformationButton : RegularCell
{
    private string _buttonHeroID;
    [SerializeField] private Sprite[] _icon;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private GameObject equipStatus; 

    private ISubscription _sub;
    public void InitiateButton(Hero heroRef){
        switch(heroRef.heroVisualID){
            case 0:
                GetComponent<Button>().image.sprite = _icon[0];
                break;
            case 1:
                GetComponent<Button>().image.sprite = _icon[1];
                break;
            case 2:
                GetComponent<Button>().image.sprite = _icon[2];
                break;
        }
        level.text = "Lv " + heroRef.level.ToString();
        _buttonHeroID = heroRef.heroID;
        if(heroRef.isEquipped){
            equipStatus.SetActive(true);
        }
        _sub = Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnPlayerEquipHero>(OnEquipHero);
    }

    public void OnEquipHero(OnPlayerEquipHero e){
        if(_buttonHeroID == e.HeroID){
            equipStatus.SetActive(e.IsEquip);
        }
    }

    public void OnDestroy()
    {
        if(_sub != null){
            _sub.Unsubscribe();
            _sub.Dispose();   
        }
    }
}
