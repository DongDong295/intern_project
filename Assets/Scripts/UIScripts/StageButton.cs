using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnlimitedScrollUI;

public class StageButton : MonoBehaviour, ICell
{
    public void InitiateButton()
    {
    
    }

    public void OnBecomeInvisible(ScrollerPanelSide side)
    {
        throw new System.NotImplementedException();
    }

    public void OnBecomeVisible(ScrollerPanelSide side)
    {
        throw new System.NotImplementedException();
    }

    public class StageCellData : CellData
    {
        public int ButtonIndex;
        public StageCellData(int index) : base(index)
        {
            ButtonIndex = index;
        }
    }
}
