using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Singletons;
using DG.Tweening;
using Cysharp.Threading.Tasks.Triggers;
using TMPro; // Include DOTween namespace for animations

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

    [SerializeField] private GameObject _heroVisual; // Visual capsule (the hero)

    private Tween _bobbingTween; // Tween for the bobbing effect
    private Quaternion _baseRotation;

    public void InitiateMiniHero(MiniHeroData data)
    {
        _baseRotation = transform.rotation;
        _stageManager = SingleBehaviour.Of<StageManager>();
        if(!_stageManager.DisposeList.Contains(this))
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
        
        // Start scale-up effect when the mini hero is initialized
        StartScaleUpEffect();
        SpawnVFX().Forget();
        MoveToBoss().Forget();
        AttackBoss().Forget();
        
        // Start the bobbing animation for the hero
        StartBobbingEffect();
    }

    private void StartScaleUpEffect()
    {
        // Start with scale 0 (invisible)
        transform.localScale = Vector3.zero; 

        // Animate scale from 0 to the original scale
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f) // Scale to full size
            .SetEase(Ease.OutBack);  // Add easing to make it feel more natural
    }

    // Start the bobbing animation for the Tiny Hero's visual
    private void StartBobbingEffect()
    {
        // Create a sequence to chain the rotations
        _bobbingTween = DOTween.Sequence()
            // Rotate from 0 to 10 degrees
            .Append(_heroVisual.transform.DOLocalRotate(new Vector3(0, 0, 10), 0.5f).SetEase(Ease.InOutSine))
            // Rotate from 10 to 0 degrees
            .Append(_heroVisual.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutSine))
            // Rotate from 0 to -10 degrees
            .Append(_heroVisual.transform.DOLocalRotate(new Vector3(0, 0, -10), 0.5f).SetEase(Ease.InOutSine))
            // Rotate from -10 to 0 degrees
            .Append(_heroVisual.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutSine))
            // Loop the sequence infinitely
            .SetLoops(-1, LoopType.Restart);
    }

    public async UniTask MoveToBoss()
    {
        while (Vector3.Distance(transform.position, _bossPosition) > _killDistance)
        {
            _isReachedTarget = false;
        
            transform.position = Vector3.MoveTowards(transform.position, _bossPosition, moveSpeed * _stageManager.StageDeltaTime);
            await UniTask.Yield(cancellationToken: _cts.Token);
        }
        TriggerKillDamage();
    }

    public async UniTask AttackBoss()
    {
        while (!_isReachedTarget)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(attackSpeed), cancellationToken: _cts.Token); // Wait for attack interval
            var bullet = await SingleBehaviour.Of<PoolingManager>().Rent("bullet");
            bullet.transform.position = transform.position;
            await bullet.GetComponent<Bullet>().InitiateBullet(moveSpeed * 2, _killDistance, damage, critChance);
        }
    }

    public void TriggerKillDamage()
    {
        _isReachedTarget = true;
        _stageManager.DealDamage(killDamage, false);// Deal kill damage
        TriggerKillDamageVFX().Forget();
        OnDestroyMiniHero(); // Destroy or return the MiniHero
    }

    public async UniTask TriggerKillDamageVFX()
    {
        // Rent the VFX from the pooling manager
        var vfx = await SingleBehaviour.Of<PoolingManager>().Rent("kill-damage-vfx");
        vfx.transform.position = transform.position + new Vector3(0.5f, 1.5f, -0.75f);

        // Get the ParticleSystem component from the VFX
        var particleSystem = vfx.GetComponent<ParticleSystem>();

        // Play the particle system
        particleSystem.Play();

        // Wait until the particle system finishes playing
        await UniTask.WaitUntil(() => !particleSystem.isPlaying);

        // Return the VFX to the pool after it finishes
        SingleBehaviour.Of<PoolingManager>().Return(vfx);
    }

    public void StopAllActions()
    {
        if (_cts != null) _cts.Cancel();
    }

    public void TinyHeroAnim()
    {
        // You can add additional animations here if needed
    }

    public void OnDestroyMiniHero()
    {
        StopAllActions(); // Stop all actions (Move, Attack)
        _heroVisual.transform.rotation = _baseRotation;
        _heroVisual.transform.DOComplete();
        _heroVisual.transform.DOKill();  // Kill any tweens on the hero visual
        _bobbingTween.Kill();            // Kill the specific bobbing tween if it exists
        // Return the MiniHero to the pooling manager
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }
    
    public void Dispose()
    {
        StopAllActions();
        OnDestroyMiniHero();
    }

    public async UniTask IncreaseStats(BuffType type, float value, float duration)
    {
        if(type == BuffType.AttackSpeed)
        {
            var baseAttackSpeed = attackSpeed;
            attackSpeed += attackSpeed * value;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _cts.Token);
            attackSpeed = baseAttackSpeed;
        }
        else if(type == BuffType.MovementSpeed)
        {
            var baseMoveSpeed = moveSpeed;
            moveSpeed += moveSpeed * value;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _cts.Token);
            moveSpeed = baseMoveSpeed;
        }
    }

    public async UniTask SpawnVFX()
    {
        var vfx = await SingleBehaviour.Of<PoolingManager>().Rent("minihero-spawn-vfx");
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
}
