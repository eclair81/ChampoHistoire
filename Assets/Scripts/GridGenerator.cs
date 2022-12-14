using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject isoTilePrefab;
    //[Header("Camera")]
    //[SerializeField] private Transform cam;
    [Header("Player")]
    [SerializeField] private GameObject player;
    [Header("Container")]
    [SerializeField] private Transform parent;

    private bool startAlreadyPlaced;
    private int numberOfEventTileAlreadyPlaced;
    private int maxNumberOfEventTile;

    //Generate a custom grid based on a Texture2D
    public void GenerateCustomGrid(Texture2D map)
    {
        startAlreadyPlaced = false;
        numberOfEventTileAlreadyPlaced = 0;
        maxNumberOfEventTile = GameManager.Instance.NumberOfObjectsInLevel();


        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixel = map.GetPixel(x, y);
                //Debug.Log(pixel);
                GenerateTile(x, y, pixel);
            }
        }
        //cam.transform.position = new Vector3((float)map.width/2 - 0.5f, (float)map.height/2 -0.5f, -10);
        //cam.gameObject.GetComponent<Camera>().orthographicSize = map.height;
    }

    private void GenerateTile(int x, int y, Color pixel)
    {
        if(pixel.a == 0) return; //Ignore transparent pixels

        //If Black Pixel -> normal tile
        if(pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
        {
            SpawnTile(x, y, "normal");
            return;
        }

        //If Green Pixel -> event tile
        if(pixel.r == 0 && pixel.g == 1.0f && pixel.b == 0)
        {
            //Don't place more event tiles than there are objects in levelList
            if (numberOfEventTileAlreadyPlaced < maxNumberOfEventTile)
            {
                numberOfEventTileAlreadyPlaced++;
                SpawnTile(x, y, "event");
                return;
            }

            SpawnTile(x, y, "normal");
            return;
        }

        //If Blue pixel -> start tile, 
        if(pixel.r == 0 && pixel.g == 0 && pixel.b == 1.0f)
        {
            // only one start tile per grid
            if(startAlreadyPlaced)
            {
                SpawnTile(x, y, "normal");
                return;
            }

            SpawnTile(x, y, "start");
            MoveOnGrid.Instance.SetPos(x, y);
            startAlreadyPlaced = true;
            //Instantiate(player, Convert.GridToIso(new Vector2(x, y)), Quaternion.identity);
            player.transform.position = Convert.GridToIso(new Vector2(x, y));
            return;
        }
    }

    private void SpawnTile(int x, int y, string etat)
    {
        GameObject tile = Instantiate(isoTilePrefab, Convert.GridToIso(new Vector2(x, y)), Quaternion.identity, parent);
        Tile tileScript = tile.GetComponent<Tile>();
        bool offset = ((x + y) % 2 == 1); //alternate for a checkboard grid, remove since we now use iso?
        tileScript.Init(offset, etat);
    }
}
