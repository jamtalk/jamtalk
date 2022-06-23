using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public Action<WordSource> onClick;
    public event Action onDrop;
    public WordSource data { get; private set; }
    private void Awake()
    {
        pointKnob.onDrag += OnDrag;
        pointKnob.onEndDrag += OnEndDrag;
    }
    public void Init(WordSource data)
    {
        this.data = data;
        isConnected = false;
        text.text = data.value;
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
            .Where(x => x.data.value == data.value)
            .ToList();
        Debug.Log(drop.Count());
        if (drop.Count > 0)
        {
            var target = drop[0].point;
            drop[0].isConnected = true;
            drop[0].intractable = false;
            var pos = target.position;
            pos.y += 15f;
            SetLine(pos);
            SetCover(pos);
        }
        else
        {
            var size = line_rt.sizeDelta;
            size.y = 0;
            line_rt.sizeDelta = size;
        }
    }
    private void SetLine(Vector2 position)
    {
        var v1 = position - (Vector2)line_rt.position;
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg - 90f;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var dis = Vector2.Distance(FactorPos(line_rt.position), FactorPos(position));
        Debug.DrawLine(line_rt.position, position);
        var size = line_rt.sizeDelta;
        size.y = dis;
        line_rt.sizeDelta = size;
    }
    private void SetCover(Vector2 position)
    {
        intractable = false;
        isConnected = true;

        var dis = Vector2.Distance(FactorPos(line_rt.position), FactorPos(position));
        var size = cover.sizeDelta;
        size.y = dis;
        var tween = cover.DOSizeDelta(size, .25f);
        tween.onComplete += () => onDrop?.Invoke();
    }

    public Vector2 FactorPos(Vector2 pos)
    {
        var resolution = scaler.referenceResolution;
        var current = new Vector2(Screen.width, Screen.height);
        var ratio = current / resolution;
        return pos * ratio;
    }
}
