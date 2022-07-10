using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Linq;

public class BaseSlotMachine<TData,TElement> : MonoBehaviour where TElement:SlotMachineElement<TData>
{
    public GameObject original;
    public RectTransform content;
    protected virtual float duration => 5;
    private List<RectTransform> elements = new List<RectTransform>();
    public void Sloting(TData[] datas, TweenCallback onDone)
    {
        StartCoroutine(PlaySloting(datas, onDone));
    }
    private void CreateElements(int count)
    {
        if (count <= 0)
            return;
        else
        {
            for(int i = 0;i < count; i++)
            {
                var element = Instantiate(original, content).GetComponent<RectTransform>();
                SetAnchor(element);
                elements.Add(element);
            }
        }
    }
    public void Clear()
    {
        for (int i = 0; i < elements.Count; i++)
            Destroy(elements[i].gameObject);
        elements.Clear();
    }
    private void SetAnchor(RectTransform element)
    {
        element.anchorMin = new Vector2(0f, 1f);
        element.anchorMax = new Vector2(1f, 1f);
        element.pivot = new Vector2(.5f, 0f);
        element.anchoredPosition = Vector2.zero;
        ElementSizing(element);
    }
    protected virtual void ElementSizing(RectTransform element)
    {
        element.sizeDelta = new Vector2(0f, content.rect.height / 2f);
        element.anchoredPosition = Vector2.zero;
    }
    private IEnumerator PlaySloting(TData[] datas, TweenCallback onDone)
    {
        yield return new WaitForEndOfFrame();
        CreateElements(3);
        var seq = GetSlotingSequnce(datas);
        seq.SetEase(Ease.InOutCubic);

        seq.onComplete += onDone;

        seq.Play();
    }
    protected virtual Sequence GetSlotingSequnce(TData[] datas)
    {
        var seq = DOTween.Sequence();
        var duration = this.duration / datas.Length;
        for (int i = 0; i < datas.Length - 1; i++)
        {
            var target = elements[i % 3];
            var tween = GetSlotingTween(target, datas[i], GetLastPosY(), duration);
            tween.onComplete += () =>
            {
                SetAnchor(target);
            };
            seq.Append(tween);
        }
        var lastTween = GetSlotingTween(elements[(datas.Length - 1) % 3], datas.Last(), GetMiddlePosY(), duration/2f);
        seq.Append(lastTween);
        return seq;
    }
    protected virtual Tween GetSlotingTween(RectTransform target, TData data,float targetPos, float duration)
    {
        var tween = target.DOAnchorPosY(targetPos, duration);
        tween.SetEase(Ease.Linear);
        tween.onPlay += () =>
        {
            target.GetComponent<TElement>().Init(data);
        };
        return tween;
    }
    protected virtual float GetLastPosY() => content.rect.height * -1.5f;
    protected virtual float GetMiddlePosY() => GetLastPosY() / 2f;
}
public abstract class SlotMachineElement<TData> : MonoBehaviour
{
    public TData data { get; protected set; }
    public abstract void Init(TData data);
}
