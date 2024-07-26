using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTutorialAdvancer : MonoBehaviour
{
    private bool arrowClicked;
    [SerializeField] private GameObject tutorialBox;
    private Button arrowButton;

    void Start()
    {
        arrowClicked = false;
        arrowButton = GetComponent<Button>();
        arrowButton.interactable = false;
    }

    public void ArrowClicked()
    {
        if(!arrowClicked)
        {
            arrowClicked = true;
            tutorialBox.SetActive(true);
            arrowButton.interactable = true;
        }
    }



    
}
