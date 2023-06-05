using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DeathInfo
{
    public string Title = "Internal Bleeding";
    public string Subtext = "{0} walked into a wall. Ouch.";
}

public class ChickenDeath : MonoBehaviour
{
    [SerializeField] private float _minPlayerDistance = 1f;
    [SerializeField] private float _maxPlayerDistance = 10f;
    [SerializeField] private float _maxRepulsion = 1f;
    [SerializeField] private float _maxAttraction = 1f;

    [Space]
    [SerializeField] private float _deathTime;
    [SerializeField] private LayerMask _deathTriggerLayerMask;
    [SerializeField] private GameObject _tombstonePrefab;
    [SerializeField] private RepulsionHandler _repulsion;

    public UnityEvent OnDeath;

    [SerializeField] private DeathInfo _playerTooCloseDeathInfo;
    [SerializeField] private DeathInfo _playerTooFarDeathInfo;
    [SerializeField] private DeathInfo _TooMuchRepulsionDeathInfo;
    [SerializeField] private DeathInfo _TooMuchAttractionDeathInfo;

    private GameManager _gameManager;
    private PlayerMovement _player;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        CheckDeathByPlayerDistance();
        CheckDeathByRepulsion();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_deathTriggerLayerMask.Contains(hit.gameObject.layer)) return;

        var deathInfo = hit.gameObject.GetComponent<DeathTrigger>().DeathInfo;
        Die(deathInfo);
    }

    private void CheckDeathByRepulsion()
    {
        if (_repulsion.Amount > _maxRepulsion)
            Die(_TooMuchRepulsionDeathInfo);
        if (_repulsion.Amount < -_maxAttraction)
            Die(_TooMuchAttractionDeathInfo);
    }

    private void CheckDeathByPlayerDistance()
    {
        float playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        if (playerDistance < _minPlayerDistance)
            Die(_playerTooCloseDeathInfo);
        if (playerDistance > _maxPlayerDistance)
            Die(_playerTooFarDeathInfo);
    }

    private void Die(DeathInfo deathInfo)
    {
        OnDeath?.Invoke();
        var tombstone = Instantiate(_tombstonePrefab, transform);
        tombstone.GetComponent<Tombstone>().SetChickenName(_gameManager.ChickenName);
        ChickenNames.AddDiedChicken(transform.position, _gameManager.ChickenName);

        StartCoroutine(_gameManager.OnLevelFailDelayed(deathInfo, _deathTime));
    }
}
