using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InitializerGP : MonoBehaviour
{
    [Header("Datacube Refs")]
    [SerializeField] private Square[] squares;
    [SerializeField] private GridPositions gridPositions;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private EnergySlotPositions energySlotPositions;

    [Header("GameObject Refs")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject energyBarPrefab;
    public GameObject currentPlayer;
    public GameObject currentEnergyBar;

    [Header("Settings")]
    [SerializeField] private float squareSpawnDelay = 0;

    [Header("Event Refs")]
    [SerializeField] private UnityEvent startedPF;
    
    
    
    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        DeactivatePlayer();
        InitializeSquares(); // calls InitializePlayer() and InitializeEnergyBar()
    }

    private void DeactivatePlayer()
    {
        playerState.isActive = false;
    }

    private void InitializeSquares()
    {
        StartCoroutine(DelayedSquareCreation());
    }

    private IEnumerator DelayedSquareCreation() {
        for (int i = 0; i < squares.Length; i++) {
            Vector2 squareSpawnPos = gridPositions.GetSquarePos(squares[i].squareRow, squares[i].squareCol);
            GameObject squareObj = Instantiate(squarePrefab, squareSpawnPos, Quaternion.identity);
            squareObj.GetComponent<SquareController>().square = squares[i];

            yield return new WaitForSeconds(squareSpawnDelay);
        }

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        playerState.onRow = gridPositions.centerRow;
        playerState.onCol = gridPositions.centerCol;
        playerState.currentEnergy = playerState.startingEnergy;

        Vector2 playerSpawnPos = gridPositions.GetSquarePos(playerState.onRow, playerState.onCol);
        currentPlayer = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        currentPlayer.GetComponent<PlayerController>().thisPlayer = currentPlayer;
        playerState.isActive = true;

        InitializeEnergyBar();
    }

    private void InitializeEnergyBar()
    {
        currentEnergyBar = Instantiate(energyBarPrefab, energySlotPositions.centerPos, Quaternion.identity);
        startedPF.Invoke();
    }
}
