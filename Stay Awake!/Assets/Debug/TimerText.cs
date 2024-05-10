using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [Header("Datacubes")]
    [SerializeField] private GameplayTimer gameplayTimer;

    [Header("Objects")]
    [SerializeField] private TMP_Text textObj;
    
    void Update()
    {
        textObj.text = gameplayTimer.getTimeElapsed().ToString("F2");
    }
}
