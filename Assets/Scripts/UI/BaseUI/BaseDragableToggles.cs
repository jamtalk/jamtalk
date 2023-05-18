using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class BaseDropable<TValue> : MonoBehaviour
{
    public event Action onDroped;
    public BaseDragableToggle<TValue> toggle { get; private set; }
    public TValue value { get; private set; }
    public void Drop() => onDroped?.Invoke();
    public virtual void Init(TValue value, BaseDragableToggle<TValue> toggle)
    {
        this.value = value;
        this.toggle = toggle;
    }
}


public class BaseDragable<TValue> : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool intractable = false;
    public event Action<PointerEventData> onBegin;
    public event Action<PointerEventData> onDrag;
    public event Action<PointerEventData> onDrop;
    private Vector3 currentPosition;
    public TValue value { get; private set; }
    public BaseDragableToggle<TValue> toggle { get; private set; }
    public virtual void Init(TValue value, BaseDragableToggle<TValue> toggle)
    {
        this.value = value;
        this.toggle = toggle;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        currentPosition = GetComponent<RectTransform>().position;
        onBegin?.Invoke(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        onDrag?.Invoke(eventData);
    }

    public virtual void ResetPosition()
    {
        GetComponent<RectTransform>().position = currentPosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        onDrop?.Invoke(eventData);
        ResetPosition();
    }
}


public abstract class BaseDragableToggle<TValue> : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image[] images;
    [SerializeField]
    public BaseDropable<TValue> drop;
    public BaseDragable<TValue> drag => throwElement.GetComponent<BaseDragable<TValue>>();

    public RectTransform throwElement;
    public AudioSinglePlayer audioPlayer;
    public AudioClip clipOn;
    public bool isOn
    {
        get => toggle.isOn;
        set
        {
            toggle.isOn = value;
            audioPlayer.Play(clipOn);
            onOn?.Invoke();
        }
    }
    public event Action onOn;
    public TValue value;
    protected virtual void Awake()
    {
        drop.onDroped += () => isOn = true;
    }
    public virtual void Init(TValue value)
    {
        this.value = value;
        var sprite = GetSprite();

        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = sprite;
            images[i].preserveAspect = true;
        }
        background.sprite = GetBackground();
        drag.Init(value, this);
        drop.Init(value, this);
    }
    protected abstract Sprite GetSprite();
    protected abstract Sprite GetBackground();
}