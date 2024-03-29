﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectDraw : MonoBehaviour
{
    Texture2D texture;    
    [SerializeField] float pixelPerUnitValue = 64f;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void AltCreateNewTexture(Rect box)
    {
        int width = Mathf.RoundToInt(box.width), height = Mathf.RoundToInt(box.height);

        if (texture != null)
        {
            Destroy(texture);
        }

        texture = new Texture2D(width, height);        
    }

    public void ClearSprite()
    {
        spriteRenderer.enabled = false;
    }

    /* public void DrawRect(Rect box)
    {        
        AltCreateNewTexture(box);        
        if (spriteRenderer.sprite != null)
        {
            Destroy(spriteRenderer.sprite);
        }
        spriteRenderer.sprite = Sprite.Create(texture, box, Vector2.zero, pixelPerUnitValue);
    }
 */
    public void DrawRect(Rect box)
    {        
        if(spriteRenderer.drawMode != SpriteDrawMode.Sliced)
        {
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        if(!spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }

        spriteRenderer.size = box.size;
    }
}
