using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GuideFingerAnimation : MonoBehaviour
{
    public Image[] images;
    [SerializeField]
    private RectTransform finger;

    Tween tween;
    public void Stop()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }
    public void Play()
    {
        Stop();
        tween = finger.DOScale(1.2f, 1f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Yoyo);
    }

    public void DoMoveCorrect(Vector3 target ,TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 2f));
        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMoveCorrect(GameObject move, Vector3 target, TweenCallback callback)
    {
        var moveTarget = move.GetComponent<RectTransform>();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 2f));
        seq.Insert(0f, moveTarget.DOMove(target, 2f));

        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMoveCorrect(RectTransform[] move, Vector3 target, TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();

        seq.Insert(0f, transform.DOMove(target, 2f));
        foreach (var item in move)
        {
            seq.Insert(0f, item.DOMove(target, 2f));
        }

        seq.onComplete += callback;

        seq.Play();
    }

    public void DoPress(TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();
        Tween tween = transform.DOScale(.7f, 1f);

        seq.Append(tween);
        seq.onComplete += callback;
        seq.Play();
    }

    public void DoClick(TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();
        Tween firstTween = transform.DOScale(1.2f, 1f);
        Tween secondsTween = transform.DOScale(1f, 1f);

        seq.Append(firstTween);
        seq.Append(secondsTween);
        seq.onComplete += callback;

        seq.Play();
    }
    public void DoFade(int loopCnt, TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();

        foreach (var item in images)
            seq.Insert(0, item.DOFade(0, 1));

        seq.SetLoops(loopCnt, 0);

        seq.onComplete += callback;
        seq.Play();
    }
}

