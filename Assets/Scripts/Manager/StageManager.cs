using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class StageManager : MonoBehaviour
{
    public float RequireDefeatTime;
    public float BossHP;

    public float BossAttackDamage;
    public float BossAttackSpeed;

    public int BossVisualID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateStage(StageDataItems data)
    {
        RequireDefeatTime = data.RequireDefeatTime;
        BossHP = data.BossHP;
        BossAttackDamage = data.BossAttackDamage;
        BossAttackSpeed = data.BossAttackSpeed;
        BossVisualID = data.BossVisualID;
        GenerateBossVisual(BossVisualID);
    }

    public void GenerateBossVisual(int visualID){
        var boss = SingleBehaviour.Of<PoolingManager>().Rent($"boss-visual-{visualID}", true);        
    }
}
