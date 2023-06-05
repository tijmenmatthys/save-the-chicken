using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopCamera : MonoBehaviour
{
    private void Start()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        var camera = GetComponent<CinemachineVirtualCamera>();
        var coop = FindObjectOfType<Coop>();
        camera.Follow = coop.transform;
    }
}
