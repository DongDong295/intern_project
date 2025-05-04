using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // Include DOTween namespace

public class Boss : MonoBehaviour
{
    [Header("Breathing Effect Settings")]
    public float breathSpeed;    // Speed of the breathing effect (how fast the breathing happens)
    public float breathAmount; // Amount to scale up and down (how noticeable the effect is)

    private Vector3 _originalScale;

    void Start()
    {
        // Store the initial scale of the boss object
        _originalScale = transform.localScale;

        // Start the breathing effect using DOTween
        //StartBreathingEffect();
    }

    private void StartBreathingEffect()
    {
        // Use DOTween to smoothly scale the boss up and down in all directions (X, Y, Z)
        transform.DOScale(
            new Vector3(
                _originalScale.x + breathAmount, 
                _originalScale.y + breathAmount,
                _originalScale.z + breathAmount
            ), 
            breathSpeed)                      // Set the duration of one full breathing cycle (slow, smooth)
            .SetLoops(-1, LoopType.Yoyo)      // Make it loop back and forth infinitely (in and out)
            .SetEase(Ease.InOutSine);         // Smooth easing function for gentle in-out transition
    }
}
