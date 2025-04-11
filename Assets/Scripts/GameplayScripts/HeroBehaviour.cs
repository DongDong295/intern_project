using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Pooling;
using ZBase.Foundation.Singletons;
using DG.Tweening;  // DOTween namespace for animations

public class HeroBehaviour : MonoBehaviour, IDispose
{   
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private Image _filler;
    private Hero _hero;
    private bool _canSpawn;
    
    private MiniHeroData _miniData;
    private CancellationTokenSource _source;

    public async UniTask InitiateHero(Hero hero)
    {
        SingleBehaviour.Of<StageManager>().DisposeList.Add(this);
        _hero = hero;
        _canSpawn = true;
        _miniData = new MiniHeroData(
                _hero.moveSpeed, _hero.attackDamageStep * _hero.level, _hero.attackSpeed, _hero.critChance, _hero.killDamage);
        _source = new CancellationTokenSource();
        _progressBar.SetActive(true);  // Show the progress bar
        StartSpawningMiniHero();  // Start spawning mini heroes
        await UniTask.CompletedTask;
    }

    public void StartSpawningMiniHero(){
        SpawnMiniHero().Forget();
    }

    private async UniTask SpawnMiniHero()
    {
        while (_canSpawn)
        {
            float cooldownTime = _hero.cooldownGenerate;
            float elapsedTime = 0f;
            while (elapsedTime < cooldownTime)
            {
                elapsedTime += Time.deltaTime;
                _filler.fillAmount = Mathf.Clamp01(elapsedTime / cooldownTime); // Update fillAmount
                await UniTask.Yield(cancellationToken: _source.Token); // Yield to allow other tasks to run
            }
            _filler.fillAmount = 1f;

            var miniHero = await SingleBehaviour.Of<PoolingManager>().Rent($"mini-hero-visual-{_hero.heroVisualID}");
            miniHero.transform.position = transform.position;
            miniHero.GetComponent<MiniHeroBehaviour>().InitiateMiniHero(_miniData);
        }
    }

    public void StopSpawningMiniHero(){
        _progressBar.SetActive(false);  // Hide the progress bar
        _canSpawn = false;
        _miniData = null;
        _source?.Cancel();
    }

    public void Dispose(){
        //SingleBehaviour.Of<PoolingManager>().Return(gameObject);
        StopSpawningMiniHero();
    }

    public void UnloadHero(){
        StopSpawningMiniHero();
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }
}
