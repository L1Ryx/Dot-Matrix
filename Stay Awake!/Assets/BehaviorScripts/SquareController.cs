using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    [Header("DataCube Refs")]
    public Square square;
    public PlayerState playerState;

    [Header("Objects")]
    [SerializeField] private GameObject anyBall;
    [SerializeField] private GameObject energyBall;
    [SerializeField] private GameObject enemyBall;
    public void UpdateHasPlayer() {
        if (playerState.onCol == square.squareCol && playerState.onRow == square.squareRow) {
            square.hasPlayer = true;
        } else {
            square.hasPlayer = false;
        }
    }
}
