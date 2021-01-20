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
                var stars = levels[i].GetComponentsInChildren<Image>(true);
                if (gameMode.GetMode() == GameMode.Modes.Normal)
                {
                    for (int j = 1; j <= normalStars[i]; j++)       //starting loop from 1 to 3 because the level button's image is also gotten by GetComponentsInChildren() above
                    {                        
                        stars[j].gameObject.SetActive(true);
                    }
                }
                else if(gameMode.GetMode() == GameMode.Modes.Alternate)
                {
                    for (int j = 1; j <= alternateStars[i]; j++)
                    {
                        stars[j].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
