using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image _breathSlider;
    [SerializeField] private TextMeshProUGUI _seedsText;
    [SerializeField] private TextMeshProUGUI _lemonsText;

    void Start()
    {
        var playerResources = FindObjectOfType<PlayerResources>();
        playerResources.BreathUpdated += OnBreathUpdated;
        playerResources.SeedsUpdated += OnSeedsUpdated;
        playerResources.LemonsUpdated += OnLemonsUpdated;
    }

    private void OnLemonsUpdated(int lemons)
    {
        _lemonsText.text = lemons.ToString();
    }

    private void OnSeedsUpdated(int seeds)
    {
        _seedsText.text = seeds.ToString();
    }

    private void OnBreathUpdated(float breath)
    {
        _breathSlider.fillAmount = breath;
    }
}
