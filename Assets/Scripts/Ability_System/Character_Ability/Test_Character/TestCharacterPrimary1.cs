using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterPrimary1 : ProjectileAbilityStrategy
{
    [SerializeField] ProjectileAbilityConfig _projectiledConfig;

    [SerializeField] private GameObject _projectileTestPrefab;

    protected override async UniTask SetUpInitializeAbility()
    {
        castPosition = transform.position;
        var bullet = Instantiate(_projectileTestPrefab, transform.position, transform.rotation);
        var direction = (InputManager.Instance.CursorPosition() - transform.position).normalized;
        await bullet.GetComponent<ProjectileStrategy>().Init(_projectiledConfig.items[0], direction);
        await OnFinish();
    }
}
