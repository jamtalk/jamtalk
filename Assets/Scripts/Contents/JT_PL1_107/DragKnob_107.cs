using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class DragKnob_107 : MonoBehaviour , IDragHandler, IEndDragHandler,IPointerDownHandler
{
    public DragKnobPoint107 pointKnob;
    public CanvasScaler scaler;
    public GraphicRaycaster caster;
    public Image image;
    public RectTransform point;
    public Image line;
    private RectTransform line_rt => line.GetComponent<RectTransform>();
    public RectTransform cover;
    public event Action onDrop;
    public event Action<DataSource> onClick;
    public bool intractable = true;
    public bool isConnected = false;
    public DataSource data { get; private set; }
    private void Awake()
    {
        pointKnob.onDrag += OnDrag;
        pointKnob.onEndDrag += OnEndDrag;
    }
    public void Init(DataSource value)
    {
        intractable = true;
        this.data = value;
        image.sprite = value.sprite;
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
            .Select(x => x.gameObject.GetComponent<DropSpaceShip_107>())
            .Union(results.Select(x => {
                var point = x.gameObject.GetComponent<DropKnobPoint107>();
                if (point != null)
                    return point.Parent;
                else
                    return null;
            }))
            .Where(x => x != null)
            .Where(x => x.data == data)
            .ToList();

        if (drop.Count > 0)
        {
            var target = drop[0].point;
            drop[0].isConnected = true;
            SetLine(target.position);
            SetCover(target.position);
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
        line.gameObject.SetActive(true);

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
        cover.gameObject.SetActive(true);
        intractable = false;
        isConnected = true;

        var dis = Vector2.Distance(FactorPos(line_rt.position), FactorPos(position));
        var size = cover.sizeDelta;
        size.y = dis;
        var tween = cover.DOSizeDelta(size, .25f);
        tween.onComplete += () => onDrop?.Invoke();
    }
    public void Reset()
    {
        cover.sizeDelta = Vector2.zero;
        line.gameObject.SetActive(false);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke(data);
    }
}
