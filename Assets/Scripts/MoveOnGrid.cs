using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnGrid : MonoBehaviour
{
    public static MoveOnGrid Instance;

    private int x, y; // current tile
    private bool firstMove;

    void Awake()
    {
        Instance = this;
        firstMove = true;
    }

    public bool TryToMove(Vector2 newPos)
    {
        Vector2 convertedNewPos = Convert.IsoToGrid(newPos);

        //Debug.Log("pos iso: " + newPos + ", pos grid: " + convertedNewPos);

        int newX = Mathf.FloorToInt(convertedNewPos[0]);
        int newY = Mathf.FloorToInt(convertedNewPos[1]);

        if(firstMove)
        {
            firstMove = false;
            Move(newX, newY);
            return true;
        }

        //Can only move toward an adjacent tile
        if( (Mathf.Abs(x - newX) == 1 && y == newY) || (Mathf.Abs(y - newY) == 1 && x == newX))
        {
            Move(newX, newY);
            return true;
        }

        //Debug.Log("Invalid Movement: " + x + ", "+ y + " to " + newX + ", " + newY);
        return false;
    }

    //update x and y to the new tile
    private void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        //Debug.Log("Moved to " + x + ", " + y);
    }

    public Vector2 GetPos()
    {
        return new Vector2(x, y);
    }
}
