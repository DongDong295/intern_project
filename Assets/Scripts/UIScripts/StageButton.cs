using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnlimitedScrollUI;

public class StageButton : RegularCell
{
    private GenerateEvent onGenerated { get; set; }
    private int index;

    public void OnGenerated(GenerateEvent onGenerated)
    {
        Debug.Log("Hi");
        this.onGenerated = onGenerated;
        var displayText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        displayText.text = $"Stage {index}";
    }
    public new void OnBecomeInvisible(ScrollerPanelSide side)
    {
    }

    public new void OnBecomeVisible(ScrollerPanelSide side)
    {
    }
}
