using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    public async UniTask InitiateStage(int stageIndex){
        StageData data = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        var thisStageData = data.stageDataItems[stageIndex];

        RequireDefeatTime = thisStageData.requireDefeatTime;
        BossHP = thisStageData.bossHp;
        BossAttackDamage = thisStageData.bossAttackDamage;
        BossAttackSpeed = thisStageData.bossAttackSpeed;
        BossVisualID = thisStageData.bossVisualId;
        Debug.Log("Finish loading stage");
        LoadBossVisual(BossVisualID);
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_GAMEPLAY_SCREEN, false));
    }

    public async void LoadBossVisual(int id){
        var boss = await SingleBehaviour.Of<PoolingManager>().Rent("boss-" + id, true);
        boss.transform.position = _bossPosition.position;
    }

    public void InitializeHero(){
        var heroes = new List<Hero>();
    }
}
