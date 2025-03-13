using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Pooling;
using ZBase.Foundation.Singletons;

public class StageManager : MonoBehaviour
{
    public float RequireDefeatTime;
    public float BossHP;

    public float BossAttackDamage;
    public float BossAttackSpeed;

    public int BossVisualID;

    
    [SerializeField] private List<Transform> _heroPositions;
    [SerializeField] private Transform _bossPosition;

    private List<Hero> _heroes;
    public async UniTask InitiateStage(int stageIndex){
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
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_GAMEPLAY_SCREEN, false));
    }

    public Transform GetHeroPosition(int index){
        return _heroPositions[index];
    }   

    public async UniTask LoadStageVisual(){
        var bossVisual = await SingleBehaviour.Of<PoolingManager>().Rent("boss-visual-" + BossVisualID);
        bossVisual.transform.position = _bossPosition.transform.position;
        Debug.Log(_heroes.Count);
        for(int i = 0; i < _heroes.Count; i++){
            var index = i;
            Debug.Log("hero-visual-" + _heroes[index].heroVisualID);
            var heroVisual = await SingleBehaviour.Of<PoolingManager>().Rent("hero-visual-" + _heroes[index].heroVisualID);
            heroVisual.transform.position = _heroPositions[index].transform.position;
            heroVisual.GetComponent<HeroBehaviour>().InitiateHero(_heroes[index]);
        }
    }
}
