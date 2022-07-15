using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarElement506 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public CanvasScaler scaler;
    public GraphicRaycaster caster;
    public bool intractable = true;
    public Image orizinalLine;
    public Image dragLine;
    public string orizinalValue { get; private set; }
    private string value;
    public event Action<string> onDrop;
    private RectTransform line_rt => dragLine.GetComponent<RectTransform>();

    public Text text;

    public void Init(string value,Vector2 pos, float duration, Action<string>onDrop)
    {
        Show(duration);
        this.onDrop = onDrop;
        gameObject.SetActive(true);
        transform.position = pos;
        intractable = true;
        text.text = value;
        orizinalValue = this.value = value;
        ResetLine();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        List<RaycastResult> result= new List<RaycastResult>();
        caster.Raycast(eventData, result);
        var targets = result
            .Select(x => x.gameObject.GetComponent<StarElement506>())
            .Where(x => x != null)
            .ToList();

        if (targets.Count() > 0)
        {
            var target = targets.First();
            if (target.orizinalLine != dragLine)
                SetLine(target);
        }
        SetLine(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onDrop?.Invoke(value);

        line_rt.sizeDelta = new Vector2(0, line_rt.sizeDelta.y);
    }

    private void SetLine(Vector2 position)
    {
        orizinalLine.gameObject.SetActive(true);
        var v1 = position - (Vector2)line_rt.position;
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var dis = Vector2.Distance(FactorPos(line_rt.position), FactorPos(position));
        Debug.DrawLine(line_rt.position, position);
        var size = line_rt.sizeDelta;
        size.x = dis;
        line_rt.sizeDelta = size;
    }

    private Vector2 FactorPos(Vector2 pos)
    {
        var resolution = scaler.referenceResolution;
        var current = new Vector2(Screen.width, Screen.height);
        var ratio = current / resolution;
        return pos * ratio;
    }
    private void SetLine(StarElement506 target)
    {
        SetLine(target.line_rt.position);
        dragLine = target.dragLine;
        value += target.orizinalValue;
    }

    public void ResetLine()
    {
        dragLine = orizinalLine;
        value = orizinalValue;
        line_rt.sizeDelta = new Vector2(0, line_rt.sizeDelta.y);
    }
    public Sequence Show(float duration)
    {
        var seq = DOTween.Sequence();
        var rt = GetComponent<RectTransform>();

        rt.localScale = Vector3.zero;
        var scale = rt.DOScale(Vector3.one, duration);

        rt.rotation = Quaternion.Euler(0, 0, 0);
        var rot = rt.DORotate(new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360);

        seq.Insert(0f, scale);
        seq.Insert(0f, rot);
        return seq;
    }
    public Sequence Hide(float duration)
    {
        var seq = DOTween.Sequence();
        var rt = GetComponent<RectTransform>();

        rt.localScale = Vector3.one;
        var scale = rt.DOScale(Vector3.zero, duration);

        rt.rotation = Quaternion.Euler(0, 0, 0);
        var rot = rt.DORotate(new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360);

        seq.Insert(0f,scale);
        seq.Insert(0f, rot);
        return seq;
    }
}
