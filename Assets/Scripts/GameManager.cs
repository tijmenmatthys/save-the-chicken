using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _enableFlutes = true;
    [SerializeField] private bool _enableDrops = true;

    [SerializeField] private float _cutsceneStartDelay = .5f;
    [SerializeField] private float _cutsceneLength = 2f;
    [SerializeField] private CinemachineVirtualCamera _deathCamera;
    [SerializeField] private CinemachineVirtualCamera _coopCamera;
    [SerializeField] private GameObject _tombstonePrefab;

    private UIManager _uiManager;

    public string ChickenName { get; private set; }

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        Time.timeScale = 0;
        ChickenName = ChickenNames.GetRandomChickenName();
        SpawnTombstones();
        _uiManager.ShowLevelStartOverlay(ChickenName);
    }

    private void SpawnTombstones()
    {
        foreach (var item in ChickenNames.DiedChicken)
        {
            var position = item.Key;
            var name = item.Value;
            var tombstone = Instantiate(_tombstonePrefab, position, Quaternion.identity);
            tombstone.GetComponent<Tombstone>().SetChickenName(name);
        }
    }

    public void OnConfirmLevelStartOverlay()
    {
        if (_enableDrops) _uiManager.ShowInstructionsDrops();
        else if (_enableFlutes) _uiManager.ShowInstructionsFlutes();
        else _uiManager.ShowInstructionsMovement();
    }

    public void OnConfirmInstructions()
    {
        StartCoroutine(PlayStartCutscene());
    }

    private IEnumerator PlayStartCutscene()
    {
        yield return new WaitForSecondsRealtime(_cutsceneStartDelay);
        _coopCamera.enabled = false;
        yield return new WaitForSecondsRealtime(_cutsceneLength);
        Time.timeScale = 1;
    }

    public void OnLevelComplete()
    {
        Time.timeScale = 0f;
        string[] diedChickenNames = ChickenNames.DiedChicken.Values.ToArray();
        _uiManager.ShowLevelCompleteOverlay(ChickenName, diedChickenNames);
    }

    public IEnumerator OnLevelFailDelayed(DeathInfo deathInfo, float deathTime)
    {
        Time.timeScale = 0f;
        _deathCamera.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(deathTime);
        _uiManager.ShowLevelFailOverlay(ChickenName, deathInfo);
    }

    public void OnStartNextLevel()
    {
        Time.timeScale = 1;
        ChickenNames.ResetDiedChicken();

        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel >= SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(nextLevel);
    }

    public void OnRestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitToMenu()
    {
        Time.timeScale = 1;
        ChickenNames.ResetDiedChicken();
        SceneManager.LoadScene(0);
    }
}
