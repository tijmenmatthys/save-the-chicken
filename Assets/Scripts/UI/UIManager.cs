using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _levelCompleteOverlay;
    [SerializeField] private GameObject _levelCompleteButton;
    [SerializeField] private TextMeshProUGUI _levelCompleteTitleText;
    [SerializeField] private TextMeshProUGUI _levelCompleteSubText;
    [SerializeField] private GameObject _levelFailOverlay;
    [SerializeField] private GameObject _levelFailButton;
    [SerializeField] private TextMeshProUGUI _levelFailTitleText;
    [SerializeField] private TextMeshProUGUI _levelFailSubText;
    [SerializeField] private GameObject _levelStartOverlay;
    [SerializeField] private GameObject _levelStartButton;
    [SerializeField] private TextMeshProUGUI _levelStartTitleText;
    [SerializeField] private TextMeshProUGUI _levelStartSubText;
    [SerializeField] private GameObject _instructionsMovement;
    [SerializeField] private GameObject _instructionsMovementButton;
    [SerializeField] private GameObject _instructionsFlutes;
    [SerializeField] private GameObject _instructionsFlutesButton;
    [SerializeField] private GameObject _instructionsDrops;
    [SerializeField] private GameObject _instructionsDropsButton;

    private EventSystem _eventSystem;
    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public void ShowLevelCompleteOverlay(string chickenName, string[] diedChickenNames)
    {
        _levelCompleteOverlay.SetActive(true);
        _levelCompleteTitleText.text = String.Format(_levelCompleteTitleText.text, chickenName);
        _levelCompleteSubText.text = String.Format(_levelCompleteSubText.text, chickenName, string.Join(", ", diedChickenNames.Distinct()));
        _eventSystem.SetSelectedGameObject(_levelCompleteButton);
    }

    public void ShowLevelFailOverlay(string chickenName, DeathInfo deathInfo)
    {
        _levelFailOverlay.SetActive(true);
        _levelFailTitleText.text = $"{chickenName} died by {deathInfo.Title}";
        _levelFailSubText.text = String.Format(deathInfo.Subtext, chickenName);
        _eventSystem.SetSelectedGameObject(_levelFailButton);
    }
    public void ShowLevelStartOverlay(string chickenName)
    {
        _levelStartOverlay.SetActive(true);
        _levelStartTitleText.text = String.Format(_levelStartTitleText.text, chickenName);
        _levelStartSubText.text = String.Format(_levelStartSubText.text, chickenName);
        _eventSystem.SetSelectedGameObject(_levelStartButton);
    }

    public void ShowInstructionsDrops()
    {
        _instructionsDrops.SetActive(true);
        _eventSystem.SetSelectedGameObject(_instructionsDropsButton);
    }

    public void ShowInstructionsFlutes()
    {
        _instructionsFlutes.SetActive(true);
        _eventSystem.SetSelectedGameObject(_instructionsFlutesButton);
    }

    public void ShowInstructionsMovement()
    {
        _instructionsMovement.SetActive(true);
        _eventSystem.SetSelectedGameObject(_instructionsMovementButton);
    }

}
