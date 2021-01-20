using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{    
    [SerializeField] Button modeTumButton;    
    [SerializeField] Button modeAlternateButton;
    GameDataController gameDataController;
    GameMode gameModeController;

    void Start()
    {
        gameDataController = FindObjectOfType<GameDataController>();
        gameModeController = FindObjectOfType<GameMode>();
        if(gameDataController != null)
        {
            modeTumButton.interactable = gameDataController.GetTumUnlockedStatus();
            modeAlternateButton.interactable = gameDataController.GetAlternateUnlockedStatus();
        }        
    }

    public void SetNormalGameMode()
    {
        gameModeController.SetModeNormal();
    }

    public void SetAlternateMode()
    {
        gameModeController.SetModeAlternate();
    }

    public void SetTimCharacter()
    {
        gameModeController.SetCharacterTim();
    }

    public void SetTumCharacter()
    {
        gameModeController.SetCharacterTum();
    }

}
