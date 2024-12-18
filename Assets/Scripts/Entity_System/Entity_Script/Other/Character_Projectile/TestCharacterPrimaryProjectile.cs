using Cysharp.Threading.Tasks;
using Runtime.DataConfig;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using ZBase.UnityScreenNavigator.Core.Sheets;

public class TestCharacterPrimaryProjectile : MonoBehaviour, IEntityUpdate
{
    private float _projectileSpeed;
    private bool _canPierce;
    private float _abilityDamage;
    private bool _canSearchEnemy;
    private Vector3 _moveDirection;
    private float _projectileRange;
    private float _searchRange;

    Rigidbody2D _rb;
    private TestCharacterPrimary1 _owner;
    private Vector3 _castPosition;
    private CancellationTokenSource _tokenSource;

    public async UniTask InitProjectile(ProjectileAbilityConfigItem data, Vector3 castPosition, Vector3 direction, TestCharacterPrimary1 owner)
    {
        _projectileRange = data.projectileRange;
        _projectileSpeed = data.projectileSpeed;
        _canPierce = data.canPierce;
        _canSearchEnemy = data.canSearchEnemy;
        _moveDirection = direction;
        _abilityDamage = data.abilityDamage;
        _searchRange = data.searchRange;
        _owner = owner;
        _castPosition = castPosition;
        _rb = GetComponent<Rigidbody2D>();
        EntityManager.Instance.UpdateEntity.Add(this);
        await UniTask.CompletedTask;
    }

    public void OnUpdate(float deltaTime)
    {
        Check();
        Move(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        //Move(fixedDeltaTime);
    }

    public void Move(float deltaTime)
    {
        //transform.position = transform.position + _moveDirection.normalized * _projectileSpeed * deltaTime;
        _rb.MovePosition(_rb.position + (Vector2)_moveDirection.normalized * _projectileSpeed * deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (!_canPierce)
                {
                    var enemy = collision.gameObject.GetComponent<EntityHolder>();
                    _owner.AddHitList(enemy);
                    DeInit().Forget();
                }
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                DeInit().Forget();  
            }
            else
            {
                return;
            }
        }
    }

    public void Check()
    {
        if(Vector3.Distance(_castPosition, transform.position) > _projectileRange)
        {
            DeInit().Forget();
        }
    }

    public async UniTask DeInit()
    {
        await _owner.DamageEnemy();
        EntityManager.Instance.UpdateEntity.Remove(this);
        Destroy(gameObject);
    }
}
