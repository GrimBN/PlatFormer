using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarSlider : MonoBehaviour
{
    [SerializeField] float  lerpValue;    
    Slider slider;
    Sprite flippedArrowSprite;
    Image childArrowImage;

    private void Start()
    {
        flippedArrowSprite = Resources.Load<Sprite>("FlippedArrow");
        childArrowImage = gameObject.GetComponentInChildren<Image>();
        slider = GetComponent<Slider>();
        childArrowImage.overrideSprite = slider.value == 0 ? flippedArrowSprite : null;
    }

    public void StartSliderCoroutine()
    {
        StartCoroutine(OpenSlider());
    }

    private IEnumerator OpenSlider()
    {
        if(slider.value == 1)
        {
            childArrowImage.overrideSprite = flippedArrowSprite;
            while (slider.value > 0.01)
            {
                slider.value = Mathf.Lerp(0, slider.value, 1-lerpValue);                                
                yield return null;
            }
            
            slider.value = 0;            
        }
        else if (slider.value == 0)
        {
            childArrowImage.overrideSprite = null;
            while (slider.value < 0.99)
            {
                slider.value = Mathf.Lerp(slider.value, 1, lerpValue);                                
                yield return null;
            }
            
            slider.value = 1;
        }
    }
}
