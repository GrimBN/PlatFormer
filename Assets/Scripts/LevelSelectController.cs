using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    //GameDataController gameDataController;

    void Awake()
    {
        //gameDataController = FindObjectOfType<GameDataController>();
        levels = GetComponentsInChildren<Button>();
        ActivateLevels();
    }

    private void ActivateLevels()
    {
        if (GameDataController.instance != null)
        {
            int[] normalStars = GameDataController.instance.GetNormalStarsCollected(), alternateStars = GameDataController.instance.GetAlternateStarsCollected();
            
            for (int i = 0; i < GameDataController.instance.GetLevelsUnlocked(); i++)
            {                
                levels[i].interactable = true;
                var stars = levels[i].GetComponentsInChildren<Image>(true);
                if (GameMode.instance.Mode == GameMode.Modes.Normal)
                {
                    for (int j = 1; j <= normalStars[i]; j++)       //starting loop from 1 to 3 because the level button's image is also gotten by GetComponentsInChildren() above
                    {                        
                        stars[j].gameObject.SetActive(true);
                    }
                }
                else if(GameMode.instance.Mode == GameMode.Modes.Alternate)
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
