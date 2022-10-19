using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int rows, cols;
    [Header("Prefab")]
    [SerializeField] private GameObject tilePrefab;
    [Header("Camera")]
    [SerializeField] private Transform cam;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                Tile tileScript = tile.GetComponent<Tile>();
                bool offset = ((i + j) % 2 == 1); //alternate for a checkboard grid
                tileScript.Init(offset);
            }
        }
        //Place camera at the center of the grid
        cam.transform.position = new Vector3((float)rows/2 - 0.5f, (float)cols/2 -0.5f, -10);
    }
}
