using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityStrategy
{
    public abstract void Init(IAbilityData data);
    public void OnUsePrimary()
    {
    }
    public void OnUseQ()
    {

    }
    public void OnUseE()
    {

    }
    public void OnUseUltimate()
    {

    }
}