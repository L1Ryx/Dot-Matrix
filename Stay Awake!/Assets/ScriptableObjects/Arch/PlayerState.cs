using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerState : ScriptableObject
{
    public int onRow;
    public int onCol;
    public int maxEnergySlots;
    public int startingEnergy;
    public int currentEnergy;
    public bool isActive;
    
}
