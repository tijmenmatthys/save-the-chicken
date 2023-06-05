using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerFlute : MonoBehaviour
{
    [SerializeField] private bool _enable = true;
    [SerializeField] private float _ocarinaStrength = 1f;
    [SerializeField] private float _kazooStrength = 1f;

    public UnityEvent OnStartOcarina;
    public UnityEvent OnStopOcarina;
    public UnityEvent OnStartKazoo;
    public UnityEvent OnStopKazoo;

    private PlayerResources _playerResources;
    private float _ocarinaRepulsion = 0;
    private float _kazooRepulsion = 0;

    public float RepulsionAmount => _ocarinaRepulsion + _kazooRepulsion;

    private void Awake()
    {
        _playerResources = GetComponent<PlayerResources>();
        _playerResources.BreathUpdated += OnBreathUpdated;
    }

    public void OnOcarina(InputValue value)
    {
        if (!_enable) return;

        if (value.isPressed & _playerResources.Breath > 0)
            SetOcarinaActive(true);
        if (!value.isPressed)
            SetOcarinaActive(false);
    }

    public void OnKazoo(InputValue value)
    {
        if (!_enable) return;

        if (value.isPressed & _playerResources.Breath > 0)
            SetKazooActive(true);
        if (!value.isPressed)
            SetKazooActive(false);
    }

    private void OnBreathUpdated(float breath)
    {
        if (breath <= 0)
        {
            SetKazooActive(false);
            SetOcarinaActive(false);
        }
    }

    private void SetOcarinaActive(bool active)
    {
        if (active)
        {
            _ocarinaRepulsion = -_ocarinaStrength;
            OnStartOcarina?.Invoke();
        }
        else
        {
            _ocarinaRepulsion = 0;
            OnStopOcarina?.Invoke();
        }
    }

    private void SetKazooActive(bool active)
    {
        if (active)
        {
            _kazooRepulsion = _kazooStrength;
            OnStartKazoo?.Invoke();
        }
        else
        {
            _kazooRepulsion = 0;
            OnStopKazoo?.Invoke();
        }
    }
}
