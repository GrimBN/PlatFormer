﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    [SerializeField] int levelsUnlocked;    //Set to private for build, only for debugging
    private bool tumUnlockedStatus = false;
    private bool alternateUnlockedStatus = false;

    private int[] normalStarsCollected, alternateStarsCollected;

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
        //SaveData();
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
            for(int i = 0; i < NUMBER_OF_LEVELS; i++)
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
            tumUnlockedStatus = gameData.tumUnlockedStatus;
            alternateUnlockedStatus = gameData.alternateUnlockedStatus;
        }
    }

    public void UpdateData(int completedLevel, int starCount, GameMode gameMode)
    {
        if (gameMode.GetMode() == GameMode.Modes.Normal)
        {
            normalStarsCollected[completedLevel] = starCount;
        }
        else if (gameMode.GetMode() == GameMode.Modes.Alternate)
        {
            alternateStarsCollected[completedLevel] = starCount;
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

        if (totalStars >= TUM_UNLOCK_STARS)
        {
            tumUnlockedStatus = true;
        }

        if (totalStars >= ALTERNATE_UNLOCK_STARS)
        {
            alternateUnlockedStatus = true;
        }
    }

    void Update()
    {
        if(Input.GetAxis("ZoomIn") > 0 && Input.GetAxis("ZoomOut") > 0 && Input.GetKeyDown(KeyCode.K))
        {
            File.Delete(Application.persistentDataPath + "/Save.sav");
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

    public int[] GetNormalStarsCollected()
    {
        return normalStarsCollected;
    }

    public int[] GetAlternateStarsCollected()
    {
        return alternateStarsCollected;
    }
}
