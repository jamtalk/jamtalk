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
    Sequence seq;

    public void GuideStop()
    {
        if( seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
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
        gameObject.SetActive(true);
        Stop();
        tween = finger.DOScale(1.2f, 1f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Yoyo);
    }

    public void DoShake(GameObject target, TweenCallback callback, float duration = 1f, float power = 10f)
    {
        gameObject.SetActive(true);
        target.gameObject.SetActive(true);

        target.transform.position = gameObject.transform.position;

        seq = DOTween.Sequence();
        seq.Append(target.transform.DOShakePosition(duration, power));
        seq.Insert(0, gameObject.transform.DOShakePosition(duration, power));
        seq.onComplete += callback;

        seq.Play();
    }

    public void DoMove(Vector3 target ,TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 2f));
        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMove(float delay,Vector3 target, TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 2f).SetDelay(delay));
        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMove(GameObject move, Vector3 target, TweenCallback callback)
    {
        gameObject.SetActive(true);
        var moveTarget = move.GetComponent<RectTransform>();
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 2f));
        seq.Insert(0f, moveTarget.DOMove(target, 2f));

        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMove(RectTransform[] targets, GameObject move, TweenCallback callback)
    {
        move.transform.position = gameObject.transform.position;
        gameObject.SetActive(true);
        move.SetActive(true);
        seq = DOTween.Sequence();
        var index = 0;
        foreach (var item in targets)
        {
            seq.Insert(index, transform.DOMove(item.transform.position, 1f));
            seq.Insert(index, move.transform.DOMove(item.transform.position, 1f));
            index++;
        }

        seq.onComplete += callback;

        seq.Play();
    }
    public void DoMove(RectTransform[] move, Vector3 target, TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();

        seq.Insert(0f, transform.DOMove(target, 2f));
        foreach (var item in move)
        {
            seq.Insert(0f, item.DOMove(target, 2f));
        }

        seq.onComplete += callback;

        seq.Play();
    }

    public void DoPress(TweenCallback callback = null)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        Tween tween = transform.DOScale(.7f, 1f);

        seq.Append(tween);
        seq.onComplete += callback;
        seq.Play();
    }
    public void DoPress(float delay, TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        Tween tween = transform.DOScale(.7f, 1f).SetDelay(delay);

        seq.Append(tween);
        seq.onComplete += callback;
        seq.Play();
    }

    public void DoClick(TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        Tween firstTween = transform.DOScale(1.2f, 1f);
        Tween secondsTween = transform.DOScale(1f, 1f);

        seq.Append(firstTween);
        seq.Append(secondsTween);
        seq.onComplete += callback;

        seq.Play();
    }
    public void DoFade(int loopCnt, TweenCallback callback)
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();

        foreach (var item in images)
            seq.Insert(0, item.DOFade(0, 1));

        seq.SetLoops(loopCnt, 0);

        seq.onComplete += callback;
        seq.Play();
    }
}

