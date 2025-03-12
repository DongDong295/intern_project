using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;

public class HeroButton : RegularCell
{
    [SerializeField] private TextMeshProUGUI level;
    public void InitiateButton(Hero heroRef){
        level.text = heroRef.level.ToString();
    }
}
