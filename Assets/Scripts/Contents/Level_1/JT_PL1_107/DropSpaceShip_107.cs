using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSpaceShip_107 : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public DropKnobPoint107 pointKnob;
    public CanvasScaler scaler;
    public bool isConnected = false;
    public bool intractable = true;
    public RectTransform cover;
    public GraphicRaycaster caster;
    public Text text;
    public RectTransform point;
    public RectTransform line_rt;
    public Image lineImage => line_rt.GetComponent<Image>();
    public UnityEvent<ResourceWordsElement> onClick;
    public UnityEvent onDrop;
    public UnityEvent onIncorrectDrop;
    public ResourceWordsElement data { get; private set; }
    private void Awake()
    {
        pointKnob.onDrag += OnDrag;
        pointKnob.onEndDrag += OnEndDrag;

        lineImage.type = Image.Type.Filled;
        lineImage.fillMethod = Image.FillMethod.Vertical;
        lineImage.fillAmount = 0;
    }
    public void Init(ResourceWordsElement data)
    {
        this.data = data;
        isConnected = false;
        text.text = data.key;
    }

    public void SetGuideLine(float delay, DragKnob_107 target)
    {
        var targets = target.point;
        target.isConnected = true;
        target.intractable = false;
        var pos = Camera.main.WorldToScreenPoint(targets.position);
        pos.y += 15f;
        SetLine(pos);

        lineImage.DOFillAmount(1, 2f).SetDelay(delay);
    }

    public void SetGuideCover(DragKnob_107 target)
    {
        var targets = target.point;
        target.isConnected = true;
        target.intractable = false;
        var pos = Camera.main.WorldToScreenPoint(targets.position);
        pos.y += 15f;
        SetCover(pos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke(data);
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        SetLine(eventData.position);
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;

        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var drop = results
            .Select(x => x.gameObject.GetComponent<DragKnob_107>())
            .Union(results.Select(x => {
                var point = x.gameObject.GetComponent<DragKnobPoint107>();
                if (point != null)
                    return point.Parent;
                else
                    return null;
            }))
            .Where(x => x != null)
            .Where(x => x.data.key == data.key)
            .ToList();

        if (drop.Count > 0)
        {
            var target = drop[0].point;
            drop[0].isConnected = true;
            drop[0].intractable = false;
            var pos = Camera.main.WorldToScreenPoint(target.position);
            pos.y += 15f;
            SetLine(pos);
            SetCover(pos);
        }
        else
        {
            onIncorrectDrop?.Invoke();
            var size = line_rt.sizeDelta;
            size.y = 0;
            line_rt.sizeDelta = size;
        }
    }
    private void SetLine(Vector2 position)
    {
        line_rt.gameObject.SetActive(true);

        var v1 = position - (Vector2)Camera.main.WorldToScreenPoint(line_rt.position);
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg - 90f;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var size = line_rt.sizeDelta;
        size.y = v1.magnitude;
        line_rt.sizeDelta = size;
    }
    private void SetCover(Vector2 position)
    {
        cover.gameObject.SetActive(true);

        intractable = false;
        isConnected = true;

        var v1 = position - (Vector2)Camera.main.WorldToScreenPoint(line_rt.position);
        var size = cover.sizeDelta;
        size.y = v1.magnitude;
        var tween = cover.DOSizeDelta(size, .25f);
        tween.onComplete += () => onDrop?.Invoke();
    }
    public void Reset()
    {
        cover.sizeDelta = Vector2.zero;
        line_rt.gameObject.SetActive(false);
        cover.gameObject.SetActive(false);
        intractable = true;
        isConnected = false;
    }
    public Vector2 FactorPos(Vector2 pos)
    {
        var resolution = scaler.referenceResolution;
        var current = new Vector2(Screen.width, Screen.height);
        var ratio = current / resolution;
        return pos * ratio;
    }
}
