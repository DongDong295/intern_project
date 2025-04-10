using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero
{
    public string heroID;

    public int heroVisualID;
    public float hpStep;
    public float attackDamageStep;
    public float critChance;
    public float cooldownGenerate;
    public float attackSpeed;
    public float moveSpeed;
    public float killDamage;

    public float expStep;
    public float expBasic;
    public float exp;
    public int level;

    public bool isEquipped;
    public Hero(){

    }

    public Hero(String heroID, HeroDataItems data)
    {
        this.heroID = heroID;
        heroVisualID = data.heroVisualId;
        hpStep = data.hpStep;
        attackDamageStep = data.attackDamageStep;
        critChance = data.critChance;
        cooldownGenerate = data.cooldownGenerate;
        attackSpeed = data.attackSpeed;
        moveSpeed = data.moveSpeed;
        killDamage = data.killDamage;
        expStep = data.expStep;
        expBasic = data.expBasic;
        exp = 0;
        level = 1;
    }

    public void LevelUpHero(float expValue){
        exp += expValue;
        while(exp >= (level * expStep)){
            Debug.Log(exp + " "+ level * expStep);
            exp -= level * expStep;
            level++;
        }
    }

    public float GetHeroExpValue(){
        return (expStep * level + exp) / 2 + expBasic;
    }

    public float GetHeroRequireExp(){
        return expStep * level;
    }
}

public class MiniHeroData{
    public float MovementSpeed;
    public float Damage;
    public float CritChance;
    public float KillDamage;

    public float AttackSpeed;

    public MiniHeroData(float speed, float damage, float attackSpeed, float critChance, float killDamage){
        MovementSpeed = speed;
        Damage = damage;
        CritChance = critChance;
        AttackSpeed = attackSpeed;
        KillDamage = killDamage;
    }
}
