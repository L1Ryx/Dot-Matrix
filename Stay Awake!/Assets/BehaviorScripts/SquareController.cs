using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    [Header("DataCube Refs")]
    public Square square;
    public PlayerState playerState;
    public void UpdateHasPlayer() {
        if (playerState.onCol == square.squareCol && playerState.onRow == square.squareRow) {
            square.hasPlayer = true;
        } else {
            square.hasPlayer = false;
        }
    }
}
