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
    [SerializeField] private GameplayTimer gameplayTimer;

    [Header("GameObject Refs")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject energyBarPrefab;
    [SerializeField] private GameObject mainGpPrefab;
    public GameObject currentPlayer;
    public GameObject currentEnergyBar;
    public GameObject currentMainGp;
    public List<GameObject> squareObjs = new List<GameObject>();

    [Header("Settings")]
    [SerializeField] private float squareSpawnDelay = 0;
    [SerializeField] private bool setGameplayTimerToZeroAtAwake = true;

    [Header("Event Refs")]
    [SerializeField] private UnityEvent startedPF;
    [SerializeField] private UnityEvent squareCreated;
    
    
    void Awake() {
        if (setGameplayTimerToZeroAtAwake) {
            gameplayTimer.setTimeElapsed(0);
        }
    }

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        DeactivatePlayer();
        InitializeSquares(); // calls InitializePlayer(), InitializeEnergyBar(), and etc.
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
        squareCreated.Invoke();
        Vector2 squareSpawnPos = gridPositions.GetSquarePos(squares[i].squareRow, squares[i].squareCol);
        GameObject newSquare = Instantiate(squarePrefab, squareSpawnPos, Quaternion.identity); // Create the new square GameObject
        newSquare.GetComponent<SquareController>().square = squares[i]; // Assign the ScriptableObject to the controller

        squareObjs.Add(newSquare); // Add the new GameObject to the list

        yield return new WaitForSeconds(squareSpawnDelay);
    }

    InitializePlayer();
}


    private void InitializePlayer()
    {
        playerState.onRow = gridPositions.centerRow;
        playerState.onCol = gridPositions.centerCol;
        playerState.currentEnergy = playerState.startingEnergy;
        playerState.currentPoints = 0;

        Vector2 playerSpawnPos = gridPositions.GetSquarePos(playerState.onRow, playerState.onCol);
        currentPlayer = Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        currentPlayer.GetComponent<PlayerController>().thisPlayer = currentPlayer;
        playerState.isActive = true;

        InitializeEnergyBar();
    }

    private void InitializeEnergyBar()
    {
        currentEnergyBar = Instantiate(energyBarPrefab, energySlotPositions.centerPos, Quaternion.identity);
        InitializeMainGp();
    }

    private void InitializeMainGp()
    {
        currentMainGp = Instantiate(mainGpPrefab, Vector2.zero, Quaternion.identity);
        currentMainGp.GetComponent<MainGP>().squares = squares;
        currentMainGp.GetComponent<MainGP>().gridPositions = gridPositions;
        currentMainGp.GetComponent<MainGP>().playerState = playerState;
        currentMainGp.GetComponent<MainGP>().gameplayTimer = gameplayTimer;
        currentMainGp.GetComponent<MainGP>().squareObjs = this.squareObjs;
        startedPF.Invoke();
        Destroy(this.gameObject); // this is dangerous
    }
}
