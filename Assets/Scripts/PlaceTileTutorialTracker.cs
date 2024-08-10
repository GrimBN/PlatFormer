using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlaceTileTutorialTracker : MonoBehaviour
{
    [SerializeField] private Tilemap foregroundTilemap;
    [SerializeField] private GameObject currentTutorialBox;
    [SerializeField] private GameObject nextTutorialBox;
    [SerializeField] private Button arrowButton;


    private void Start() 
    {
        Tilemap.tilemapTileChanged += ForegroundTilemapChanged;
    }

    //The TilePlacer.cs script uses SetTile() function even when multiple tiles are placed at once with the box draw tool
    //Therefore this callback is called for each tile that was placed/erased in a single box
    private void ForegroundTilemapChanged(Tilemap changedTilemap, Tilemap.SyncTile[] syncTiles)
    {
        string msg = changedTilemap.name + ": ";

        //The tile being used is a RuleTile which affecs all 8 adjecent tiles around it
        //That is why the returned SyncTile array has 9 elements in it, one for the center tile at index 4
        //which is the tile that was directly changed and the tiles at index 0-3 and 5-8 which are the surrounding tiles
        /* foreach(Tilemap.SyncTile tile in syncTiles)
        {
            Debug.Log(tile.tile == null);
            msg += tile.position.ToString() + " ; "; 
        } */
        msg += syncTiles[4].position.ToString() + " - " + (syncTiles[4].tile != null);

        Debug.Log(msg);

        bool allTilesPlaced = true;
        for(int i = -10; i <= 6; i++ )
        {
            if( i > -5 && i < 0) {continue;}

            Vector3Int pos = new Vector3Int(i, -6, 0); //Values of the restricted tile positions
            if(!foregroundTilemap.HasTile(pos))
            {
                allTilesPlaced = false;
            }

        }
        
        if(allTilesPlaced)
        {
            currentTutorialBox.SetActive(false);
            nextTutorialBox.SetActive(true);
            arrowButton.interactable = true;
        }
    }
}
