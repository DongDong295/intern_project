using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro; // Import DOTween

public class PopUpWarnNoEquippedHero : MonoBehaviour
{

    public float moveDistance = 100f; // How far up it moves
    public float duration = 1.5f;     // Total duration
    
    private TextMeshProUGUI text; // Reference to the TextMeshProUGUI component

    private Vector3 _startPosition; // Store the starting position of the text
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        _startPosition = transform.position; // Get the starting position of the text
        text.gameObject.SetActive(false); // Hide the text at the start
    }

    public void ShowPopUp()
    {
        transform.position = _startPosition; // Get the TextMeshProUGUI component
        text.alpha = 1; // Reset the alpha to 1
            text.text = "Please equip a hero!"; // Set the text to display
            gameObject.SetActive(true); // Show the pop-up
            text.DOFade(0, duration/2).SetEase(Ease.InQuad); // Fade out the text
            transform.DOMoveY(transform.position.y + moveDistance, duration/2).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                gameObject.SetActive(false); // Hide the pop-up after the animation
            });
    }
}
