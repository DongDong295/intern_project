using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Singletons;
using System.Threading;  // Needed for CancellationToken

public class Bullet : MonoBehaviour, IDispose
{
    private float _speed;
    private float _damage;
    private float _critChance;
    private StageManager _stageManager;
    private float _killDistance;

    [SerializeField] private float _speedMultiplier;

    // CancellationTokenSource for stopping the MoveTowardBoss task
    private CancellationTokenSource _cancellationTokenSource;

    public void InitiateBullet(float speed, float killDistance, float damage, float critChance)
    {
        _stageManager = SingleBehaviour.Of<StageManager>();
        _stageManager.DisposeList.Add(this);
        _damage = damage;
        _speed = speed;
        _critChance = critChance;
        _killDistance = killDistance;

        // Initialize the CancellationTokenSource
        _cancellationTokenSource = new CancellationTokenSource();

        // Start the movement towards the boss with the cancellation token
        MoveTowardBoss(_cancellationTokenSource.Token).Forget();
    }

    private async UniTask MoveTowardBoss(CancellationToken cancellationToken)
    {
        // Continuously move the bullet until it reaches the kill distance or it is cancelled
        while (Vector3.Distance(transform.position, _stageManager.GetBossPosition()) > _killDistance && _stageManager.GetBossReference() != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _stageManager.GetBossPosition(), _speed * _speedMultiplier * _stageManager.StageDeltaTime);
            await UniTask.Yield(cancellationToken: cancellationToken);
        }
        DealDamage(_damage);
    }

    private void DealDamage(float inputDamage)
    {
        var critValue = Random.value;
        Debug.Log(critValue + " " + _critChance / 100 + " Is Crit: " + (critValue <= _critChance / 100));
        if (critValue <= (_critChance / 100))
            inputDamage *= 2;
        
        _stageManager.DealDamage(inputDamage, critValue <= _critChance / 100);
        _cancellationTokenSource?.Cancel();
        SingleBehaviour.Of<PoolingManager>().Return(this.gameObject);
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        SingleBehaviour.Of<PoolingManager>().Return(this.gameObject);
    }
}
