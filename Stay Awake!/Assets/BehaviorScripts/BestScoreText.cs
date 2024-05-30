using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class BestScoreText : MonoBehaviour
{
    [Header("Datacube Refs")]
    [SerializeField] PlayerState playerState;

    [Header("Component Refs")]
    TMP_Text text;

    private void Awake() {
        text = gameObject.GetComponent<TMP_Text>();
    }

    public void UpdateText() {
        text.text = "Best: " + playerState.highScore.ToString();
    }
}
