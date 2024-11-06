using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using Sirenix.Reflection.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterAbilityE1 : SelfBuffAbilityStrategy
{
    [SerializeField] SelfBuffAbilityConfig configs;
    protected async override UniTask SetUpInitializeAbility()
    {
        Init(configs.items[0]);
        var modifer = GetComponent<CharacterStatsModifier>();
        await modifer.IncreaseSpeed(speedBuffAmount, speedDuration);
        await OnFinish();
    }
}
