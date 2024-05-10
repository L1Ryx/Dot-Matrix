using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainGP : MonoBehaviour
{
    [Header("Datacubes")]
    public GameplayTimer gameplayTimer;
    public Square[] squares;
    public GridPositions gridPositions;
    public PlayerState playerState;

    [Header("Settings")]
    public float startTime = 0;
    
    void Awake() {
        Initialize();
    }

    private void Initialize()
    {
        gameplayTimer.setTimeElapsed(startTime);
    }

    void Update() {
        gameplayTimer.addTimeElapsed(Time.deltaTime);
    }
}
