using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int currentSceneIndex;    

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;        
    }

    public void ReloadScene()
    {                
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {        
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadGivenScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("Game Over");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
