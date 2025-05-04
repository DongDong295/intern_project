using UnityEngine;
using DG.Tweening;

public class TeleportVisual : MonoBehaviour
{
    [SerializeField] private GameObject _visual;
    void Start()
    {
        // Rotate around Y axis 360 degrees over 2 seconds, infinitely
        _visual.transform.DORotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
}
