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

    [Header("Events")]
    [SerializeField] private UnityEvent updateEnergy;
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

    public void UpdatePlayerEnergy() {
        if (square.hasPlayer && square.ballValue != 0) {
            if (square.ballValue == 1) {
                AddEnergy(square.ballValue);
            } else if (square.ballValue == -1) {
                MinusEnergy(square.ballValue * -1);
            }
            square.ballValue = 0;
            if (currentBallObj != null) {
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
    }

    private void MinusEnergy(int num) {
        playerState.currentEnergy -= num;
        if (playerState.currentEnergy < 0) {
            playerState.currentEnergy = 0;
        }
    }
    public void TestSpawnBall() {
        currentBallObj = Instantiate(ballPrefab, this.transform);
        currentBallObj.GetComponent<Ball>().HandleBall(BallType.Enemy, this.gameObject);
    }

    
}
