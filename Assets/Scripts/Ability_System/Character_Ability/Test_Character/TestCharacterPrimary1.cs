using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterPrimary1 : ProjectileAbilityStrategy, IEntityData
{
    protected float cooldown;

    protected CastType castType;

    protected AbilityType abilityType;

    protected Target target;

    [SerializeField] private GameObject _bulletTestPrefab;
    private GameObject _bulletTest;

    public override async UniTask Init(IAbilityData data)
    {
        await base.Init(data);
        cooldown = data.Cooldown;
        castType = data.CastType;
        abilityType = data.AbilityType;
        target = data.Target;
    }

    public override async UniTask InitiateAbility()
    {
        _bulletTest = Instantiate(_bulletTestPrefab, transform.position, transform.rotation);

        await base.InitiateAbility();
        await OnFinish();
    }
}
