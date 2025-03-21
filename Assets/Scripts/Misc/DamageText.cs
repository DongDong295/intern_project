using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using ZBase.Foundation.Singletons;  // Make sure to include this for DOTween

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveDistance;  // Distance to move the text upwards
    [SerializeField] private float duration;  // Duration of the animation

    private Sequence _sequence;
    private Vector3 _startPosition;
    public void InitializeText(float damage, bool isCrit)
    {
        _sequence = DOTween.Sequence();
        _startPosition = text.transform.position;
        text.alpha = 1;
        text.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        if (isCrit)
            text.text = $"CRIT! {damage}";
        else
            text.text = damage.ToString();

        AnimateDamageText();
    }

    private void AnimateDamageText()
    {
        // Create a sequence for animations

        // First, scale the text
        _sequence.Append(text.transform.DOScale(1.5f, duration).SetEase(Ease.OutBack));  // Scale up to 1.5x its size

        // Then, move the text upwards
        _sequence.Join(text.transform.DOMoveY(transform.position.y + moveDistance, duration).SetEase(Ease.OutQuad));

        // After scaling and moving, fade the text out
        _sequence.Append(text.DOFade(0, duration / 2).SetEase(Ease.InQuad));

        // Once the sequence is complete, return the object to the pool
        _sequence.OnKill(() => {
            _sequence.Complete();
            _sequence.Kill();
            text.transform.position = _startPosition;
            SingleBehaviour.Of<PoolingManager>().Return(gameObject); }
        );
    }
}
