using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable110 : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    public bool intractable = false;
    public event Action<PointerEventData> onBegin;
    public event Action<PointerEventData> onDrag;
    public event Action<PointerEventData> onDrop;
    private Vector3 currentPosition;
    public eAlphabet value { get; private set; }
    public AlphabetToggle110 toggle { get; private set; }
    public void Init(eAlphabet value, AlphabetToggle110 toggle)
    {
        this.value = value;
        this.toggle = toggle;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        currentPosition = GetComponent<RectTransform>().position;
        onBegin?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        onDrag?.Invoke(eventData);
    }

    public void ResetPosition()
    {
        GetComponent<RectTransform>().position = currentPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        onDrop?.Invoke(eventData);
        ResetPosition();
    }
}
