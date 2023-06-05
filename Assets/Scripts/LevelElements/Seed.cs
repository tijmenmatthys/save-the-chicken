using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Seed: MonoBehaviour
{
    [SerializeField] private LayerMask _chickenLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (!_chickenLayerMask.Contains(other.gameObject.layer)) return;

        Destroy(gameObject);
    }
}
