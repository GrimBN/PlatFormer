using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectDraw : MonoBehaviour
{
    Texture2D texture;    
    [SerializeField] float pixelPerUnitValue = 64f;
    //[SerializeField] int maxTextureListLength = 400 * 64 * 64;
    //List<Color> textureColors = new List<Color>();
    //List<Color> textureColors2 = new List<Color>();

    //Color[] textureColors = new Color[32000];
    //Color[] textureColors2 = new Color[32000];

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //CreateColorLists();

    }

    /*private void CreateColorLists()
    {       
        for(int i=0; i < maxTextureListLength; i++)        
        {
            textureColors.Add(Color.white);
            textureColors2.Add(Color.clear);
        }       
    }*/

    /*private void CreateNewTexture(Rect box)   //Slower than new method
    {
        int width = Mathf.RoundToInt(box.width), height = Mathf.RoundToInt(box.height);

        if(texture != null)
        {
            Destroy(texture);
        }

        texture = new Texture2D(width, height);
        List<Color> whiteTextureList = new List<Color>();
        List<Color> transparentTextureList = new List<Color>();

        for (int i = 0; i < width * height; i++)
        {
            whiteTextureList.Add(Color.white);
            transparentTextureList.Add(Color.clear);            
        }

        texture.SetPixels(whiteTextureList.ToArray());
        texture.SetPixels(2, 2, width - 4, height - 4, transparentTextureList.ToArray());
        texture.Apply();
    }    */

    private void AltCreateNewTexture(Rect box)
    {
        int width = Mathf.RoundToInt(box.width), height = Mathf.RoundToInt(box.height);

        /*if(width * height > maxTextureListLength)
        {
            Debug.Log("Size of required texture larger than max size, returning");
            return;
        }*/

        if (texture != null)
        {
            Destroy(texture);
        }

        texture = new Texture2D(width, height);

        /*Color[] whiteColors = new Color[width * height];
        Color[] clearColors = new Color[width * height];

        textureColors.CopyTo(0, whiteColors, 0, width * height);
        textureColors2.CopyTo(0, clearColors, 0, width * height);

        texture.SetPixels(whiteColors);
        texture.SetPixels(2, 2, width - 4, height - 4, clearColors);
        texture.Apply();*/
    }

    public void ClearSprite()
    {
        spriteRenderer.sprite = null;
    }

    public void DrawRect(Rect box)
    {        
        AltCreateNewTexture(box);        
        if (spriteRenderer.sprite != null)
        {
            Destroy(spriteRenderer.sprite);
        }
        spriteRenderer.sprite = Sprite.Create(texture, box, Vector2.zero, pixelPerUnitValue);
    }
}
