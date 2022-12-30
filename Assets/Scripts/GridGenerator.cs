using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject isoTilePrefab;
    [SerializeField] private GameObject decorPrefab;
    //[Header("Camera")]
    //[SerializeField] private Transform cam;
    [Header("Player")]
    [SerializeField] private GameObject player;
    [Header("Containers")]
    [SerializeField] private Transform tileContainer;
    [SerializeField] private Transform decorContainer;
    [SerializeField] private Transform buildingContainer;

    private bool startAlreadyPlaced;
    private int numberOfEventTileAlreadyPlaced;
    private int maxNumberOfEventTile;

    //Generate a custom grid based on a Texture2D
    public void GenerateCustomGrid(Texture2D map, GameObject tilePrefab, List<DecorOnTile> decorList, List<Building> buildingList)
    {
        isoTilePrefab = tilePrefab;
        int index = 0; // index for decorList

        startAlreadyPlaced = false;
        numberOfEventTileAlreadyPlaced = 0;
        maxNumberOfEventTile = GameManager.Instance.NumberOfObjectsInLevel();


        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixel = map.GetPixel(x, y);

                GenerateTile(x, y, pixel);
                GenerateDecor(x, y, decorList[index].decorationOnThisTile);
                index++;
            }
        }
        //cam.transform.position = new Vector3((float)map.width/2 - 0.5f, (float)map.height/2 -0.5f, -10);
        //cam.gameObject.GetComponent<Camera>().orthographicSize = map.height;

        GenerateBuildings(buildingList);
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
        GameObject tile = Instantiate(isoTilePrefab, Convert.GridToIso(new Vector2(x, y)), Quaternion.identity, tileContainer);
        Tile tileScript = tile.GetComponent<Tile>();
        tileScript.Init(etat);
    }


    private void GenerateDecor(int x, int y, List<Decoration> decorList)
    {
        foreach(Decoration deco in decorList)
        {
            //Spawn deco
            GameObject decor = Instantiate(decorPrefab, Convert.GridToIso(new Vector2(x, y)), Quaternion.identity, decorContainer);
            //Apply correction
            decor.transform.position += (Vector3)deco.correctionXY;
            //Apply Sprite
            SpriteRenderer spriteRenderer = decor.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = deco.decoSprite;

            if(deco.spriteOrder != 0) spriteRenderer.sortingOrder = deco.spriteOrder;
        }
    }


    private void GenerateBuildings(List<Building> buildingList)
    {
        foreach (Building batiment in buildingList)
        {
            GameObject thisBuilding = Instantiate(batiment.buildingPrefab, batiment.posXY, Quaternion.identity, buildingContainer);
            thisBuilding.GetComponent<SpriteRenderer>().sortingOrder = batiment.spriteOrder;
        }
    }
}
