using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Singletons;

public class MainGameplayScreen : ZBase.UnityScreenNavigator.Core.Screens.Screen
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _bossHPText;  
    [SerializeField] private TextMeshProUGUI _bossNameText;
    [SerializeField] private Image _healthDisplayer;
    private float _bossMaxHP;

    private StageManager _stageManager;

    public override async UniTask Initialize(Memory<object> args)
    {
        await SingleBehaviour.Of<AudioManager>().PlayMusic("music-battle");
        _stageManager = SingleBehaviour.Of<StageManager>();
        _bossMaxHP = _stageManager.BossHP;
        DisplayBossData();
        base.Initialize(args);
        Pubsub.Subscriber.Scope<UIEvent>().Subscribe<OnBossTakeDamageEvent>(UpdateHealthBar);
        DisplayTime().Forget();
        _mainMenuButton.onClick.AddListener(() => {
            _stageManager.OnStageEnd();
        });
    }

    public void UpdateHealthBar(OnBossTakeDamageEvent e){
        _bossHPText.text = $"{_stageManager.BossHP.ToString()} / {_bossMaxHP.ToString()}";
        _healthDisplayer.fillAmount = _stageManager.BossHP/_bossMaxHP;
    }

    private void DisplayBossData(){
        _bossHPText.text = $"{_stageManager.BossHP.ToString()} / {_bossMaxHP.ToString()}";
        _bossNameText.text = _stageManager.BossName.ToString();
    }

    public async UniTask DisplayTime(){
        while(_stageManager.RequireDefeatTime > 0){
            _time.text = _stageManager.RequireDefeatTime.ToString();
            await UniTask.Yield();
        }
    }
}
