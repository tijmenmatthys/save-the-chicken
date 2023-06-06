using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (!_audioManager.IsPlayingSound("Music"))
            _audioManager.PlaySound("Music");
        if (_audioManager.IsPlayingSound("Ambience"))
            _audioManager.StopSound("Ambience");
    }

    public void OnLoadLevel(int levelId)
    {
        _audioManager.PlaySound("Button");
        SceneManager.LoadScene(levelId);
    }

    public void ExitGame()
    {
        _audioManager.PlaySound("Button");
        Application.Quit();
    }
}
