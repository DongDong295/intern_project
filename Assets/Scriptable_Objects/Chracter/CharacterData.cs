using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character Data")]
public class CharacterData : ScriptableObject
{
    public float Speed;
    public float DashRange;
    public float DashSpeed;
    public float DashDuration;
}
