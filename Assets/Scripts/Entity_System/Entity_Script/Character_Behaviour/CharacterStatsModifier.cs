using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsModifier : MonoBehaviour 
{
    public async UniTask IncreaseSpeed(float speed, float duration)
    {
        var movementBehaviour = GetComponent<CharacterMovementBehaviour>();
        movementBehaviour.IncreaseCharacterSpeed(speed);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        movementBehaviour.DecreaseCharacterSpeed(speed);
    }
}


