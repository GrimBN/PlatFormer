using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarSlider : MonoBehaviour
{
    [SerializeField] float maxXMultiplier, lerpValue;
    //[SerializeField] GameObject blockingCollider;
    [SerializeField] GameObject toolbarParentObject;
    float differenceMultiplier, objectDifferenceMultiplier, minColliderXValue, maxColliderXValue, minObjectXValue, maxObjectXValue;
    Slider slider;
    Sprite flippedArrowSprite;
    Image childArrowImage;

    private void Start()
    {        
        maxObjectXValue = Screen.width * maxXMultiplier;
        minObjectXValue = toolbarParentObject.transform.position.x;
        //minColliderXValue = Camera.main.ScreenToWorldPoint(new Vector3(minObjectXValue, 0, blockingCollider.transform.position.z)).x;
        //maxColliderXValue = Camera.main.ScreenToWorldPoint(new Vector3(maxObjectXValue, 0, blockingCollider.transform.position.z)).x;
        //blockingCollider.transform.position = new Vector2(minColliderXValue, blockingCollider.transform.position.y);
        //differenceMultiplier = maxColliderXValue - minColliderXValue;        
        objectDifferenceMultiplier = maxObjectXValue - minObjectXValue;

        flippedArrowSprite = Resources.Load<Sprite>("FlippedArrow");
        childArrowImage = gameObject.GetComponentInChildren<Image>();
        slider = GetComponent<Slider>();      
        UpdatePositions(slider.value);
    }

    public void UpdatePositions(float value)
    {
        //UpdateColliderPos(value);
        UpdateParentObjectPos(value);
        //StartCoroutine(OpenSlider());
    }

    /*private void UpdateColliderPos(float value)
    {
        if(value <=1 && value >=0)
        {
            Vector2 newColliderPos = new Vector2(minColliderXValue + value * differenceMultiplier, blockingCollider.transform.position.y);
            blockingCollider.transform.position = newColliderPos;
        }
    }*/

    private void UpdateParentObjectPos(float value)
    {
        if (value <= 1 && value >= 0)
        {            
            Vector2 newObjectPos = new Vector2(minObjectXValue + value * objectDifferenceMultiplier, toolbarParentObject.transform.position.y);
            toolbarParentObject.transform.position = newObjectPos;
        }
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
                //UpdateColliderPos(slider.value);
                UpdateParentObjectPos(slider.value);
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
                //UpdateColliderPos(slider.value);
                UpdateParentObjectPos(slider.value);
                yield return null;
            }
            
            slider.value = 1;
        }
    }
}
