using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnLevelWasLoaded(int level)
    {
        PlayerData.TrySetCurrentLevel(level);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void LoadNewGame()
    {
        SceneManager.LoadSceneAsync("Tutorial");
        PlayerData.SetCurrentLevel(2);
    }

    public void LoadLastLevel()
    {
        SceneManager.LoadSceneAsync(PlayerData.CurrentLevel);
    }
}
