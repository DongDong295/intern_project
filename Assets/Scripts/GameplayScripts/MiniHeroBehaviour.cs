using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Singletons;

public class MiniHeroBehaviour : MonoBehaviour, IDispose
{
    private float damage;
    private float moveSpeed;
    private float critChance;
    private float killDamage;

    private float attackSpeed;
    private bool _isReachedTarget;
    private Vector3 _bossPosition;
    private float _killDistance;
    private StageManager _stageManager;

    private CancellationTokenSource _cts; // Token to cancel tasks

    public void InitiateMiniHero(MiniHeroData data){
        _stageManager = SingleBehaviour.Of<StageManager>();
        _stageManager.DisposeList.Add(this);
        _cts = new CancellationTokenSource(); // Initialize the cancellation token
        _isReachedTarget = false;
        damage = data.Damage;
        moveSpeed = data.MovementSpeed;
        critChance = data.CritChance;
        killDamage = data.KillDamage;
        attackSpeed = data.AttackSpeed;
        _bossPosition = _stageManager.GetBossPosition();
        _bossPosition.y = 0;
        _killDistance = _stageManager.GetBossScale().z;
        
        MoveToBoss().Forget();
        AttackBoss().Forget();
    }

    public async UniTask MoveToBoss()
    {
        while (Vector3.Distance(transform.position, _bossPosition) > _killDistance)
        {
            // If the boss hasn't been reached, keep moving towards the boss
            _isReachedTarget = false;
        
            transform.position = Vector3.MoveTowards(transform.position, _bossPosition, moveSpeed * _stageManager.StageDeltaTime);
            await UniTask.Yield(cancellationToken: _cts.Token);
        }
        TriggerKillDamage();
    }

    public async UniTask AttackBoss(){
        while (!_isReachedTarget)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(attackSpeed), cancellationToken: _cts.Token); // Wait for attack interval
            var bullet = await SingleBehaviour.Of<PoolingManager>().Rent("bullet");
            bullet.transform.position = transform.position;
            await bullet.GetComponent<Bullet>().InitiateBullet(moveSpeed * 2, _killDistance, damage, critChance);
        }
    }

    public void TriggerKillDamage(){
        _isReachedTarget = true;
        _stageManager.DealDamage(killDamage, false); // Deal kill damage
        OnDestroyMiniHero(); // Destroy or return the MiniHero
    }

    public void StopAllActions(){
        if (_cts != null) _cts.Cancel();
    }

    public void OnDestroyMiniHero(){
        StopAllActions(); // Stop all actions (Move, Attack)
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }

    public void Dispose(){
        StopAllActions();
        OnDestroyMiniHero();
    }

    public async UniTask IncreaseStats(BuffType type, float value, float duration){
        if(type == BuffType.AttackSpeed){
            var baseAttackSpeed = attackSpeed;
            attackSpeed += attackSpeed * value;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _cts.Token);
            attackSpeed = baseAttackSpeed;
        }
        else if(type == BuffType.MovementSpeed){
            var baseMoveSpeed = moveSpeed;
            moveSpeed += moveSpeed * value;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _cts.Token);
            moveSpeed = baseMoveSpeed;
        }
    }
}
