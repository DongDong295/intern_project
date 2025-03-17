using System;
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

    private IDisposable _equipHeroSubscription;  // Store the subscription to unsubscribe later

    public void InitiateButton(Hero heroRef)
    {
        level.text = "Lv " + heroRef.level.ToString();
        _buttonHeroID = heroRef.heroID;
        if (heroRef.isEquipped)
        {
            equipStatus.SetActive(true);
        }

        // Subscribe to the event and store the subscription to unsubscribe later
        _equipHeroSubscription = Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnPlayerEquipHero>(OnEquipHero);
    }

    public void OnEquipHero(OnPlayerEquipHero e)
    {
        if (_buttonHeroID == e.HeroID)
        {
            equipStatus.SetActive(e.IsEquip);
        }
    }

    // Unsubscribe when the object is destroyed
    public void OnDestroy()
    {
        if (_equipHeroSubscription != null)
        {
            // Assuming that the subscription token (or IDisposable) has a method to unsubscribe
            _equipHeroSubscription.Dispose(); // This should remove the subscription if it's IDisposable
        }
    }
}
