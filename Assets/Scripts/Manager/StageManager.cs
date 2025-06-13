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

    public string BossName;
    public float StageDeltaTime;
    [SerializeField] private List<Transform> _heroPositions;
    [SerializeField] private Transform _bossPosition;

    [SerializeField] private List<Transform> _displayPosition;

    public List<IDispose> DisposeList;
    private List<string> _heroesIDs;
    private bool _isStageEnd;
    public bool IsWin;
    private GameObject _bossVisualRef;
    private GameObject _mapObject;
    private CancellationTokenSource _cts;

    private BuffData _buffData;
    private StageDataItems _stageData;
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
        _stageData = data.stageDataItems[stageIndex];
        RequireDefeatTime = _stageData.requireDefeatTime;
        BossHP = _stageData.bossHp;
        BossAttackDamage = _stageData.bossAttackDamage;
        BossAttackSpeed = _stageData.bossAttackSpeed;
        BossVisualID = _stageData.bossVisualId;
        BossName = _stageData.bossName;
        await LoadStageVisual();
        CountdownStage().Forget();
        RandomBuff().Forget();
        
        _mapObject = await SingleBehaviour.Of<PoolingManager>().Rent($"map-prefab-{data.stageDataItems[stageIndex].mapPrefabId}");
        _mapObject.transform.position = new Vector3(0, 0, 1);
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_GAMEPLAY_SCREEN, false));
    }

    public GameObject GetBossReference(){
        return _bossVisualRef;
    }

    public async UniTask OnEquipHero(OnPlayerEquipHero e){    
        if(e.IsEquip){
            if(!_ownedHeroVisual.ContainsKey(e.HeroID)){
                var visual = await SingleBehaviour.Of<PoolingManager>().Rent($"hero-visual-{e.Hero.heroVisualID}");
                visual.transform.position = _displayPosition[index].transform.position;
                _ownedHeroVisual.Add(e.HeroID, visual);
                _heroesIDs.Add(e.HeroID);
                visual.GetComponent<HeroBehaviour>().DisableProgressbar();
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
            heroVisual.transform.position = _displayPosition[i].transform.position;
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
            BossHP = 0;
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
            var heroVisual = _ownedHeroVisual[_heroesIDs[heroIndex]];
            var heroData = _ownedHeroDict[_heroesIDs[heroIndex]];
            //Debug.Log("hero-visual-" + _heroes[index].heroVisualID);
            //var heroVisual = await SingleBehaviour.Of<PoolingManager>().Rent("hero-visual-" + _heroes[index].heroVisualID);
            heroVisual.transform.position = _heroPositions[heroIndex].transform.position;
            await heroVisual.GetComponent<HeroBehaviour>().InitiateHero(heroData);
        }
    }


    public void OnStageEnd(){
        if(IsWin){
            SingleBehaviour.Of<PlayerDataManager>().PlayerGem += (int)_stageData.prizeValue;
            Pubsub.Publisher.Scope<PlayerEvent>().Publish(new OnUpdateGem(_stageData.prizeValue));
        }
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowModalEvent(ModalUI.STAGE_END_MODAL, false));
        foreach(var i in DisposeList){
            i.Dispose();
        }
        DisposeList.Clear();
        SingleBehaviour.Of<PoolingManager>().Return(_bossVisualRef);
        SingleBehaviour.Of<PoolingManager>().Return(_mapObject);
        MoveHeroesBackToDisplayPositions();
        _cts?.Cancel();
    }

    private void MoveHeroesBackToDisplayPositions()
    {
        for (int i = 0; i < _heroesIDs.Count; i++)
        {
            if (_ownedHeroVisual.TryGetValue(_heroesIDs[i], out var heroVisual))
            {
                heroVisual.transform.position = _displayPosition[i].position;
            }
        }
    }

    public void OnLogOut()
    {
        foreach (var e in _ownedHeroVisual)
        {
            SingleBehaviour.Of<PoolingManager>().Return(e.Value);
        }
    }

    private async UniTask RandomBuff()
    {
        while (!_isStageEnd)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(10, 15)), cancellationToken: _cts.Token);
            if (CurrentBuff != null)
                return;
            var selectedBuff = _buffData.items[UnityEngine.Random.Range(0, _buffData.items.Length)];
            var buff = await SingleBehaviour.Of<PoolingManager>().Rent("buff-sphere");
            CurrentBuff = buff;
            buff.transform.position = new Vector3(0, 3, -9);
            await buff.GetComponent<BuffSphere>().InitiateBuff(selectedBuff);
        }
    }
}
