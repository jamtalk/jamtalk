using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimationElement : PoolingElement
{
    public RectTransform[] rects;
    public RectTransform[] rightRts;
    public RectTransform[] leftRts;
    public int positive = 5;
    public int negative = -5;
    public Button button;
    public bool isTwoWay = false;
    Sequence seq;

    private void Awake()
    {
        if (!isTwoWay)
            Play();
        else
            PlayTwoWay();
    }

    public void Play()
    {
        seq = DOTween.Sequence();

        foreach (var item in rects)
        {
            seq.Insert(0, MakeTween(item, positive));
            seq.Insert(0, MakeTween(item, negative));
        }

        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();
    }

    public void PlayTwoWay()
    {
        seq = DOTween.Sequence();

        foreach (var item in leftRts)
        {
            seq.Insert(0, MakeTween(item, positive));
            seq.Insert(1, MakeTween(item, negative));
        }
        foreach (var item in rightRts)
        {
            seq.Insert(0, MakeTween(item, negative));
            seq.Insert(1, MakeTween(item, positive));
        }

        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();

    }
    private Tween MakeTween(RectTransform rt, float z = 0)
    {
        var tween = rt.DORotate(new Vector3(0, 0, z), 1f);
        tween.SetEase(Ease.Linear);
        return tween;
    }

    public void Stop()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
