using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutCanvas : MonoBehaviour
{
    Image image;
    [SerializeField] float fadeRate, delay;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        Color newColor = new Color(image.color.r, image.color.g, image.color.b, 0f);
        while(image.color.a < 1)
        {
            newColor = new Color(image.color.r, image.color.g, image.color.b, image.color.a + fadeRate);
            image.color = newColor;
            //yield return new WaitForSeconds(delay);
        }        
    }

    public IEnumerator FadeIn()
    {
        Color newColor = new Color(image.color.r, image.color.g, image.color.b, 1f);
        while (image.color.a > 0)
        {
            newColor = new Color(image.color.r, image.color.g, image.color.b, image.color.a - fadeRate);
            image.color = newColor;
            yield return new WaitForSeconds(delay);
        }
        gameObject.SetActive(false);
    }
}
