using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] Button modeTumButton;
    [SerializeField] Button modeAlternateButton;
    GameDataController gameDataController;

    void Start()
    {
        gameDataController = FindObjectOfType<GameDataController>();
        if(gameDataController != null)
        {
            modeTumButton.interactable = gameDataController.GetTumUnlockedStatus();
            modeAlternateButton.interactable = gameDataController.GetAlternateUnlockedStatus();
        }
    }

}
