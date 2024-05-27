using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [Header("Datacube Refs")]
    [SerializeField] PlayerState playerState;

    [Header("Component Refs")]
    TMP_Text text;

    private void Awake() {
        text = gameObject.GetComponent<TMP_Text>();
        
    }

    private void OnEnable() {
        UpdateText();
    }
    public void UpdateText() {
        text.text = playerState.currentPoints.ToString();
    }

}
