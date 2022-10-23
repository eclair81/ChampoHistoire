using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [Header("Camera")]
    [SerializeField] private Transform cam;
    [Header("Custom Grid")]
    [SerializeField] private Texture2D map;

    void Start()
    {
        GenerateCustomGrid();
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

        //If Black Pixel -> normal tile
        if(pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
        {
            /*GameObject tile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
            Tile tileScript = tile.GetComponent<Tile>();
            bool offset = ((x + y) % 2 == 1); //alternate for a checkboard grid
            tileScript.Init(offset, false);*/
            SpawnTile(x, y, false);
            return;
        }

        //If Green Pixel -> event tile
        if(pixel.r == 0 && pixel.g == 1.0f && pixel.b == 0)
        {
            /*GameObject tile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
            Tile tileScript = tile.GetComponent<Tile>();
            bool offset = ((x + y) % 2 == 1); //alternate for a checkboard grid
            tileScript.Init(offset, true);*/
            SpawnTile(x, y, true);
            return;
        }
    }

    private void SpawnTile(int x, int y, bool isEvent)
    {
        GameObject tile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
        Tile tileScript = tile.GetComponent<Tile>();
        bool offset = ((x + y) % 2 == 1); //alternate for a checkboard grid
        tileScript.Init(offset, isEvent);
    }

}
