using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pedestrian : MonoBehaviour
{
    [SerializeField] private Transform[] _controlPoints;
    [SerializeField] private bool _isCircularMovement = false;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private Transform _sprite;

    private int _currentTarget;
    private Vector3 _currentMovement;
    private bool _isMoving = true;
    private bool _isMovingForward = true;
    private Rigidbody _rigidBody;
    private Camera _camera;

    public Vector3 Velocity =>
        (_controlPoints[_currentTarget].position - transform.position).normalized * _movementSpeed;

    private void Start()
    {
        // start by moving towards the second control point (assuming there is at least one more)
        _currentTarget = 1;
        _rigidBody = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            var targetPosition = Vector3.MoveTowards(transform.position
                , _controlPoints[_currentTarget].position, _movementSpeed * Time.deltaTime);
            _currentMovement = targetPosition - transform.position;

            //transform.position = targetPosition;
            _rigidBody.MovePosition(targetPosition);
            foreach (var point in _controlPoints)
                point.transform.position -= _currentMovement;

            if (transform.position == _controlPoints[_currentTarget].position)
                StartCoroutine(WaitAtControlPoint());
        }
        SetModelRotation();
    }

    private void SetModelRotation()
    {
        if (_currentMovement.magnitude == 0) return;

        var localScale = _sprite.localScale;
        var xMovement = (_camera.transform.rotation * _currentMovement).x;
        localScale.x = -Mathf.Sign(xMovement) * Mathf.Abs(localScale.x);
        _sprite.localScale = localScale;
    }

    private void SetNextTarget()
    {
        if (_isMovingForward)
        {
            _currentTarget++;
            if (_currentTarget == _controlPoints.Length)
            {
                if (_isCircularMovement) _currentTarget = 0;
                else
                {
                    _currentTarget = _controlPoints.Length - 2;
                    _isMovingForward = false;
                }
            }
        }
        else
        {
            _currentTarget--;
            if (_currentTarget == -1)
            {
                _currentTarget = 1;
                _isMovingForward = true;
            }
        }
    }

    private IEnumerator WaitAtControlPoint()
    {
        _isMoving = false;
        yield return new WaitForSeconds(_waitTime);
        SetNextTarget();
        _isMoving = true;
    }
}
