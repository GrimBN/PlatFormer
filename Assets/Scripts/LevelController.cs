using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
//using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    //TODO : Playtest alternate mode to identify workable values for block count and time
    //Parameters
    bool isPlaying = false;
    bool hasWon = false;
    bool hasLost = false;
    int starCount = 0;
    const int MAX_STAR_COUNT = 3;
    float timer = 0f;
    [SerializeField] int blockCount;
    [SerializeField] float timeLimit;
    int blocksLeft;    

    //Cached Component References
    [SerializeField] Tim timPrefab;
    [SerializeField] Tim tumPrefab;
    [SerializeField] Vector3 characterInitialPos;

    //Cached Object References
    LevelLoader levelLoader;
    TilePlacer tilePlacer;
    Tim characterInstance;
    GameObject[] stars;
    MovingPlatform[] movingPlatforms;
    List<Coroutine> platformCoroutines = new List<Coroutine>();
    GameMode gameMode;
    Collider2D foreGroundTilemap;
    [SerializeField] GameObject winLabel;
    [SerializeField] GameObject loseLabel;
    [SerializeField] TextMeshProUGUI blocksText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject[] starImages;
    [SerializeField] CinemachineVirtualCamera playingCam;
    [SerializeField] CinemachineStateDrivenCamera drivenCamera;        


    void Start()
    {
        FindObjectReferences();
        InstantiateCharacter();
        AssignPlayingAndDrivenCam();
        IdentifyGameMode();
        ResetWinAndLoseScreens();
    }

    private void FindObjectReferences()
    {
        tilePlacer = FindObjectOfType<TilePlacer>();
        /*characterInstance = FindObjectOfType<Tim>();
        characterInitialPos = characterInstance.transform.position;*/
        gameMode = FindObjectOfType<GameMode>();
        foreGroundTilemap = GameObject.FindGameObjectWithTag("Drawing Tilemap").GetComponent<Collider2D>();
        levelLoader = FindObjectOfType<LevelLoader>();
        stars = GameObject.FindGameObjectsWithTag("Star");
        movingPlatforms = GameObject.FindObjectsOfType<MovingPlatform>();             
    }

    private void InstantiateCharacter()
    {
        //never do the following the way it has been done
        characterInstance = Instantiate(gameMode != null ? gameMode.GetCharacter() == GameMode.Character.Tim ? timPrefab : tumPrefab : timPrefab, characterInitialPos, Quaternion.identity);
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
        if (gameMode != null && gameMode.GetMode() == GameMode.Modes.Alternate)
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
        /*if(Input.GetButtonDown("Jump"))
        {
            PlayLevel();
        }
        else if(Input.GetButtonDown("Cancel"))
        {
            ReloadLevel();
        }*/
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
            /*if(timer != null)
            {
                Destroy(timer);
            }

            timer = new Timer();*/

            foreach(MovingPlatform platform in movingPlatforms)
            {
                platform.SetIsPlaying(true);
                platformCoroutines.Add(StartCoroutine(platform.MovePlatform()));                
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
        hasWon = true;
        isPlaying = false;
        if ( gameMode != null && gameMode.GetMode() == GameMode.Modes.Alternate)
        {
            starImages[0].SetActive(true);
            if (blocksLeft >= 0)
            {
                starImages[1].SetActive(true);
            }
            if(timer < timeLimit)
            {
                starImages[2].SetActive(true);
            }
        }

        ResetMovingPlatforms(false);

        var gameDataController = FindObjectOfType<GameDataController>();
        if(gameDataController != null)
        {
            //if (levelID > 0) gameDataController.UpdateData(levelID, starCount, gameMode);
            gameDataController.UpdateData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, starCount, gameMode);            
        }

        winLabel.SetActive(true);        
    }

    public void Lose()
    {
        hasLost = true;
        isPlaying = false;
        loseLabel.SetActive(true);
    }

    public void ReloadLevel()
    {
        Destroy(characterInstance.gameObject);
        InstantiateCharacter();
        AssignPlayingAndDrivenCam();
        isPlaying = false;
        timer = 0f;
        if (timeText != null)
        {
            UpdateTimerText();
        }

        if(gameMode.GetMode() == GameMode.Modes.Normal)
        {
            ResetStars();
        }
        ResetMovingPlatforms();

        tilePlacer.gameObject.SetActive(true);
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
}
