using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coop : MonoBehaviour
{
    [SerializeField] private LayerMask _chickenLayerMask;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_chickenLayerMask.Contains(other.gameObject.layer)) return;

        _gameManager.OnLevelComplete();
    }
}
