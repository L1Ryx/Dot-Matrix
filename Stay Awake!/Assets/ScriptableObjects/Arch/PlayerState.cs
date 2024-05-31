using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerState : ScriptableObject
{
    [Header("States")]
    public int onRow;
    public int onCol;
    public int currentEnergy;
    public bool isActive;
    public int currentPoints;
    public int lastEarnedPoints;

    [Header("Settings")]
    public int maxEnergySlots;
    public int startingEnergy;
    [Header("Save Data")]
    public int highScore;
    public bool hasSeenTutorial;
    
    
    
}
