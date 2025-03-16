using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class HeroMaterialButton : RegularCell
{   
    [SerializeField] private Image _selectedImage;
    private bool _isSelected;
    public void InitiateButton(Hero heroRef){
        _isSelected = false;
        this.GetComponent<Button>().onClick.AddListener(() => { OnClickButton(heroRef);});
    }

    private void OnClickButton(Hero heroRef){
        _isSelected = !_isSelected;
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnSelectMaterialHeroForUpgrade(heroRef, _isSelected));
        _selectedImage.gameObject.SetActive(_isSelected);
    }
}
