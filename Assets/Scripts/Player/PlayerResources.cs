using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public Action<float> BreathUpdated;
    public Action<int> SeedsUpdated;
    public Action<int> LemonsUpdated;

    [SerializeField] private float _breathUsageRate = .5f;
    [SerializeField] private float _breathRegenerationRate = .1f;
    [SerializeField] private int _startSeeds = 3;
    [SerializeField] private int _startLemons = 3;

    private PlayerFlute _flute;
    private float _breath = 1;
    private int _seeds;
    private int _lemons;

    public float Breath => _breath;

    private void Awake()
    {
        _flute = GetComponent<PlayerFlute>();
    }

    private void Start()
    {
        _seeds = _startSeeds;
        _lemons = _startLemons;
    }

    private void Update()
    {
        UpdateBreath();
    }

    public bool UseSeed()
    {
        if (_seeds == 0) return false;

        _seeds--;
        SeedsUpdated?.Invoke(_seeds);
        return true;
    }
    public bool UseLemon()
    {
        if (_lemons == 0) return false;

        _lemons--;
        LemonsUpdated?.Invoke(_lemons);
        return true;
    }

    private void UpdateBreath()
    {
        _breath += _breathRegenerationRate * Time.deltaTime;
        if (Mathf.Abs(_flute.RepulsionAmount) > float.Epsilon)
            _breath -= _breathUsageRate * Time.deltaTime;

        _breath = Mathf.Clamp(_breath, 0, 1);
        BreathUpdated?.Invoke(Breath);
    }
}
