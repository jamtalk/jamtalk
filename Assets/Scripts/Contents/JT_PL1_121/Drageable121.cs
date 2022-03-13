using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drageable121 : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rt => GetComponent<RectTransform>();
    private int value;
    private Vector2 currentPos;
    public event Action onCorrect;
    public event Action onIncorrect;
    public void Init(int value)
    {
        this.value = value;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = rt.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var caster = FindObjectOfType<GraphicRaycaster>();
        var result = new List<RaycastResult>();
        caster.Raycast(eventData, result);
        var components = result
            .Select(x => x.gameObject.GetComponent<SiteWord121>())
            .Where(x=>x!=null);
        if (components.Count() > 0)
        {
            var target = components.First();
            if (target.GetInstanceID() == value)
                CorrectPosition();
            else
                IncorrectPosition();
        }
        else
            IncorrectPosition();
    }

    private void CorrectPosition()
    {
        rt.anchoredPosition = Vector2.zero;
        onCorrect?.Invoke();
    }
    private void IncorrectPosition()
    {
        var seq = DOTween.Sequence();
        var scaleTween = rt.DOScale(0f, 1f);
        var rotTween = rt.DORotate(new Vector3(0, 0, 270f),1f);
        var resetScaleTween = rt.DOScale(1f, .25f);
        var resetRotTween = rt.DORotate(Vector3.zero, .25f);
        resetScaleTween.onPlay += () => rt.anchoredPosition = currentPos;
        seq.Insert(0f, scaleTween);
        seq.Insert(0f, rotTween);
        seq.Insert(1f, resetScaleTween);
        seq.Insert(1f, resetRotTween);
        seq.onComplete += () =>
        {
            onIncorrect?.Invoke();
        };
        seq.Play();
    }
}
