using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class MainGameplayScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private TextMeshProUGUI _bossHPText;  
    [SerializeField] private TextMeshProUGUI _bossIDText;

    private float _bossMaxHP;

    public override UniTask Initialize(Memory<object> args)
    {
        _bossMaxHP = SingleBehaviour.Of<StageManager>().BossHP;
        DisplayBossData();
        base.Initialize(args);
        return UniTask.CompletedTask;
    }

    private void DisplayBossData(){
        _bossHPText.text = $"{SingleBehaviour.Of<StageManager>().BossHP.ToString()} / {_bossMaxHP.ToString()}";
        _bossIDText.text = SingleBehaviour.Of<StageManager>().BossVisualID.ToString();
    }
}
