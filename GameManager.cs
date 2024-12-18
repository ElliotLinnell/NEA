using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static string targetSceneName;

    public static void StartLoading(string sceneName)
    {
        targetSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene"); 
    }

    public void LoadScene(string sceneName)
    {
        StartLoading(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        LoadScene("MainScene");
    }

    public void LoadGame()
    {
        LoadScene("MainScene");
    }

    public void Settings()
    {
        LoadScene("Settings");
    }

    public void Quit()
    {
        QuitGame();
    }

    public static string GetTargetSceneName()
    {
        return targetSceneName;
    }
    public void loadSettingsFromInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings();
        }
    }
}