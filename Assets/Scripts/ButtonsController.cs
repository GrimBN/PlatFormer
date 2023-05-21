using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] Button modeTumButton;
    [SerializeField] Button modeAlternateButton;
    //GameDataController gameDataController;
    //GameMode gameModeController;

    void Start()
    {
        //gameDataController = FindObjectOfType<GameDataController>();
        //gameModeController = FindObjectOfType<GameMode>();
        //if(gameDataController != null)
        //{
        modeTumButton.interactable = GameDataController.instance.GetTumUnlockedStatus();
        modeAlternateButton.interactable = GameDataController.instance.GetAlternateUnlockedStatus();
        if (!modeAlternateButton.interactable)
        {
            modeAlternateButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Locked";
        }
        //}        
    }

    public void SetNormalGameMode()
    {
        GameMode.instance.SetModeNormal();
    }

    public void SetAlternateMode()
    {
        GameMode.instance.SetModeAlternate();
    }

    public void SetTimCharacter()
    {
        GameMode.instance.SetCharacterTim();
    }

    public void SetTumCharacter()
    {
        GameMode.instance.SetCharacterTum();
    }

}
