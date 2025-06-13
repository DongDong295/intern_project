using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Singletons;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;  // Needed for CancellationToken

public class Bullet : MonoBehaviour, IDispose
{
    private float _speed;
    private float _damage;
    private float _critChance;
    private StageManager _stageManager;
    private float _killDistance;
    private bool _isDealtDamage;

    [SerializeField] private float _speedMultiplier;

    // CancellationTokenSource for stopping the MoveTowardBoss task
    private CancellationTokenSource _cancellationTokenSource;
    public async UniTask InitiateBullet(float speed, float killDistance, float damage, float critChance)
    {
        _isDealtDamage = false;
        _stageManager = SingleBehaviour.Of<StageManager>();
        _stageManager.DisposeList.Add(this);
        _damage = damage;
        _speed = speed;
        _critChance = critChance;
        _killDistance = killDistance;

        // Initialize the CancellationTokenSource
        _cancellationTokenSource = new CancellationTokenSource();

        // Start the movement towards the boss with the cancellation token
        await MoveTowardBoss(_cancellationTokenSource.Token);
    }

    private async UniTask MoveTowardBoss(CancellationToken cancellationToken)
    {
        while (Vector3.Distance(transform.position, _stageManager.GetBossPosition()) > _killDistance && _stageManager.GetBossReference() != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _stageManager.GetBossPosition(), _speed * _speedMultiplier * _stageManager.StageDeltaTime);
            await UniTask.Yield(cancellationToken: cancellationToken);
        }
        if(!_isDealtDamage)
            DealDamage(_damage);
    }

    private void DealDamage(float inputDamage)
    {
        _isDealtDamage = true;
        var critValue = Random.value;
        if (critValue <= (_critChance / 100))
            inputDamage *= 2;
        
        _stageManager.DealDamage(inputDamage, critValue <= _critChance / 100);
        BulletHitVfx().Forget();
        _cancellationTokenSource?.Cancel();
        SingleBehaviour.Of<PoolingManager>().Return(this.gameObject);
    }

    public async UniTask BulletHitVfx()
    {
        var vfx = await SingleBehaviour.Of<PoolingManager>().Rent("bullet-hit-vfx");
        vfx.transform.position = transform.position;

        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            // Wait until the particle system finishes
            await UniTask.WaitUntil(() => !ps.IsAlive(true));
        }

        SingleBehaviour.Of<PoolingManager>().Return(vfx);
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        SingleBehaviour.Of<PoolingManager>().Return(this.gameObject);
    }
}
