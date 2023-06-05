using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RepulsionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _repulsionLayerMask;
    [SerializeField] private Transform _repulsionIndicator;
    [SerializeField] private Transform _attractionIndicator;
    [SerializeField] private float _indicatorSizeMultiplier = 1f;

    private Vector3 _partialSum = Vector3.zero;
    private float _partialAmount = 0;

    private List<Repulsion> _activeRepulsions = new List<Repulsion>();
    private PlayerFlute[] _playerFlutes;

    public Vector3 Sum { get; private set; } = Vector3.zero;
    public float Amount { get; private set; } = 0;

    private void Awake()
    {
        _playerFlutes = FindObjectsOfType<PlayerFlute>();
    }

    private void FixedUpdate()
    {
        RemoveDestroyedRepulsions();
        UpdateRepulsions();
        UpdateRepulsionIndicator();
    }

    private void UpdateRepulsionIndicator()
    {
        float angle = Vector3.SignedAngle(Vector3.right, Sum, Vector3.up);
        //if (Amount > 0)
        //{
        //    _repulsionIndicator.localRotation = Quaternion.Euler(0, angle, 0);
        //    _repulsionIndicator.localScale = Sum.magnitude * _indicatorSizeMultiplier * Vector3.one;
        //    _repulsionIndicator.gameObject.SetActive(true);
        //    _attractionIndicator.gameObject.SetActive(false);
        //}
        //else if (Amount < 0)
        if (Amount > 0 || Amount < 0)
        {
            _attractionIndicator.localRotation = Quaternion.Euler(0, angle, 0);
            _attractionIndicator.localScale = Sum.magnitude * _indicatorSizeMultiplier * Vector3.one;
            _attractionIndicator.gameObject.SetActive(true);
            _repulsionIndicator.gameObject.SetActive(false);
        }
        else
        {
            _attractionIndicator.gameObject.SetActive(false);
            _repulsionIndicator.gameObject.SetActive(false);
        }
    }

    private void RemoveDestroyedRepulsions()
    {
        // since OnTriggerExit is not called on destroyed objects, we need to handle this manually
        for (int i = _activeRepulsions.Count - 1; i >= 0; i--)
        {
            if (_activeRepulsions[i] == null)
                _activeRepulsions.RemoveAt(i);
        }
    }

    private void UpdateRepulsions()
    {
        _partialSum = Vector3.zero;
        _partialAmount = 0;

        foreach (var repulsion in _activeRepulsions)
            ProcessRepulsion(repulsion);
        foreach (var flute in _playerFlutes)
            ProcessFluteRepulsion(flute);

        Sum = _partialSum;
        Amount = _partialAmount;
    }

    private void ProcessRepulsion(Repulsion repulsion)
    {
        float distance = Vector3.Distance(transform.position, repulsion.transform.position);
        float amount = repulsion.Amount * (1 - distance / repulsion.Radius);
        _partialAmount += amount;

        Vector3 direction = transform.position - repulsion.transform.position;
        direction.y = 0;
        direction.Normalize();
        _partialSum += direction * amount;
    }

    private void ProcessFluteRepulsion(PlayerFlute flute)
    {
        _partialAmount += flute.RepulsionAmount;

        Vector3 direction = transform.position - flute.transform.position;
        direction.y = 0;
        direction.Normalize();
        _partialSum += direction * flute.RepulsionAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_repulsionLayerMask.Contains(other.gameObject.layer)) return;
        _activeRepulsions.Add(other.GetComponent<Repulsion>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!_repulsionLayerMask.Contains(other.gameObject.layer)) return;
        _activeRepulsions.Remove(other.GetComponent<Repulsion>());
    }
}
