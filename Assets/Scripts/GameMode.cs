using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public enum Modes
    {
        Normal, Alternate
    }

    public enum Character
    {
        Tim, Tum
    }

    [SerializeField] Modes mode = Modes.Normal;
    [SerializeField] Character character = Character.Tim;

    private void Awake()
    {
        int numberOfGameModeInstances = FindObjectsOfType<GameMode>().Length;
        if (numberOfGameModeInstances > 1)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    public void SetModeNormal()
    {
        mode = Modes.Normal;
    }

    public void SetModeAlternate()
    {
        mode = Modes.Alternate;
    }

    public Modes GetMode()
    {
        return mode;
    }

    public void SetCharacterTim()
    {
        character = Character.Tim;
    }

    public void SetCharacterTum()
    {
        character = Character.Tum;
    }

    public Character GetCharacter()
    {
        return character;
    }
}
