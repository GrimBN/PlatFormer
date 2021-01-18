using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    private int levelsUnlocked;
    private bool tumUnlockedStatus = false;
    private bool alternateUnlockedStatus = false;

    public int[] normalStarsCollected, alternateStarsCollected;

    private const int NUMBER_OF_LEVELS = 20;
    private const int TUM_UNLOCK_STARS = 10;
    private const int ALTERNATE_UNLOCK_STARS = 10;

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
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        LoadData();
    }

    private void SaveData()
    {        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.sav");
        GameData gameData = new GameData
        {
            levelsUnlocked = levelsUnlocked,
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
            SaveData();
        }

        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save.sav", FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();

            levelsUnlocked = gameData.levelsUnlocked;
            tumUnlockedStatus = gameData.tumUnlockedStatus;
            alternateUnlockedStatus = gameData.alternateUnlockedStatus;
        }
    }

    public void UpdateData(int completedLevel)
    {
        if(completedLevel == levelsUnlocked)
        {
            levelsUnlocked++;
            if (levelsUnlocked == TUM_UNLOCK_STARS) tumUnlockedStatus = true;
            if (levelsUnlocked == ALTERNATE_UNLOCK_STARS) alternateUnlockedStatus = true;
            SaveData();
        }
    }

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
}
