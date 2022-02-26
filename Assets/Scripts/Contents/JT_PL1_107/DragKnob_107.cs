using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DragKnob_107 : MonoBehaviour, IDragHandler, IDropHandler
{
    public GraphicRaycaster caster;
    public Image image;
    public RectTransform point;
    public Image line;
    private RectTransform line_rt => line.GetComponent<RectTransform>();
    public RectTransform cover;
    private string currentValue;
    public event Action onDrop;
    private bool intractable = true;
    public void Init(string value)
    {
        intractable = true;
        currentValue = value;
        image.sprite = GameManager.Instance.GetSpriteWord(value);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;

        SetLine(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!intractable)
            return;

        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var drop = results
            .Select(x => x.gameObject.GetComponent<DropSpaceShip_107>())
            .Where(x => x != null)
            .Where(x=>x.currentValue == currentValue)
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
        var v1 = position - (Vector2)line_rt.position;
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg - 90f;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var dis = Vector2.Distance(position, line_rt.position);
        var size = line_rt.sizeDelta;
        size.y = dis;
        line_rt.sizeDelta = size;
    }
    private void SetCover(Vector2 position)
    {
        intractable = false;

        var dis = Vector2.Distance(position, line_rt.position);
        var size = cover.sizeDelta;
        size.y = dis;
        var tween = cover.DOSizeDelta(size, .25f);
        tween.onComplete += () => onDrop?.Invoke();
    }
}
