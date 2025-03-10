using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnlimitedScrollUI;

public interface ICellData
{
    int Index { get; set; }
}

public class CellData : ICellData
{
    public int Index { get; set; }

    public CellData(int index)
    {
        Index = index;
    }
}
