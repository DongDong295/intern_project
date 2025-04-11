using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
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
    private List<string> _heroesIDs;
    private bool _isStageEnd;
    public bool IsWin;
    private GameObject _bossVisualRef;

    private CancellationTokenSource _cts;

    private BuffData _buffData;

    private int index = 0;
    Dictionary<string, Hero> _ownedHeroDict = new Dictionary<string, Hero>();
    Dictionary<string, GameObject> _ownedHeroVisual = new Dictionary<string, GameObject>();
    public GameObject CurrentBuff;

    public async UniTask OnStartApplication(){
        _heroesIDs = new List<string>();
        _ownedHeroDict = SingleBehaviour.Of<PlayerDataManager>().OwnedHeroDict;
        Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnPlayerEquipHero>(OnEquipHero);
        //Pubsub.Subscriber.Scope<PlayerEvent>().Subscribe<OnUnequipHero>(OnUnequipHero);
    }
    public async UniTask InitiateStage(int stageIndex){
        IsWin = false;
        _buffData = await Singleton.Of<DataManager>().Load<BuffData>(Data.BUFF_DATA);
        _cts = new CancellationTokenSource();
        DisposeList = new List<IDispose>();
        Index = stageIndex;
        _isStageEnd = false;
        StageDeltaTime = Time.deltaTime;
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

    public async UniTask OnEquipHero(OnPlayerEquipHero e){    
        if(e.IsEquip){
            if(!_ownedHeroVisual.ContainsKey(e.HeroID)){
                var visual = await SingleBehaviour.Of<PoolingManager>().Rent($"hero-visual-{e.Hero.heroVisualID}");
                visual.transform.position = _heroPositions[index].transform.position;
                _ownedHeroVisual.Add(e.HeroID, visual);
                _heroesIDs.Add(e.HeroID);
                index++;
            }
        }
        else{
            OnUnequipHero(e.HeroID);
        }
    }

    public void OnUnequipHero(string HeroID){
        if(_ownedHeroVisual.ContainsKey(HeroID)){
            var visual = _ownedHeroVisual[HeroID];
            visual.GetComponent<HeroBehaviour>().UnloadHero();
            _ownedHeroVisual.Remove(HeroID);
            _heroesIDs.Remove(HeroID);
        }
        index--;
        RePositionHero();
    }

    public void RePositionHero(){
        for(int i = 0; i < _ownedHeroVisual.Count; i++){
            var heroVisual = _ownedHeroVisual.ElementAt(i).Value;
            heroVisual.transform.position = _heroPositions[i].transform.position;
        }
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
        var damageText = await SingleBehaviour.Of<PoolingManager>().Rent("damage-text-object");
        damageText.transform.position = _bossPosition.position + new Vector3(UnityEngine.Random.Range(-2, 2), 0, -3);
        damageText.GetComponent<DamageText>().InitializeText(damage, isCrit);
    }

    public async UniTask LoadStageVisual(){
        _bossVisualRef = await SingleBehaviour.Of<PoolingManager>().Rent("boss-visual-" + BossVisualID);
        _bossVisualRef.transform.position = _bossPosition.transform.position;
        //var ownedHeroVisualList = _ownedHeroVisual.Values.ToList();
        //Debug.Log("Owned Hero Visual Count: " + ownedHeroVisualList.Count);
        for(int i = 0; i < _heroesIDs.Count; i++){
            var heroIndex = i;
            var heroVisual = _ownedHeroVisual[_heroesIDs[i]];
            var heroData = _ownedHeroDict[_heroesIDs[i]];
            //Debug.Log("hero-visual-" + _heroes[index].heroVisualID);
            //var heroVisual = await SingleBehaviour.Of<PoolingManager>().Rent("hero-visual-" + _heroes[index].heroVisualID);
            //heroVisual.transform.position = _heroPositions[index].transform.position;
            await heroVisual.GetComponent<HeroBehaviour>().InitiateHero(heroData);
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
            if(CurrentBuff != null)
                return;
            var selectedBuff = _buffData.items[UnityEngine.Random.Range(0, _buffData.items.Length)];
            var buff = await SingleBehaviour.Of<PoolingManager>().Rent("buff-sphere");
            CurrentBuff = buff;
            buff.transform.position = new Vector3(0, 3, -9);
            await buff.GetComponent<BuffSphere>().InitiateBuff(selectedBuff);
        }
    }
}
