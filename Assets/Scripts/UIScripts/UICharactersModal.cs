using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnlimitedScrollUI;
using ZBase.Foundation.Singletons;

public class UICharactersModal : BasicModal
{    
    [SerializeField] private GridUnlimitedScroller _scroller;
    private HeroData _heroData;
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
        _heroData = await Singleton.Of<DataManager>().Load<HeroData>(Data.HERO_DATA);
    }
}
