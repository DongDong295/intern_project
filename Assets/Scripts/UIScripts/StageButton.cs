using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;

public class StageButton : RegularCell
{
    [SerializeField] private TextMeshProUGUI _displayText;
    private int _index = 0;

    public void OnGenerated(int index)
    {
        _index = index;
        _displayText.text = $"Stage {_index}";
        Debug.Log($"Generated: {index}");
    }

    public new void OnBecomeInvisible(ScrollerPanelSide side)
    {
    }

    public new void OnBecomeVisible(ScrollerPanelSide side)
    {
    }
}
