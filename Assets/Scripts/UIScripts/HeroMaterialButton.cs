using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class HeroMaterialButton : RegularCell
{   
    [SerializeField] private Sprite[] _icon;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private TextMeshProUGUI _level;
    private bool _isSelected;
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
        _isSelected = false;
        _level.text = "Lv " + heroRef.level.ToString();
        this.GetComponent<Button>().onClick.AddListener(() => { OnClickButton(heroRef);});
    }

    private void OnClickButton(Hero heroRef){
        _isSelected = !_isSelected;
        Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnSelectMaterialHeroForUpgrade(heroRef, _isSelected));
        _selectedImage.gameObject.SetActive(_isSelected);
    }
}
