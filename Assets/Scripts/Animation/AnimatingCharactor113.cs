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
        Debug.LogFormat("[::{0}::] <b>Play</b>", name);
        seq = DOTween.Sequence();
        seq.Insert(0, MakeTween(legLeft,-10f));
        seq.Insert(0, MakeTween(legRight,10f));
        seq.onKill += () =>
        {
            legLeft.rotation = Quaternion.Euler(0, 0, 10);
            legRight.rotation = Quaternion.Euler(0, 0, -10);
        };
        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();
    }
    private Tween MakeTween(RectTransform rt, float angle)
    {
        var tween = rt.DORotate(new Vector3(0, 0, angle), .5f);
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
