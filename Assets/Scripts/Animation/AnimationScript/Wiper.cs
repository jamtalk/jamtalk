using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Wiper : AnimationScript
{
    private enum eDirection
    {
        Left,
        Right
    }
    [SerializeField]
    [Range(1f,90f)]
    private float angle;
    [SerializeField]
    private eDirection direction;
    private Sequence seq;
    public override float duration => base.duration / 2f;

    public override void Play()
    {
        Stop();
        float startAngle = angle / 2f;
        float endAngle = angle / 2f;
        seq = DOTween.Sequence();
        if (direction == eDirection.Left)
            endAngle *= -1f;
        else
            startAngle *= -1f;

        var startTween = GetComponent<RectTransform>().DORotate(new Vector3(0, 0, startAngle), duration);
        startTween.SetEase(Ease.Linear);
        startTween.SetLoops(2, LoopType.Yoyo);
        var endTween = GetComponent<RectTransform>().DORotate(new Vector3(0, 0, endAngle), duration);
        endTween.SetEase(Ease.Linear);
        endTween.SetLoops(2, LoopType.Yoyo);

        seq.Append(startTween);
        seq.Append(endTween);

        seq.SetLoops(-1, LoopType.Restart);
        seq.Play();
    }

    public override void Stop()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
