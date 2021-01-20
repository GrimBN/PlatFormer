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
        levels = GetComponentsInChildren<Button>();
        ActivateLevels();
    }

    private void ActivateLevels()
    {
        if (gameDataController != null)
        {
            int[] normalStars = gameDataController.GetNormalStarsCollected(), alternateStars = gameDataController.GetAlternateStarsCollected();
            GameMode gameMode = FindObjectOfType<GameMode>();
            for (int i = 0; i < gameDataController.GetLevelsUnlocked(); i++)
            {
                levels[i].interactable = true;
                var stars = levels[i].GetComponentsInChildren<Image>();
                if (gameMode.GetMode() == GameMode.Modes.Normal)
                {
                    for (int j = 0; j < normalStars[i]; j++)
                    {
                        stars[j].gameObject.SetActive(true);
                    }
                }
                else if(gameMode.GetMode() == GameMode.Modes.Alternate)
                {
                    for (int j = 0; j < alternateStars[i]; j++)
                    {
                        stars[j].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
