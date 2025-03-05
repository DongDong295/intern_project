using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class StageManager : MonoBehaviour
{
    public float RequireDefeatTime;
    public float BossHP;

    public float BossAttackDamage;
    public float BossAttackSpeed;

    public int BossVisualID;


    public async UniTask InitiateStage(int stageIndex){
        StageData data = await Singleton.Of<DataManager>().Load<StageData>(Data.STAGE_DATA);
        var thisStageData = data.stageDataItems[stageIndex];

        RequireDefeatTime = thisStageData.requireDefeatTime;
        BossHP = thisStageData.bossHp;
        BossAttackDamage = thisStageData.bossAttackDamage;
        BossAttackSpeed = thisStageData.bossAttackSpeed;
        BossVisualID = thisStageData.bossVisualId;
        Debug.Log("Finish loading stage");
        Pubsub.Publisher.Scope<UIEvent>().Publish(new ShowScreenEvent(ScreenUI.MAIN_GAMEPLAY_SCREEN, false));
    }

    public void GenerateBossVisual(int visualID){
        //var boss = SingleBehaviour.Of<PoolingManager>().Rent($"boss-visual-{visualID}", true);        
    }

    public void DeInitStage(){

    }
}
