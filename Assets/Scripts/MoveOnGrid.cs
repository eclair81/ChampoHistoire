using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnGrid : MonoBehaviour
{
    public static MoveOnGrid Instance;

    private int x, y; // current tile

    void Start()
    {
        Instance = this;
        x = -1;
        y = -1;
    }

    public bool TryToMove(Vector2 newPos)
    {
        int newX = Mathf.FloorToInt(newPos[0]);
        int newY = Mathf.FloorToInt(newPos[1]);

        if(x == -1 || y == -1)
        {
            Move(newX, newY);
            return true;
        }

        //Can only move toward an adjacent tile
        if( (Mathf.Abs(x - newX) == 1 && y == newY) || (Mathf.Abs(y - newY) == 1 && x == newX))
        {
            Move(newX, newY);
            return true;
        }

        Debug.Log("Invalid Movement: " + x + ", "+ y + " to " + newX + ", " + newY);
        return false;
    }

    //update x and y to the new tile
    private void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        Debug.Log("Moved to " + x + ", " + y);
    }
}
