using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathing : MonoBehaviour
{
    Tilemap pathTilemap;
    TileBase[] tilesArray;

    void Start()
    {
        pathTilemap = GetComponent<Tilemap>();
    }
    
    private void CalcTileArray()
    {
        tilesArray = pathTilemap.GetTilesBlock(pathTilemap.cellBounds);
        foreach(TileBase tile in tilesArray)
        {
           // Debug.Log(tile.)
        }
    }
}
