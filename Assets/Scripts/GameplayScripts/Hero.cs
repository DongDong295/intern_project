using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public string heroID;
    public float hpStep;
    public float attackDamageStep;
    public float critChance;
    public float cooldownGenerate;
    public float attackSpeed;
    public float moveSpeed;
    public float killDamage;

    public float expStep;
    public float expBasic;

    public bool isEquipped;
    public Hero(){

    }

    public Hero(String heroID, HeroDataItems data)
    {
        this.heroID = heroID;
        hpStep = data.hpStep;
        attackDamageStep = data.attackDamageStep;
        critChance = data.critChance;
        cooldownGenerate = data.cooldownGenerate;
        attackSpeed = data.attackSpeed;
        moveSpeed = data.moveSpeed;
        killDamage = data.killDamage;
        expStep = data.expStep;
        expBasic = data.expBasic;
    }
}
