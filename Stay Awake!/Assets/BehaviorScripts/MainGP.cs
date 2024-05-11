using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

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

    [Header("Settings")]
    public float startTime = 0;
    public float ballSpeedMultiplier;
    public float ballSpeedIncreaseFactor;
    public float baseTimeBeforeNextSpawnRound;
    public int energyBallCount;
    public int enemyBallCount;
    public int roundCounter = 0;
    public int roundsBeforeIncrease;
    
    void Awake() {
        Initialize();
    }

    private IEnumerator SpawnRoutine()
    {
        // Wait for the initial grace period before starting the first round
        yield return new WaitForSeconds(baseTimeBeforeNextSpawnRound / 2);

        while (true)  // Loop to continue spawning rounds
        {
            SpawnWave();

            // Increase the speed multiplier logarithmically every few rounds
            if (++roundCounter % roundsBeforeIncrease == 0)
            {
                ballSpeedMultiplier += Mathf.Log(ballSpeedMultiplier + ballSpeedIncreaseFactor);
                roundCounter = 0;  // Reset counter after increase
            }

            // Calculate the time before the next spawn round
            float timeBeforeNextRound = baseTimeBeforeNextSpawnRound / ballSpeedMultiplier;
            yield return new WaitForSeconds(timeBeforeNextRound);
        }
    }

    private void SpawnWave()
    {
        if (squareObjs.Count < Math.Max(energyBallCount, enemyBallCount))
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
        gameplayTimer.setTimeElapsed(startTime);
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
        gameplayTimer.addTimeElapsed(Time.deltaTime);
        
    }
}
