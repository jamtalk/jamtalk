using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleElement : DoubleClickButton
{
    [SerializeField]
    private bool interactable = false;

    public Text textValue;
    public WordsData.WordSources data { get; private set; }

    private float duration = 1f;
    private Sequence seq;

    public void Init(WordsData.WordSources data)
    {
        this.data = data;
        textValue.text = data.value;
        name = data.value;
        gameObject.SetActive(true);
    }
    public void Init(char value)
    {
        textValue.text = value.ToString();
        name = value.ToString();
        gameObject.SetActive(true);
    }

    public void Play(TweenCallback callback, float size)
    {
        var min = 0.5f;
        var max = size;
        seq = DOTween.Sequence();
        Tween startTween;
        Tween endTween;

        transform.localScale = new Vector3(max, max, 1);
        startTween = transform.DOScale(min, duration);
        endTween = transform.DOScale(max, duration);

        startTween.SetEase(Ease.Linear);
        endTween.SetEase(Ease.Linear);
        seq.Append(startTween);
        seq.Append(endTween);
        seq.onComplete += callback;
        seq.Play();
    }
}
