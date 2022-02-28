using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimatingDino : MonoBehaviour, IStopalbeAnimation
{
    public RectTransform head;
    public RectTransform tail;
    public RectTransform leg_back_left;
    public RectTransform leg_back_right;
    public RectTransform leg_front_left;
    public RectTransform leg_front_right;
    Sequence seq;
    private void Awake()
    {
        Play();
    }

    public void Play()
    {
        seq = DOTween.Sequence();
        seq.Insert(0, MakeTween(head));
        seq.Insert(0, MakeTween(tail));
        seq.Insert(0, MakeTween(leg_back_left));
        seq.Insert(0, MakeTween(leg_back_right));
        seq.Insert(0, MakeTween(leg_front_left));
        seq.Insert(0, MakeTween(leg_front_right));
        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();
    }

    private Tween MakeTween(RectTransform rt)
    {
        var tween = rt.DORotate(new Vector3(0, 0, 0), 1f);
        tween.SetEase(Ease.Linear);
        return tween;
    }

    public void Stop()
    {
        if(seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
