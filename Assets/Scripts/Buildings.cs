using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Data/Buildings", order = 1)]
public class Buildings : ScriptableObject
{
    public List<Building> buildingsOnThisLevel;
}

[System.Serializable]
public class Building
{
    public Vector2 posXY;
    public int spriteOrder;       // Certain sprites need to be adjusted manually to get the right visual effect
    public GameObject buildingPrefab;
}