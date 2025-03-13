using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZBase.Foundation.Pooling;
using ZBase.Foundation.Singletons;

public class HeroBehaviour : MonoBehaviour
{   
    private Hero _hero;
    private bool _canSpawn;

    private CancellationTokenSource _source;
    public void InitiateHero(Hero hero){
        _hero = hero;
        _canSpawn = true;
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
            //await miniHero.GetComponent<MiniHeroBehaviour>().InitiateMiniHero();
        }
    }

    public void StopSpawningMiniHero(){
        _canSpawn = false;
        _source?.Cancel();
    }
}
