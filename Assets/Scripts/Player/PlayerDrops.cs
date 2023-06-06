using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDrops : MonoBehaviour
{
    [SerializeField] private bool _enable = true;
    [SerializeField] private GameObject _seedPrefab;
    [SerializeField] private GameObject _lemonPrefab;

    private PlayerResources _resources;
    private AudioManager _audioManager;

    private void Awake()
    {
        _resources = GetComponent<PlayerResources>();
        _audioManager = FindObjectOfType<AudioManager>();
    }


    public void OnDropSeed(InputValue value)
    {
        if (!_enable) return;

        if (_resources.UseSeed())
        {
            _audioManager.PlaySound("Button");
            Instantiate(_seedPrefab, transform.position, Quaternion.identity);
        }
    }

    public void OnDropLemon(InputValue value)
    {
        if (!_enable) return;

        if (_resources.UseLemon())
        {
            _audioManager.PlaySound("Button");
            Instantiate(_lemonPrefab, transform.position, Quaternion.identity);
        }
    }
}
