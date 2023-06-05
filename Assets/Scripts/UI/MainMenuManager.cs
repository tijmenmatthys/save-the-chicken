using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnLoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
