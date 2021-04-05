using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    //Parameters
    bool isErasing = false;
    bool isDrawing = false;
    bool isBoxSelectCoroutineRunning = false;
    bool isDrawingToolbarOpen = false;
    bool isLevelControlToolbarOpen = false;
    Touch touch;
    bool usingBoxSelect = true;    
    [SerializeField] float pixelSizeMultiplier = 64f;    
    int tileCount = 0;
    Vector3Int INVALID_POS = new Vector3Int(0, 0, -1); // Treat as a constant
    
    //Component References
    [SerializeField] RuleTile tile;    
    [SerializeField] Tilemap foreGroundTilemap;   
    [SerializeField] Tilemap hazardsTilemap;
    RectTransform toolbarBackground, levelControlBackground;
    Tilemap fixedForegroundTilemap;
    LevelController levelController;
    CompositeCollider2D validAreaCollider;    
    Rect box;
    
    //References
    RectDraw rectDraw;

    private void Start()
    {
        Input.simulateMouseWithTouches = false;
        validAreaCollider = GetComponent<CompositeCollider2D>();
        rectDraw = FindObjectOfType<RectDraw>();
        fixedForegroundTilemap = GameObject.FindGameObjectWithTag("Fixed Foreground").GetComponent<Tilemap>();
        toolbarBackground = GameObject.FindGameObjectWithTag("Drawing Toolbar").GetComponent<RectTransform>();
        levelControlBackground = GameObject.FindGameObjectWithTag("Level Control Toolbar").GetComponent<RectTransform>();        
        levelController = FindObjectOfType<LevelController>();
    }    

    private void Update()
    {
        ProcessTouchInput();
    }

    private void ProcessTouchInput()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (ValidPosition(touch.position))
            {
                if (!usingBoxSelect && (isDrawing || isErasing))
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
                    {
                        Vector3Int tilePos = GetTilePos(touch.position);
                        PlaceOrEraseTile(tilePos);
                    }
                }
                else if (usingBoxSelect && !isBoxSelectCoroutineRunning && (isDrawing || isErasing))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        StartCoroutine(BoxSelect());
                        isBoxSelectCoroutineRunning = true;
                    }
                }
            }
        }
    }

    private void OnMouseDown()
     {
         if (usingBoxSelect && ValidPosition(Input.mousePosition))
         {
             StartCoroutine(BoxSelect());
         }
     }

     private void OnMouseDrag()
     {
         if (!usingBoxSelect && ValidPosition(Input.mousePosition))
         {
             Vector3Int clickPos = GetClickPos();                       
             PlaceOrEraseTile(clickPos);            
         }
     }  

    private Vector3Int GetClickPos()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
        return GetTilePos(clickPos);
    }

    private Vector3Int GetTilePos(Vector2 posToConvert)
    {        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(posToConvert);
        Vector2 roundedWorldPos = new Vector3(Mathf.Floor(worldPos.x), Mathf.Floor(worldPos.y));
        Vector3Int tilePos = foreGroundTilemap.layoutGrid.WorldToCell(worldPos);
        return tilePos;
    }

    private bool ValidPosition(Vector2 posToValidate)
    {        
        return !(RectTransformUtility.RectangleContainsScreenPoint(toolbarBackground, posToValidate) || RectTransformUtility.RectangleContainsScreenPoint(levelControlBackground, posToValidate));
    }

    private IEnumerator BoxSelect()
    {
        Vector3Int tilePosNew;
        tilePosNew = GetClickPos();
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            tilePosNew = GetTilePos(touch.position);
        }        

        Vector3Int tilePosOld = new Vector3Int(tilePosNew.x, tilePosNew.y, tilePosNew.z);
        Vector3Int tilePosStart = new Vector3Int(tilePosNew.x, tilePosNew.y, tilePosNew.z);       
        box.x = 0;
        box.y = 0;
        box.width = pixelSizeMultiplier;
        box.height = pixelSizeMultiplier;
        rectDraw.gameObject.transform.position = new Vector3(tilePosNew.x, tilePosNew.y, 0);
        rectDraw.DrawRect(box);        

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            while (Input.GetButton("Fire1"))
            {
                
                tilePosNew = GetClickPos();                                                                                  
                CalcBoxShape(tilePosNew, ref tilePosOld, ref tilePosStart);                
                yield return null;
            }
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            while (touch.phase != TouchPhase.Ended)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    tilePosNew = GetTilePos(touch.position);
                    CalcBoxShape(tilePosNew, ref tilePosOld, ref tilePosStart);
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        break;
                    }
                }
                yield return null;
            }
        }

        BoxFillOrErase();
        rectDraw.ClearSprite();
        isBoxSelectCoroutineRunning = false;
    }

    private void CalcBoxShape(Vector3Int tilePosNew, ref Vector3Int tilePosOld, ref Vector3Int tilePosStart)
    {
        if (tilePosNew != tilePosOld)
        {
            //calculating change in width of box
            if (tilePosNew.x > tilePosStart.x)  // position of object remains same and only width needs to change
            {
                box.width = (tilePosNew.x - tilePosStart.x + 1) * pixelSizeMultiplier;
            }
            else if (tilePosNew.x < tilePosStart.x)  // position of object is shifted instead of the x origin of the Rect to be drawn because this actually worked
            {
                box.width = (tilePosStart.x - tilePosNew.x + 1) * pixelSizeMultiplier;
                rectDraw.gameObject.transform.position = new Vector3(tilePosNew.x, rectDraw.gameObject.transform.position.y, 0);
            }
            else if (tilePosNew.x == tilePosStart.x) // reset width and x position of object
            {
                box.width = pixelSizeMultiplier;
                rectDraw.gameObject.transform.position = new Vector3(tilePosStart.x, rectDraw.gameObject.transform.position.y, 0);
            }

            //calculating change in height of box
            if (tilePosNew.y > tilePosStart.y)  // position of object remains same and only height needs to change
            {
                box.height = (tilePosNew.y - tilePosStart.y + 1) * pixelSizeMultiplier;
            }
            else if (tilePosNew.y < tilePosStart.y) // position of object is shifted instead of the y origin of the Rect to be drawn because this actually worked
            {
                box.height = (tilePosStart.y - tilePosNew.y + 1) * pixelSizeMultiplier;
                rectDraw.gameObject.transform.position = new Vector3(rectDraw.gameObject.transform.position.x, tilePosNew.y, 0);
            }
            else if (tilePosNew.y == tilePosStart.y)    // reset height and y position of object
            {
                box.height = pixelSizeMultiplier;
                rectDraw.gameObject.transform.position = new Vector3(rectDraw.gameObject.transform.position.x, tilePosStart.y, 0);
            }
            rectDraw.DrawRect(box);
            tilePosOld = tilePosNew;
        }
    }

    private void BoxFillOrErase()
    {
        Vector3Int boxLocationInInt = new Vector3Int(Mathf.RoundToInt(rectDraw.gameObject.transform.position.x), Mathf.RoundToInt(rectDraw.gameObject.transform.position.y), Mathf.RoundToInt(rectDraw.gameObject.transform.position.z));
        Vector3Int boxSizeInInt = new Vector3Int(Mathf.RoundToInt(box.width / pixelSizeMultiplier), Mathf.RoundToInt(box.height / pixelSizeMultiplier), 1);
        BoundsInt boxToFill = new BoundsInt(boxLocationInInt, boxSizeInInt);

        if (isDrawing)
        {
            foreach (Vector3Int position in boxToFill.allPositionsWithin)
            {
                if (validAreaCollider.OverlapPoint(new Vector2(position.x, position.y)) && !hazardsTilemap.HasTile(position) && !foreGroundTilemap.HasTile(position))
                {
                    foreGroundTilemap.SetTile(position, tile);
                    tileCount++;
                    levelController.DecreaseBlockCount(); //decrease because user has used one of his available blocks and now has one less block left
                }
            }
        }
        else if(isErasing)
        {
            foreach (Vector3Int position in boxToFill.allPositionsWithin)
            {
                if (validAreaCollider.OverlapPoint(new Vector2(position.x, position.y)) && foreGroundTilemap.HasTile(position))
                {
                    foreGroundTilemap.SetTile(position, null);
                    tileCount--;
                    levelController.IncreaseBlockCount(); //increase because removing a block means user has one more block available to play
                }
            }
        }        
    }

    private void PlaceOrEraseTile(Vector3Int tilePos)
    {
        //Vector3Int tilePos = GetClickPos();
        if (foreGroundTilemap.HasTile(tilePos) && isErasing)
        {
            foreGroundTilemap.SetTile(tilePos, null);
            tileCount--;
            levelController.IncreaseBlockCount(); //increase because removing a block means user has one more block available to play
        }        
        else if (!foreGroundTilemap.HasTile(tilePos) && isDrawing && validAreaCollider.OverlapPoint(new Vector2(tilePos.x, tilePos.y)) && !hazardsTilemap.HasTile(tilePos) && !fixedForegroundTilemap.HasTile(tilePos))
        {            
            foreGroundTilemap.SetTile(tilePos, tile);
            tileCount++;
            levelController.DecreaseBlockCount(); //decrease because user has used one of his available blocks and now has one less block left
        }        
    } 

    public void FlipDrawingToolbarOpenStatus()
    {
        isDrawingToolbarOpen = !isDrawingToolbarOpen;
    }

    public void FlipLevelControlToolbarOpenStatus()
    {
        isLevelControlToolbarOpen = !isLevelControlToolbarOpen;
    }

    public void SetBoxSelect(bool value)
    {
        usingBoxSelect = value;
    }

    public void SetErasing(bool value)
    {
        isErasing = value;
    }

    public bool GetErasing()
    {
        return isErasing;
    }

    public void SetDrawing(bool value)
    {
        isDrawing = value;
    }

    public bool GetDrawing()
    {
        return isDrawing;
    }

    public int GetTileCount()
    {
        foreGroundTilemap.CompressBounds();
        return tileCount;
    }
}
