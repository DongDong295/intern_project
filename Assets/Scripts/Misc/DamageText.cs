using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using ZBase.Foundation.Singletons;  // Make sure to include this for DOTween

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float moveDistance;  // Distance to move the text upwards
    [SerializeField] private float duration;  // Duration of the animation

    public void InitializeText(float damage, bool isCrit)
    {
        text.alpha = 1;
        if (isCrit)
            text.text = $"CRIT! {damage}";
        else
            text.text = damage.ToString();

        // Call DOTween to animate the text
        AnimateDamageText();
    }

    private void AnimateDamageText()
    {
        // Create a sequence for animations
        Sequence damageTextSequence = DOTween.Sequence();

        // First, scale the text
        damageTextSequence.Append(text.transform.DOScale(1.5f, duration).SetEase(Ease.OutBack));  // Scale up to 1.5x its size

        // Then, move the text upwards
        damageTextSequence.Join(text.transform.DOMoveY(transform.position.y + moveDistance, duration).SetEase(Ease.OutQuad));

        // After scaling and moving, fade the text out
        damageTextSequence.Append(text.DOFade(0, duration / 2).SetEase(Ease.InQuad));

        // Once the sequence is complete, return the object to the pool
        damageTextSequence.OnKill(() => SingleBehaviour.Of<PoolingManager>().Return(gameObject));
    }
}
