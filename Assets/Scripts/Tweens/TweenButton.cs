using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TweenButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _selectedScaleAmount = 1.1f;
    [SerializeField] private float _selectedScalePeriod = .3f;

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            TweenUp(this.gameObject);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            TweenDown(this.gameObject);
        }
    }

    private void TweenDown(GameObject gameObject)
    {
        transform.DOScale(1, _selectedScalePeriod).SetEase(Ease.InOutSine);
    }

    private void TweenUp(GameObject gameObject)
    {
        transform.DOScale(_selectedScaleAmount, _selectedScalePeriod).SetEase(Ease.InOutSine);
    }
}
