using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _movementMaxSpeed = 10f;
    [SerializeField] private float _movementAcceleration = 5f;
    [SerializeField] private float _movementDrag = .1f;
    [SerializeField] private float _gravity = 20f;

    public float DebugSpeed;

    private Camera _camera;
    private CharacterController _charController;
    private Vector2 _inputVector = Vector2.zero;
    private Vector3 _horizontalMovement = Vector3.zero;
    private float _verticalMovement = 0f;

    private void Start()
    {
        _camera = Camera.main;
        _charController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ApplyAcceleration();
        ApplyDeceleration();
        ApplyGravity();
        ApplyModelRotation();
        Move();
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
        _horizontalMovement *= 1 - _movementDrag * Time.deltaTime;
    }

    private void ApplyAcceleration()
    {
        _horizontalMovement += new Vector3(_inputVector.x, 0, _inputVector.y) * _movementAcceleration * Time.deltaTime;
        if (_horizontalMovement.magnitude > _movementMaxSpeed)
            _horizontalMovement = _horizontalMovement.normalized * _movementMaxSpeed;

        DebugSpeed = _horizontalMovement.magnitude;
    }

    private void ApplyModelRotation()
    {
        if (_inputVector.magnitude == 0) return;

        var localScale = _model.localScale;
        var xMovement = (_camera.transform.rotation * _horizontalMovement).x;
        localScale.x = Mathf.Sign(xMovement) * Mathf.Abs(localScale.x);
        _model.localScale = localScale;
    }

    public void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
        if (_inputVector.magnitude > 1f)
        {
            _inputVector = _inputVector.normalized;
        }
    }
}