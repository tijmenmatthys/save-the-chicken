using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;
    void Start()
    {
        _mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.rotation = _mainCamera.transform.rotation;
    }
}
