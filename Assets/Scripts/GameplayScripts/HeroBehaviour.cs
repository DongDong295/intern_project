using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Pooling;
using ZBase.Foundation.Singletons;

public class HeroBehaviour : MonoBehaviour, IDispose
{   
    private Hero _hero;
    private bool _canSpawn;
    
    private MiniHeroData _miniData;
    private CancellationTokenSource _source;
    public void InitiateHero(Hero hero){
        SingleBehaviour.Of<StageManager>().DisposeList.Add(this);
        _hero = hero;
        _canSpawn = true;
        _miniData = new MiniHeroData(
                _hero.moveSpeed, _hero.attackDamageStep * _hero.level, _hero.attackSpeed, _hero.critChance, _hero.killDamage);
        _source = new CancellationTokenSource();
        StartSpawningMiniHero();
    }

    public void StartSpawningMiniHero(){
        SpawnMiniHero().Forget();
    }

    private async UniTask SpawnMiniHero(){
        while(_canSpawn){
            await UniTask.Delay(TimeSpan.FromSeconds(_hero.cooldownGenerate), cancellationToken: _source.Token);
            var miniHero = await SingleBehaviour.Of<PoolingManager>().Rent($"mini-hero-visual-{_hero.heroVisualID}");
            miniHero.transform.position = transform.position;
            await miniHero.GetComponent<MiniHeroBehaviour>().InitiateMiniHero(_miniData);
        }
    }

    public void StopSpawningMiniHero(){
        _canSpawn = false;
        _miniData = null;
        _source?.Cancel();
    }

    public void Dispose(){
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
        StopSpawningMiniHero();
    }
}
