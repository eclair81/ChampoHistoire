using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnGrid : MonoBehaviour
{
    public static MoveOnGrid Instance;

    //private int x, y; // current tile
    [SerializeField] private List<Pos2D> allPos;
    private int levelIndex;

    void Awake()
    {
        Instance = this;
    }

    //Try to move to the given Iso pos
    public bool TryToMove(Vector2 newPos)
    {
        Vector2 convertedNewPos = Convert.IsoToGrid(newPos);

        //Debug.Log("pos iso: " + newPos + ", pos grid: " + convertedNewPos);

        int x = allPos[levelIndex].x;
        int y = allPos[levelIndex].y;
        int newX = Mathf.FloorToInt(convertedNewPos[0]);
        int newY = Mathf.FloorToInt(convertedNewPos[1]);

        //Can only move toward an adjacent tile
        if( (Mathf.Abs(x - newX) == 1 && y == newY) || (Mathf.Abs(y - newY) == 1 && x == newX))
        {
            SetPos(newX, newY);
            return true;
        }

        //Debug.Log("Invalid Movement: " + x + ", "+ y + " to " + newX + ", " + newY);
        return false;
    }

    //Update x and y to the new tile
    //Only call directly to bypass adjacency check (like when setting starting pos)
    public void SetPos(int newX, int newY)
    {
        //x = newX;
        //y = newY;
        allPos[levelIndex].x = newX;
        allPos[levelIndex].y = newY;
        //Debug.Log("Moved to " + x + ", " + y);
    }

    public Vector2 GetPos()
    {
        //return new Vector2(x, y);
        return new Vector2(allPos[levelIndex].x, allPos[levelIndex].y);
    }

    public void UpdateLevelIndex(int newLevelIndex)
    {
        levelIndex = newLevelIndex;
    }

    public void AddSpaceInAllPos(int howManyLevels)
    {
        for (int i = 0; i < howManyLevels; i++)
        {
            allPos.Add(new Pos2D(0, 0));
        }
    }
}

[System.Serializable]
public class Pos2D
{
    public int x;
    public int y;

    public Pos2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}