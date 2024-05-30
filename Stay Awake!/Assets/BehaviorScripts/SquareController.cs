using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class SquareController : MonoBehaviour
{
    [Header("DataCube Refs")]
    public Square square;
    public PlayerState playerState;
    public GameplayTimer gameplayTimer;

    [Header("Objects")]
    public GameObject ballPrefab;
    public GameObject currentBallObj;
    private static List<GameObject> allBalls = new List<GameObject>();

    [Header("Events")]
    [SerializeField] private UnityEvent updateEnergy;
    [SerializeField] private UnityEvent shouldAddPoints;
    [SerializeField] private UnityEvent addEnergy;
    public void InitializeSquare() {
        square.ballValue = 0;
        square.hasPlayer = false;
    }

    public void UpdateHasPlayer() {
        if (playerState.onCol == square.squareCol && playerState.onRow == square.squareRow) {
            square.hasPlayer = true;
        } else {
            square.hasPlayer = false;
        }
        UpdatePlayerEnergy();
    }

    public void DeleteAllBalls() {
        // Destroy all balls in the list and clear it
        foreach (GameObject ball in allBalls) {
            if (ball != null) {
                Destroy(ball);
            }
        }
        allBalls.Clear();

        // Set the ballValue of every square to 0
        foreach (SquareController squareController in FindObjectsOfType<SquareController>()) {
            squareController.square.ballValue = 0;
            squareController.currentBallObj = null;
        }
    }

    public void UpdatePlayerEnergy() {
        if (square.hasPlayer && square.ballValue != 0) {
            if (square.ballValue == 1) {
                AddEnergy(square.ballValue);
            } else if (square.ballValue == -1) {
                MinusEnergy(square.ballValue * -1);
            }
            square.ballValue = 0;
            if (currentBallObj != null) {
                allBalls.Remove(currentBallObj);  // Remove from list
                Destroy(currentBallObj);
            }
            updateEnergy.Invoke();
        }
    }

    private void AddEnergy(int num) {
        playerState.currentEnergy += num;
        if (playerState.currentEnergy > playerState.maxEnergySlots) {
            playerState.currentEnergy = playerState.maxEnergySlots;
        }
        shouldAddPoints.Invoke();
        addEnergy.Invoke();
    }

    private void MinusEnergy(int num) {
        playerState.currentEnergy -= num;
        if (playerState.currentEnergy < 0) {
            playerState.currentEnergy = 0;
        }
    }

    public void TestSpawnBall() { // TESTING ONLY - NOT USUALLY CALLED
        // empty
    }

    public void SpawnBall(BallType type, float speedMultiplier) {
        currentBallObj = Instantiate(ballPrefab, this.transform);
        currentBallObj.GetComponent<Ball>().HandleBall(type, this.gameObject, speedMultiplier);
        allBalls.Add(currentBallObj);  // Add to list of all balls
    }
    
}
