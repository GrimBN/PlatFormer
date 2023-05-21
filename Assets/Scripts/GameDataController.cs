using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class Constants
{
    public const int NUMBER_OF_LEVELS = 20;
    public const int TUM_UNLOCK_STARS = 10;
    public const int ALTERNATE_UNLOCK_STARS = 15;
}

public class GameDataController : MonoBehaviour
{
    private int levelsUnlocked;    
    private bool tumUnlockedStatus = false;
    private bool alternateUnlockedStatus = false;

    private int[] normalStarsCollected, alternateStarsCollected;

    public static GameDataController instance;
  
    void Awake()
    {
        int numberOfGameDataControllerInstances = FindObjectsOfType<GameDataController>().Length;        
        if (numberOfGameDataControllerInstances > 1)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        LoadData();
    }    

    private void SaveData()
    {        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.sav");
        GameData gameData = new GameData
        {
            levelsUnlocked = levelsUnlocked,
            normalStarsCollected = normalStarsCollected,
            alternateStarsCollected = alternateStarsCollected,
            tumUnlockedStatus = tumUnlockedStatus,
            alternateUnlockedStatus = alternateUnlockedStatus
        };

        bf.Serialize(file, gameData);
        file.Close();                
    }

    public void LoadData()
    {
        if (!File.Exists(Application.persistentDataPath + "/Save.sav"))
        {
            levelsUnlocked = 1;
            normalStarsCollected = new int[20];
            alternateStarsCollected = new int[20];
            for(int i = 0; i < Constants.NUMBER_OF_LEVELS; i++)
            {
                normalStarsCollected[i] = 0;
                alternateStarsCollected[i] = 0;
            }
            SaveData();
        }

        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save.sav", FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();
            
            levelsUnlocked = gameData.levelsUnlocked;
            normalStarsCollected = gameData.normalStarsCollected;
            alternateStarsCollected = gameData.alternateStarsCollected;
            
            tumUnlockedStatus = gameData.tumUnlockedStatus;
            alternateUnlockedStatus = gameData.alternateUnlockedStatus;
        }
    }

    public void UpdateData(int completedLevel, int starCount)
    {
        if (GameMode.instance.Mode == GameMode.Modes.Normal && starCount > normalStarsCollected[completedLevel - 1])
        {
            normalStarsCollected[completedLevel - 1] = starCount;
        }
        else if (GameMode.instance.Mode == GameMode.Modes.Alternate && starCount > alternateStarsCollected[completedLevel - 1])
        {
            alternateStarsCollected[completedLevel - 1] = starCount;
        }

        if (completedLevel == levelsUnlocked)
        {
            levelsUnlocked++;
        }

        CheckForUnlocks();

        SaveData();
    }

    private void CheckForUnlocks()
    {
        int totalStars = 0;
        for (int i = 0; i < normalStarsCollected.Length; i++)
        {
            totalStars += normalStarsCollected[i];
        }        

        if (totalStars >= Constants.TUM_UNLOCK_STARS && !tumUnlockedStatus)
        {
            tumUnlockedStatus = true;            
        }

        if (totalStars >= Constants.ALTERNATE_UNLOCK_STARS && !alternateUnlockedStatus)
        {
            alternateUnlockedStatus = true;
            FindObjectOfType<LevelController>().SetUnlockTextActive();
        }
    }

    /*void Update()
    {
        if(Input.GetAxis("ZoomIn") > 0 && Input.GetAxis("ZoomOut") > 0 && Input.GetKeyDown(KeyCode.K))
        {
            File.Delete(Application.persistentDataPath + "/Save.sav");
        }
        if (Input.GetAxis("ZoomIn") > 0 && Input.GetAxis("ZoomOut") > 0 && Input.GetKeyDown(KeyCode.J))
        {
            for (int i = 0; i < 5; i++)
            {
                normalStarsCollected[i] = 3;
                alternateStarsCollected[i] = 3;
            }
        }
    }*/

    public int GetLevelsUnlocked()
    {
        return levelsUnlocked;
    }

    public bool GetTumUnlockedStatus()
    {
        return tumUnlockedStatus;
    }

    public bool GetAlternateUnlockedStatus()
    {
        return alternateUnlockedStatus;
    }

    public int[] GetNormalStarsCollected()
    {
        return normalStarsCollected;
    }

    public int[] GetAlternateStarsCollected()
    {
        return alternateStarsCollected;
    }
}
