using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoopIndicator : MonoBehaviour
{
    [SerializeField] private float _margin = .05f;

    private Transform _coop;
    private Camera _camera;
    private Image _indicatorImage;

    void Awake()
    {
        _coop = FindObjectOfType<Coop>().transform;
        _camera = Camera.main;
        _indicatorImage = GetComponent<Image>();
    }

    void Update()
    {
        var screenSize = new Vector3(_camera.scaledPixelWidth, _camera.scaledPixelHeight);
        var screenPosition = _camera.WorldToScreenPoint(_coop.position);

        // use position relative to center of screen for easy calculations
        var position = screenPosition - .5f * screenSize;
        var border = .5f * screenSize - _margin * screenSize.y * Vector3.one;

        if (Mathf.Abs(position.x) < border.x && Mathf.Abs(position.y) < border.y)
        {
            _indicatorImage.enabled = false;
            return;
        }

        float scaleToBorder;
        if (Mathf.Abs(position.x) > Mathf.Abs(position.y) * _camera.aspect)
            scaleToBorder = border.x / Mathf.Abs(position.x);
        else
            scaleToBorder = border.y / Mathf.Abs(position.y);

        Vector3 borderPosition = position * scaleToBorder;
        _indicatorImage.transform.position = borderPosition + .5f * screenSize;
        _indicatorImage.enabled = true;

        float angle = Vector3.SignedAngle(Vector3.right, borderPosition, Vector3.forward);
        _indicatorImage.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
