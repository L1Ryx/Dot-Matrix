using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridPositions : ScriptableObject
{
    [SerializeField] private Vector2 centerPos;
    public int centerRow;
    public int centerCol;
    [SerializeField] private float posOffset;
    public int maxRow;
    public int maxCol;


    public Vector2 GetSquarePos(int row, int col) {
        float posX = centerPos.x + ( (centerCol - col) * (posOffset * -1) );
        float posY = centerPos.y + ( (centerRow - row) * (posOffset));

        return new Vector2(posX, posY);
    }
}
