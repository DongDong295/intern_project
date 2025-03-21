using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Singletons;
using DG.Tweening; // Add DOTween namespace

public class BuffSphere : MonoBehaviour, IDispose
{
    private BuffDataItems data;
    private StageManager _stageManager;

    [SerializeField] private TextMeshProUGUI _displayBuff;
    public float moveDistance = 0.5f; // Distance to move up and down
    public float moveDuration = 1f; // Duration for one complete cycle
    private Tween _tween; // Tween object to store the animation
    private Tween _shrinkTween; // Tween object for shrinking effect

    private Vector3 _baseScale;
    public async UniTask InitiateBuff(BuffDataItems data)
    {
        Debug.Log("Initiate the buff");
        _baseScale = transform.localScale;
        _stageManager = SingleBehaviour.Of<StageManager>();
        this.data = data;
        DisplayBuffName();
        _stageManager.DisposeList.Add(this);
        StartBuffAnimation();

        await UniTask.CompletedTask;
    }

    void OnMouseDown()
    {
        ShrinkBuff().Forget();
    }

    public async UniTask ActiveBuff()
    {
        _stageManager.CurrentBuff = null;
        foreach (var i in _stageManager.DisposeList)
        {
            if (i is MiniHeroBehaviour)
            {
                var a = i as MiniHeroBehaviour;
                a.IncreaseStats(data.buffName, data.buffValue, data.buffTime).Forget();
            }
        }
        await UniTask.CompletedTask;
    }

    private void DisplayBuffName()
    {
        _displayBuff.transform.position = transform.position + new Vector3(0, 2, 0);
        switch (data.buffName)
        {
            case BuffType.AttackSpeed:
                _displayBuff.text = "Increase Attack Speed!";
                break;
            case BuffType.MovementSpeed:
                _displayBuff.text = "Increase Movement Speed!";
                break;
        }
    }

    private void StartBuffAnimation()
    {
        _tween = transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo) // This makes it loop back and forth (up and down)
            .SetEase(Ease.InOutSine); // Smooth easing for up and down movement
    }

    // Shrink the buff when clicked
    private async UniTask ShrinkBuff()
    {
        // Shrink the buff by scaling it down
        _shrinkTween = transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InQuad).OnComplete(() => {transform.localScale = _baseScale;}); // Shrink to zero scale

        // Wait for the shrink animation to complete
        await _shrinkTween.AsyncWaitForKill();

        // After shrink animation completes, return the buff to the pool
        ActiveBuff().Forget(); // Apply buff effects
        SingleBehaviour.Of<PoolingManager>().Return(gameObject); // Return to the pool
    }

    // Dispose method to stop the animation and return the object
    public void Dispose()
    {
        // Kill the tween if it exists when the BuffSphere is disposed
        _tween.Kill();
        _shrinkTween?.Kill(); // Kill the shrink tween if it exists
        SingleBehaviour.Of<PoolingManager>().Return(gameObject);
    }
}
