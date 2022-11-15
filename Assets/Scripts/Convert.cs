using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Convert 
{
    // /!\ the sprite's size used for the iso tile must be equal to 1 unity unit (or scaled to be)

    // Transform normal grid to Iso, halved to avoid "chessboard" pattern (void between each tile)
    private static Vector2 isoX = new Vector2(0.5f, 0.25f);
    private static Vector2 isoY = new Vector2(-0.5f, 0.25f);

    // determinant used to invert the matrix -> iso to grid
    private static float det = (1 / (isoX.x * isoY.y - isoY.x * isoX.y));

    private static Vector2 invX = new Vector2(det * isoY.y, det * -isoX.y);
    private static Vector2 invY = new Vector2(det * -isoY.x, det * isoX.x);


    //Convert normal grid position to isometric position
    public static Vector3 GridToIso(Vector2 gridPos)
    {
        float posIsoX = (gridPos.x * isoX.x) + (gridPos.y * isoY.x);
        float posIsoY = (gridPos.x * isoX.y) + (gridPos.y * isoY.y);
        return new Vector3(posIsoX, posIsoY);
    }

    //Convert isometric position to normal grid position
    public static Vector2 IsoToGrid(Vector2 isoPos)
    {
        float posGridX = isoPos.x * invX.x + isoPos.y * invY.x;
        float posGridY = isoPos.x * invX.y + isoPos.y * invY.y;
        return new Vector2(posGridX, posGridY);
    }

    // 0/1/2/3 -> Up-Right/Up-Left/Down-Left/Down-Right
    public static int AngleToDir(Vector2 vectDir)
    {
        float sign = (vectDir.y >= 0) ? 1 : -1; // get if the swipe is upward or downward
        float offset = (sign == 1) ? 0 : 360;   // add an offset to get a 0-360° angle
        float angle = Vector2.Angle(new Vector2(1, 0), vectDir) * sign + offset; // compare to right (normal) vector

        if(angle <= 90f)
        {
            //Debug.Log("Up-Right");
            return 0;
        }
        if(angle > 90f && angle <= 180f)
        {
            //Debug.Log("Up-Left");
            return 1;
        }
        if(angle > 180f && angle <= 270f)
        {
            //Debug.Log("Down-Left");
            return 2;
        }
        if(angle > 270f)
        {
            //Debug.Log("Down-Right");
            return 3;
        }
        return -1;
    }
}
