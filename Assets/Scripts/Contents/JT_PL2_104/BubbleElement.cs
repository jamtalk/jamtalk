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
    public VowelWordsData data { get; private set; }
    public DigraphsWordsData digraphs { get; private set; }

    private float duration = 1f;
    private Sequence seq;
    private Vector3 defaultPosition;

    public void Init(VowelWordsData data)
    {
        this.data = data;
        textValue.text = data.key;
        name = data.key;
        gameObject.SetActive(true);
    }

    public void Init(DigraphsWordsData data)
    {
        digraphs = data;
        textValue.text = data.key;
        name = data.key;
        gameObject.SetActive(true);
    }

    public void Init(string value)
    {
        textValue.text = value.ToString();
        name = value.ToString();
        gameObject.SetActive(true);
    }

    public void SetDefaultPosition()
    {
        defaultPosition = transform.position;
        //interactable = true;
    }
    public void ResetPosition()
    {
        transform.position = defaultPosition;
    }

    public void InOut(TweenCallback callback, float minimum, float maximum)
    {
        var min = minimum;
        var max = maximum;
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

    public void OutIn(TweenCallback callback, float minimum, float maximum)
    {
        var min = minimum;
        var max = maximum;
        seq = DOTween.Sequence();
        Tween endTween;
        Tween startTween;

        transform.localScale = new Vector3(max, max, 1);
        endTween = transform.DOScale(1.2f, .5f);
        startTween = transform.DOScale(min, duration);

        endTween.SetEase(Ease.Linear);
        startTween.SetEase(Ease.Linear);
        seq.Append(endTween);
        seq.Append(startTween);
        seq.onComplete += callback;
        seq.Play();
    }
}
