using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    GameDataController gameDataController;

    void Awake()
    {
        gameDataController = FindObjectOfType<GameDataController>();
        ActivateLevels();
    }

    private void ActivateLevels()
    {
        if (gameDataController != null)
        {
            for (int i = 0; i < gameDataController.GetLevelsUnlocked(); i++)
            {
                levels[i].interactable = true;
            }
        }
    }
}
