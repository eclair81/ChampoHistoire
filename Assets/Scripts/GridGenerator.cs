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
    [Header("Custom Grid")]
    [SerializeField] private Texture2D map;

    void Start()
    {
        //GenerateGrid();
        GenerateCustomGrid();
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

    //Generate a custom grid based on a Texture2D
    private void GenerateCustomGrid(/*Texture2D map*/)
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixel = map.GetPixel(x, y);
                //Debug.Log(pixel);
                GenerateTile(x, y, pixel);
            }
        }
        cam.transform.position = new Vector3((float)map.width/2 - 0.5f, (float)map.height/2 -0.5f, -10);
        cam.gameObject.GetComponent<Camera>().orthographicSize = map.height/2;
    }

    private void GenerateTile(int x, int y, Color pixel)
    {
        if(pixel.a == 0) return; //Ignore transparent pixels

        //Only using black pixels for now
        if(pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
            Tile tileScript = tile.GetComponent<Tile>();
            bool offset = ((x + y) % 2 == 1); //alternate for a checkboard grid
            tileScript.Init(offset);
        }
    }

}
