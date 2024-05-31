using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MainGP : MonoBehaviour
{
    [Header("Datacubes")]
    public GameplayTimer gameplayTimer;
    public Square[] squares;
    public GridPositions gridPositions;
    public PlayerState playerState;
    private SquareController[] squareControllers;

    [Header("Objects")]
    public List<GameObject> squareObjs;
    
    // [Header("Events")]
    // public UnityEvent gameplayOver;

    [Header("Settings")]
    public float startTime = 0;
    public float ballSpeedMultiplier;
    public float ballSpeedIncreaseFactor;
    public float baseTimeBeforeNextSpawnRound;
    public int minTotalBallCount;  // Minimum total number of balls to spawn each wave
    public int maxTotalBallCount;  // Maximum total number of balls to spawn each wave
    public int minEnergyBallCount;  // Minimum number of energy balls to spawn
    public int maxEnergyBallCount;  // Maximum number of energy balls to spawn
    public int roundsBeforeIncrease = 5;  // Number of rounds before increasing speed multiplier
    public float timeCapForSpeedIncrease;  // Time cap in seconds after which speed will no longer increase
    private int roundCounter = 0;  // Tracks the number of rounds since last speed increase
    private float totalTimeElapsed = 0;  // Tracks the total time elapsed since the start of the game
    public string gameplayLoopTrackName = "GameplayLoop";
        
    void Awake() {
        Initialize();
    }

    public void Die() {

        // 2. Destroy ALL balls on screen and stop ball generating
        StopAllCoroutines(); 

        // STOP MUSIC
        MusicManager.Instance.StopMusic(gameplayLoopTrackName);
    }

    public void DeleteAllSquareObjs() {
        
        foreach (GameObject squareObj in squareObjs) {
            Destroy(squareObj);
        }
        squareObjs.Clear();
        Destroy(this.gameObject); // very dangerous
    }


    private IEnumerator SpawnRoutine()
    {
        // Wait for the initial grace period before starting the first round
        yield return new WaitForSeconds(baseTimeBeforeNextSpawnRound / 2);

        while (true)  // Loop to continue spawning rounds
        {
            SpawnWave();

            // Calculate the time before the next spawn round
            float timeBeforeNextRound = baseTimeBeforeNextSpawnRound / ballSpeedMultiplier;
            yield return new WaitForSeconds(timeBeforeNextRound);

            totalTimeElapsed += timeBeforeNextRound;  // Update the total elapsed time

            // Increase the speed multiplier logarithmically every few rounds if below the time cap
            if (totalTimeElapsed < timeCapForSpeedIncrease && ++roundCounter % roundsBeforeIncrease == 0)
            {
                ballSpeedMultiplier += Mathf.Log(ballSpeedMultiplier + ballSpeedIncreaseFactor);
                roundCounter = 0;  // Reset counter after increase
            }
        }
    }



    private void SpawnWave()
    {
        if (!playerState.isActive) {
            return;
        }
        int totalBallCount = UnityEngine.Random.Range(minTotalBallCount, maxTotalBallCount + 1);
        int energyBallCount = UnityEngine.Random.Range(minEnergyBallCount, maxEnergyBallCount + 1);
        int enemyBallCount = totalBallCount - energyBallCount;

        if (energyBallCount > totalBallCount) {
            energyBallCount = totalBallCount; // Ensure energy balls don't exceed the total count
            enemyBallCount = 0; // All balls this wave are energy balls
        }

        if (squareObjs.Count < totalBallCount)
        {
            Debug.LogError("Not enough GameObject squares available for the requested number of spawns.");
            return;
        }

        HashSet<int> usedIndices = new HashSet<int>();  // Shared set to track used indices

        // Spawn Energy Balls
        List<int> energyIndices = SelectRandomSquareIndices(energyBallCount, usedIndices);
        foreach (int index in energyIndices)
        {
            if (index >= 0 && index < squareObjs.Count)
            {
                SquareController controller = squareObjs[index].GetComponent<SquareController>();
                if (controller != null) {
                    controller.SpawnBall(BallType.Energy, ballSpeedMultiplier);
                    usedIndices.Add(index);  // Mark this index as used
                }
                else
                    Debug.LogError("SquareController not found for energy ball at index: " + index);
            }
            else
            {
                Debug.LogError("Invalid index for energy ball at index: " + index);
            }
        }

        // Spawn Enemy Balls, ensuring they don't overlap with energy balls
        List<int> enemyIndices = SelectRandomSquareIndices(enemyBallCount, usedIndices);
        foreach (int index in enemyIndices)
        {
            if (index >= 0 && index < squareObjs.Count)
            {
                SquareController controller = squareObjs[index].GetComponent<SquareController>();
                if (controller != null)
                    controller.SpawnBall(BallType.Enemy, ballSpeedMultiplier);
                else
                    Debug.LogError("SquareController not found for enemy ball at index: " + index);
            }
            else
            {
                Debug.LogError("Invalid index for enemy ball at index: " + index);
            }
        }
    }


    private List<int> SelectRandomSquareIndices(int count, HashSet<int> usedIndices)
    {
        List<int> selectedIndices = new List<int>();
        if (squares.Length < count) // Additional safety check
        {
            Debug.LogError("Not enough squares to select the requested number of indices.");
            return selectedIndices;
        }

        while (selectedIndices.Count < count)
        {
            int randomIndex = UnityEngine.Random.Range(0, squares.Length);
            if (!usedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                usedIndices.Add(randomIndex); // Add to used indices to prevent reuse in the same wave
            }
        }

        return selectedIndices;
    }


    private void Initialize()
    {
        playerState.isActive = true;
        gameplayTimer.setTimeElapsed(startTime);
        MusicManager.Instance.PlayMusic(gameplayLoopTrackName);
        CacheSquareControllers();
        StartCoroutine(SpawnRoutine());
    }

    private void CacheSquareControllers()
    {
        squareControllers = new SquareController[squareObjs.Count];
        for (int i = 0; i < squareObjs.Count; i++)
        {
            if (squareObjs[i] != null)
                squareControllers[i] = squareObjs[i].GetComponent<SquareController>();
            else
                Debug.LogError("GameObject in squareObjs at index " + i + " is null.");
            
            if (squareControllers[i] == null)
                Debug.LogError("SquareController not found on GameObject at index " + i);
        }
    }


    void Update() {
        if (playerState.isActive) {
            gameplayTimer.addTimeElapsed(Time.deltaTime);
        }
    }
}
