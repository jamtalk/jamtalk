using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Breather : AnimationScript
{
    private enum eDirection
    {
        Up,
        Down
    }
    [SerializeField]
    private eDirection direction;
    [SerializeField]
    [Range(.1f, 1f)]
    private float range;
    private Sequence seq;
    public override float duration => base.duration / 2f;
    public override void Play()
    {
        var min = 1 - range;
        var max = 1 + range;
        seq = DOTween.Sequence();
        Tween startTween;
        Tween endTween;
        if (direction == eDirection.Down)
        {
            transform.localScale = new Vector3(max, max, 1);
            startTween = transform.DOScale(min, duration);
            endTween = transform.DOScale(max, duration);
        }
        else
        {
            transform.localScale = new Vector3(min, min, 1);
            startTween = transform.DOScale(max, duration);
            endTween = transform.DOScale(min, duration);
        }
        startTween.SetEase(Ease.Linear);
        endTween.SetEase(Ease.Linear);
        seq.Append(startTween);
        seq.Append(endTween);
        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();
    }

    public override void Stop()
    {
        if(seq!=null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
