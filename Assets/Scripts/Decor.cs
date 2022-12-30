using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Decor", menuName = "Data/Decor", order = 1)]
public class Decor : ScriptableObject
{
    public List<DecorOnTile> allDecorOnThisLevel; // lenght >= number of tiles in the level
}

[System.Serializable]
public class DecorOnTile
{
    //public Vector2 posXY;
    public List<Decoration> decorationOnThisTile;
}

[System.Serializable]
public class Decoration
{
    public Vector2 correctionXY; // Adjust position on grid (decor sprites aren't all the same size)
    public int spriteOrder;      // Certain sprites need to be adjusted manually to get the right visual effect
    public Sprite decoSprite;
}