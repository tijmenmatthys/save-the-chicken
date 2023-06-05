using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Tween : MonoBehaviour
{
    [SerializeField] private bool _enableScaleTween = true;
    [SerializeField] private float _scaleAmount = 1.1f;
    [SerializeField] private float _scalePeriod = 1f;

    [Space]
    [SerializeField] private bool _enableRotationTween = true;
    [SerializeField] private float _rotationAmount = 30f;
    [SerializeField] private float _rotationPeriod = 1f;
    [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

    private bool _hasStarted = false;

    void OnEnable()
    {
        if (_hasStarted) return;

        if (_enableScaleTween) TweenScale();
        if (_enableRotationTween) TweenRotation();
        _hasStarted = true;
    }

    private void TweenScale()
    {
        float startScale = transform.localScale.x;
        transform.localScale = startScale / _scaleAmount * Vector3.one;

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(startScale * _scaleAmount, _scalePeriod).SetEase(Ease.InOutSine))
            .Append(transform.DOScale(startScale / _scaleAmount, _scalePeriod).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Restart)
            .SetUpdate(true); // use unscaled time to play tweens even when game is paused
    }

    private void TweenRotation()
    {
        var startRotation = Quaternion.AngleAxis(-_rotationAmount, _rotationAxis) * transform.localRotation;
        var endRotation = Quaternion.AngleAxis(_rotationAmount, _rotationAxis) * transform.localRotation;
        transform.localRotation = startRotation;

        Sequence sequence = DOTween.Sequence()
            .Append(transform.DORotate(endRotation.eulerAngles, _rotationPeriod).SetEase(Ease.InOutSine))
            .Append(transform.DORotate(startRotation.eulerAngles, _rotationPeriod).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Restart)
            .SetUpdate(true);
    }
}
