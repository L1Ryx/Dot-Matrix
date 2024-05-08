using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnergySlotPositions : ScriptableObject
{
    [SerializeField] PlayerState playerState;
    public Vector2 centerPos;
    [SerializeField] private float distanceOffset;


    public Vector2 GetSlotPosition(int num) {
        int mid = (int) Math.Ceiling( (double) playerState.maxEnergySlots / 2);
        float amountOffset = distanceOffset * (num - mid);
        Vector2 pos = new Vector2(centerPos.x + amountOffset, centerPos.y);
        return pos;
    }
}
