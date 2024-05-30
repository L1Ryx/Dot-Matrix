using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreText : MonoBehaviour
{
    [Header("Datacube Refs")]
    [SerializeField] PlayerState playerState;

    [Header("Component Refs")]
    TMP_Text text;

    [Header("Events")]
    [SerializeField] private UnityEvent gameplayOver;

    [Header("Settings")]
    [SerializeField] private float totalBlinkTime;
    [SerializeField] private float blinkInterval;

    private void Awake() {
        text = gameObject.GetComponent<TMP_Text>();
    }

    public void BlinkAndEndGameplay() {
        StartCoroutine(BlinkCoroutine());
    }


    private IEnumerator BlinkCoroutine() {
        Color originalColor = text.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float elapsedTime = 0f;
        bool isOriginalColor = true;

        while (elapsedTime < totalBlinkTime) {
            text.color = isOriginalColor ? originalColor : transparentColor;
            isOriginalColor = !isOriginalColor;
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Ensure the text is set back to the original color after blinking
        text.color = originalColor;

        // Invoke the gameplayOver event and log a message
        gameplayOver.Invoke();
        // Debug.Log("Gameplay over event invoked.");
    }

    private void OnEnable() {
        UpdateText();
    }

    public void UpdateText() {
        text.text = playerState.currentPoints.ToString();
    }
}
