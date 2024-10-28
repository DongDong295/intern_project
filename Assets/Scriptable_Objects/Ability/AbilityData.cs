using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "New Ability Data")]
public class AbilityData : ScriptableObject, IAbilityData
{
    public float cooldown;
    public CastType castType;
    public AbilityType abilityType;
    public Target target;
    public float Cooldown => cooldown;
    public CastType CastType => castType;
    public AbilityType AbilityType => abilityType;
    public Target Target => target;
}
