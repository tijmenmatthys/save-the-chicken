using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Repulsion : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _amount = 1f;

    private SphereCollider _collider;

    public float Amount => _amount;
    public float Radius => _radius;

    private void OnValidate()
    {
        _collider ??= GetComponent<SphereCollider>();
        _collider.radius = Radius;
    }

    private void OnDrawGizmos()
    {
        var color = _amount > 0 ? Color.red : Color.green;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
