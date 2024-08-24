using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class LevelController : MonoBehaviour
{        
    //Parameters
    bool isPlaying = false;
    bool hasWon = false;
    bool hasLost = false;
    bool speedup = false;
    int starCount = 0;
    const int MAX_STAR_COUNT = 3;
    float timer = 0f;
    [SerializeField] int blockCount;
    [SerializeField] float timeLimit;
    [Range(0, 1)] [SerializeField] float volume = 0.5f;
    int blocksLeft;    

    //Cached Component References
    [SerializeField] Tim timPrefab;
    [SerializeField] Tim tumPrefab;
    [SerializeField] Transform characterInitialPos;

    //Cached Object References
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private TilePlacer tilePlacer;
    Tim characterInstance;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private MovingPlatform[] movingPlatforms;
    List<Coroutine> platformCoroutines = new List<Coroutine>();
    [SerializeField] private Collider2D foreGroundTilemap;
    [SerializeField] GameObject winLabel;
    [SerializeField] GameObject loseLabel;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip loseSFX;
    [SerializeField] TextMeshProUGUI blocksText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject[] starImages;
    [SerializeField] CinemachineVirtualCamera playingCam;
    [SerializeField] CinemachineStateDrivenCamera drivenCamera;        


    void Start()
    {
        //FindObjectReferences();
        InstantiateCharacter();
        AssignPlayingAndDrivenCam();
        IdentifyGameMode();
        ResetWinAndLoseScreens();
    }

    private void FindObjectReferences()
    {
        tilePlacer = FindObjectOfType<TilePlacer>();
        //gameMode = FindObjectOfType<GameMode>();
        //foreGroundTilemap = GameObject.FindGameObjectWithTag("Drawing Tilemap").GetComponent<Collider2D>();
        levelLoader = FindObjectOfType<LevelLoader>();
        //stars = GameObject.FindGameObjectsWithTag("Star");
        movingPlatforms = FindObjectsOfType<MovingPlatform>();             
    }

    private void InstantiateCharacter()
    {
        //never do the following the way it has been done
        characterInstance = Instantiate(GameMode.instance.p_Character == GameMode.Character.Tim ? timPrefab : tumPrefab, characterInitialPos.position, Quaternion.identity);
    }

    private void AssignPlayingAndDrivenCam()
    {
        if (playingCam != null && drivenCamera != null)
        {
            playingCam.Follow = characterInstance.transform;
            drivenCamera.m_AnimatedTarget = characterInstance.gameObject.GetComponent<Animator>();
        }
    }

    private void IdentifyGameMode()
    {
        if (GameMode.instance.Mode == GameMode.Modes.Alternate)
        {
            blocksText.gameObject.SetActive(true);
            blocksLeft = blockCount;
            UpdateBlockText();
            if (timeText != null)
            {
                UpdateTimerText();
            }
            foreach (GameObject star in stars)
            {
                star.SetActive(false);
            }
        }
        else
        {
            blocksText.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
        }
    }

    private void ResetWinAndLoseScreens()
    {
        foreach (GameObject starImage in starImages)
        {
            starImage.SetActive(false);
        }
        winLabel.SetActive(false);
        loseLabel.SetActive(false);
    }

    private void UpdateBlockText()
    {
        if (blocksText != null)
        {
            blocksText.text = "Blocks : " + blocksLeft.ToString();
            if(blocksLeft < 0)
            {
                blocksText.color = Color.red;
            }
            else
            {
                blocksText.color = Color.white;
            }
        }
    }

    private void Update()
    {
        AlternateModeTimerUpdate();
    }

    private void AlternateModeTimerUpdate()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timeText != null)
            {
                UpdateTimerText();
            }
        }
    }

    private void UpdateTimerText()
    {
        timeText.text = "Time : " + timer.ToString("F") + "/" + timeLimit.ToString("F");
        if (timeText.color != Color.red && timer > timeLimit)
        {
            timeText.color = Color.red;
        }
        else if (timeText.color != Color.white && timer < timeLimit)
        {
            timeText.color = Color.white;
        }
    }

    public void PlayLevel()
    {
        if (!isPlaying)
        {
            tilePlacer.gameObject.SetActive(false);
            isPlaying = true;
            characterInstance.SetPlaying(isPlaying);
            foreGroundTilemap.enabled = false;          //The level's drawable tilemap sometimes has botched collision, this is an attempt at a fix
            foreGroundTilemap.enabled = true;
            timer = 0f;

            foreach(MovingPlatform platform in movingPlatforms)
            {
                platform.SetIsPlaying(true);
                platformCoroutines.Add(StartCoroutine(platform.MovePlatform()));                
            }
            if(speedup)
            {
                Time.timeScale = 2f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void DecreaseBlockCount()
    {
        blocksLeft--;
        UpdateBlockText();
    }

    public void IncreaseBlockCount()
    {
        blocksLeft++;
        UpdateBlockText();
    }

    public void StarGet()
    {
        if(starCount < MAX_STAR_COUNT)
        {
            starImages[starCount].SetActive(true);
            starCount++;            
        }
    }

    public void Win()
    {
        Time.timeScale = 1f;
        hasWon = true;
        isPlaying = false;
        if ( GameMode.instance.Mode == GameMode.Modes.Alternate)
        {
            starImages[0].SetActive(true);
            starCount = 1;
            if (blocksLeft >= 0)
            {
                starImages[1].SetActive(true);
                starCount++;
            }
            if(timer < timeLimit)
            {
                starImages[2].SetActive(true);
                starCount++;
            }
        }

        ResetMovingPlatforms(false);

        //var gameDataController = FindObjectOfType<GameDataController>();
        if(GameDataController.instance != null)
        {            
            GameDataController.instance.UpdateData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, starCount);            
        }

        winLabel.SetActive(true);
        AudioSource.PlayClipAtPoint(winSFX, Camera.main.transform.position,volume);
    }

    public void Lose()
    {
        Time.timeScale = 1f;
        hasLost = true;
        isPlaying = false;
        loseLabel.SetActive(true);
        AudioSource.PlayClipAtPoint(loseSFX, Camera.main.transform.position,volume);
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        Destroy(characterInstance.gameObject);
        InstantiateCharacter();
        AssignPlayingAndDrivenCam();
        isPlaying = false;
        timer = 0f;
        if (timeText != null)
        {
            UpdateTimerText();
        }

        if(GameMode.instance.Mode == GameMode.Modes.Normal)
        {
            ResetStars();
        }
        ResetMovingPlatforms();

        tilePlacer.gameObject.SetActive(true);
    }

    public void SetSpeedup(bool value)
    {
        speedup = value;
        if(isPlaying && speedup)
        {
            Time.timeScale = 2f;
        }
        else if(isPlaying && !speedup)
        {
            Time.timeScale = 1f;
        }
    }

    private void ResetStars()
    {
        foreach (GameObject star in stars)
        {
            if (!star.activeInHierarchy)
            {
                star.SetActive(true);
            }
        }

        foreach (GameObject starImage in starImages)
        {
            starImage.SetActive(false);
        }
        starCount = 0;
    }

    private void ResetMovingPlatforms(bool toReset = true)
    {
        for (int i = 0; i < movingPlatforms.Length; i++)       //platformCoroutines.Count *should* be equal to movingPlatforms.Count...
        {
            if (platformCoroutines.Count > 0)
            {
                StopCoroutine(platformCoroutines[i]);
            }

            if (toReset)
            {
                movingPlatforms[i].ResetPlatformPosition();
            }
        }

        platformCoroutines.Clear(); //Very Important, otherwise reseting and replaying the level causes new coroutines to added to the list which results
                                    //different Counts for the two lists used in the for loop and also retains redundant coroutines in the list 
                                    //clogging up the memory
    }

    public void SetUnlockTextActive()
    {
        var texts = winLabel.GetComponentsInChildren<TextMeshProUGUI>(true);
        
        foreach(TextMeshProUGUI text in texts)
        {
            Debug.Log(text);
            text.gameObject.SetActive(true);
        }
    }
}
