using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimatingCharactor113 : MonoBehaviour,IStopalbeAnimation
{
    public RectTransform legLeft;
    public RectTransform legRight;
    Sequence seq;
    public void Play()
    {
        seq = DOTween.Sequence();
        seq.Insert(0, MakeTween(legLeft));
        seq.Insert(0, MakeTween(legRight));
    }
    private Tween MakeTween(RectTransform rt)
    {
        var tween = rt.DORotate(new Vector3(0, 0, 0), 1f);
        tween.SetEase(Ease.Linear);
        return tween;
    }

    public void Stop()
    {
        if(seq!= null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
