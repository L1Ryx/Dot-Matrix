using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnergyBar : MonoBehaviour
{
    [Header("Datacube Refs")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private EnergySlotPositions energySlotPositions;

    [Header("Private Fields")]
    [SerializeField] private Dictionary<int, GameObject> slots = new Dictionary<int, GameObject>();

    [Header("Object Refs")]
    [SerializeField] private GameObject energySlotPrefab;

    [Header("Sprite Refs")]
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite filledSprite;
    

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        for (int i = 0; i < playerState.maxEnergySlots; i++) {
            Vector2 spawnPos = energySlotPositions.GetSlotPosition(i + 1);
            slots[i] = Instantiate(energySlotPrefab, spawnPos, Quaternion.identity);
            if (i + 1 <= playerState.startingEnergy) {
                slots[i].GetComponent<SpriteRenderer>().sprite = filledSprite;
            } else {
                slots[i].GetComponent<SpriteRenderer>().sprite = emptySprite;
            }
        }
    }
}
