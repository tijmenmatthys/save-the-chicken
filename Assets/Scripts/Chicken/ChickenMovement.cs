using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using TMPro;
using UnityEngine.Rendering.Universal;

public class ChickenMovement : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _movementMaxSpeed = 10;
    [SerializeField] private float _movementDrag = .1f;
    [SerializeField] private float _gravity = 20f;

    [Space]
    [SerializeField] private bool _randomEnable = true;
    [SerializeField] private float _randomRotation = 100;
    [SerializeField] private float _randomAcceleration = 10;

    [Space]
    [SerializeField] private bool _repulsionEnable = true;
    [SerializeField] private float _repulsionAcceleration = 20;
    [SerializeField] private float _repulsionMaxSpeed = 20;
    
    [Space]
    [SerializeField] private TextMeshProUGUI _debugRepulsionSumText;
    [SerializeField] private TextMeshProUGUI _debugRepulsionAmountText;
    [SerializeField] private TextMeshProUGUI _debugSpeedText;

    public float DebugSpeed;

    private CharacterController _charController;
    private RepulsionHandler _repulsion;
    private Vector3 _randomWalkInput = Vector3.zero;
    private Vector3 _horizontalMovement = Vector3.zero;
    private float _verticalMovement = 0f;
    private float _currentMovementAngle = 0f;
    private Camera _camera;


    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        _repulsion = GetComponentInChildren<RepulsionHandler>();
        _camera = Camera.main;
    }

    private void Update()
    {
        DrawDebugInfo();
    }

    private void FixedUpdate()
    {
        ApplyRandomWalk();
        ApplyAcceleration();
        ApplyDeceleration();
        ApplyGravity();

        SetModelRotation();
        Move();
    }

    private void ApplyRandomWalk()
    {
        _currentMovementAngle += UnityEngine.Random.Range(-_randomRotation, _randomRotation);
        float inputStrength = UnityEngine.Random.Range(0f, 1f);
        _randomWalkInput = Quaternion.AngleAxis(_currentMovementAngle, Vector3.up) * Vector3.forward * inputStrength;
    }

    private void Move()
    {
        Vector3 totalMovement = _horizontalMovement;
        totalMovement.y = _verticalMovement;
        _charController.Move(totalMovement * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_charController.isGrounded)
            // Reset vertical movement, but make sure we still collide with ground to make the jump work
            _verticalMovement = -_gravity * _charController.skinWidth;
        else
            _verticalMovement -= _gravity * Time.deltaTime;
    }

    private void ApplyDeceleration()
    {
        //float speed = _horizontalMovement.magnitude;
        //float drag = _movementDrag * speed * speed;
        //_horizontalMovement -= drag * _horizontalMovement.normalized * Time.deltaTime;

        _horizontalMovement *= 1 - _movementDrag * Time.deltaTime;
    }

    private void ApplyAcceleration()
    {
        float maxSpeed = _movementMaxSpeed;

        if (_randomEnable)
        {
            _horizontalMovement += _randomWalkInput * _randomAcceleration * Time.deltaTime;
        }
        if (_repulsionEnable)
        {
            _horizontalMovement += _repulsion.Sum * _repulsionAcceleration * Time.deltaTime;
            maxSpeed += _repulsionMaxSpeed * Mathf.Abs(_repulsion.Amount);
        }

        // limit speed
        if (_horizontalMovement.magnitude > maxSpeed)
            _horizontalMovement = _horizontalMovement.normalized * maxSpeed;

        DebugSpeed = _horizontalMovement.magnitude;
    }

    private void DrawDebugInfo()
    {
        // debug
        Debug.DrawLine(transform.position, transform.position + _repulsion.Sum * 10, Color.red);
        Debug.DrawLine(transform.position, transform.position + _randomWalkInput * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position + _horizontalMovement, Color.magenta);
        if (_debugRepulsionSumText != null)
            _debugRepulsionSumText.text = $"Repulsion Sum: {_repulsion.Sum.magnitude:0.00}";
        if (_debugRepulsionAmountText != null)
            _debugRepulsionAmountText.text = $"Repulsion Amount: {_repulsion.Amount:0.00}";
        if (_debugSpeedText != null)
            _debugSpeedText.text = $"Total Speed: {_horizontalMovement.magnitude:0.00}";
    }

    private void SetModelRotation()
    {
        if (_horizontalMovement.magnitude == 0) return;

        var localScale = _model.localScale;
        var xMovement = (_camera.transform.rotation * _horizontalMovement).x;
        localScale.x = Mathf.Sign(xMovement) * Mathf.Abs(localScale.x);
        _model.localScale = localScale;
    }

}