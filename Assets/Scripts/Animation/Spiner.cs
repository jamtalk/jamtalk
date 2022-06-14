using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spiner : AnimationScript
{
    private enum eDirection
    {
        Left,
        Right
    }
    [SerializeField]
    private eDirection direction;

    private Tween tween;
    public override void Play()
    {
        Stop();
        var angle = direction == eDirection.Right ? -360 : 360;
        tween = GetComponent<RectTransform>().DORotate(new Vector3(0, 0, angle), duration,RotateMode.FastBeyond360);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Incremental);
    }
    public override void Stop()
    {
        if(tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }

}
