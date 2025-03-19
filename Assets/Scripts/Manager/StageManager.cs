using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ZBase.Foundation.Pooling;
using ZBase.Foundation.Singletons;

public class StageManager : MonoBehaviour
{
    public int Index;
    public float RequireDefeatTime;
    public float BossHP;

    public float BossAttackDamage;
    public float BossAttackSpeed;

    public int BossVisualID;

    public float StageDeltaTime;
    [SerializeField] private List<Transform> _heroPositions;
    [SerializeField] private Transform _bossPosition;

    public List<IDispose> DisposeList;
    private List<Hero> _heroes;
    private bool _isStageEnd;
    public bool IsWin;
    private GameObject _bossVisualRef;

    private CancellationTokenSource _cts;

    private BuffData _buffData;
    public async UniTask InitiateStage(int stageIndex){
        IsWin = false;
        _buffData = await Singleton.Of<DataManager>().Load<BuffData>(Data.BUFF_DATA);
        _cts = new CancellationTokenSource();
        DisposeList = new List<IDispose>();
        Index = stageIndex;
        _isStageEnd = false;
        StageDeltaTime = Time.deltaTime;
        _heroes = new List<Hero>();
        _heroes = SingleBehaviour.Of<PlayerDataManager>().EquippedHero;
        StageData data = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        var thisStageData = data.stageDataItems[stageIndex];
        RequireDefeatTime = thisStageData.requireDefeatTime;
        BossHP = thisStageData.bossHp;
        BossAttackDamage = thisStageData.bossAttackDamage;
        BossAttackSpeed = thisStageData.bossAttackSpeed;
        BossVisualID = thisStageData.bossVisualId;
        await LoadStageVisual();
        CountdownStage().Forget();
        RandomBuff().Forget();
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_GAMEPLAY_SCREEN, false));
    }

    public GameObject GetBossReference(){
        return _bossVisualRef;
    }

    public Transform GetHeroPosition(int index){
        return _heroPositions[index];
    }   
    public Vector3 GetBossPosition(){
        return _bossPosition.position;
    }   
    public Vector3 GetBossScale(){
        return _bossPosition.localScale;
    }   

    private async UniTask CountdownStage(){
        while(RequireDefeatTime > 0){
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cts.Token);
            RequireDefeatTime--;
        }
        if(!_isStageEnd){
            _isStageEnd = true;
            IsWin = false;
            OnStageEnd();
        }
    }

    public void DealDamage(float damage, bool isCrit){
        if(BossHP > 0){
            BossHP -= damage;
            Pubsub.Publisher.Scope<UIEvent>().Publish(new OnBossTakeDamageEvent());
            DisplayDamageText(damage, isCrit).Forget();
        }
        if(BossHP <= 0 && !_isStageEnd){
            _isStageEnd = true;
            IsWin = true;
            OnStageEnd();
            return;
        }
    }

    public async UniTask DisplayDamageText(float damage, bool isCrit){
        var damageText = await SingleBehaviour.Of<PoolingManager>().Rent("damage-text");
        damageText.transform.position = _bossPosition.position + new Vector3(UnityEngine.Random.Range(-2, 2), 0, -3);
        damageText.GetComponent<DamageText>().InitializeText(damage, isCrit);
    }

    public async UniTask LoadStageVisual(){
        _bossVisualRef = await SingleBehaviour.Of<PoolingManager>().Rent("boss-visual-" + BossVisualID);
        _bossVisualRef.transform.position = _bossPosition.transform.position;
        Debug.Log(_heroes.Count);
        for(int i = 0; i < _heroes.Count; i++){
            var index = i;
            Debug.Log("hero-visual-" + _heroes[index].heroVisualID);
            var heroVisual = await SingleBehaviour.Of<PoolingManager>().Rent("hero-visual-" + _heroes[index].heroVisualID);
            heroVisual.transform.position = _heroPositions[index].transform.position;
            await heroVisual.GetComponent<HeroBehaviour>().InitiateHero(_heroes[index]);
        }
    }

    public void OnStageEnd(){
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.STAGE_END_MODAL, false));
        foreach(var i in DisposeList){
            i.Dispose();
        }
        DisposeList.Clear();
        SingleBehaviour.Of<PoolingManager>().Return(_bossVisualRef);
        _bossVisualRef = null;
        _cts?.Cancel();
    }

    private async UniTask RandomBuff(){
        while(!_isStageEnd){
            await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(10, 15)), cancellationToken: _cts.Token);
            var selectedBuff = _buffData.items[UnityEngine.Random.Range(0, _buffData.items.Length)];
            var buff = await SingleBehaviour.Of<PoolingManager>().Rent("buff-sphere");
            buff.transform.position = new Vector3(0, 3, -9);
            await buff.GetComponent<BuffSphere>().InitiateBuff(selectedBuff);
        }
    }
}
