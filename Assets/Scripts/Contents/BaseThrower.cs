using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseThrower<T> : MonoBehaviour
{
    protected float upperTime;
    protected float moveTime;
    protected float lowerTime;
    protected float inertTime;
    protected float upperSize = 1.5f;
    protected RectTransform rt => GetComponent<RectTransform>();
    protected Sequence seq = null;

    protected abstract void SetItem(T item);
    protected abstract void SetTime(RectTransform target);
    public void Throw(T item, RectTransform target, System.Action onCompleted)
    {
        SetItem(item);
        transform.localScale = Vector3.one;
        SetTime(target);

        gameObject.SetActive(true);

        seq = DOTween.Sequence();

        var scaleUpperTween = transform.DOScale(upperSize, upperTime);
        scaleUpperTween.SetEase(Ease.Linear);
        var scaleLowerTween = transform.DOScale(.5f, lowerTime);
        scaleLowerTween.SetEase(Ease.Linear);
        var moveTween = transform.DOMove(target.position, moveTime);
        moveTween.SetEase(Ease.Linear);

        seq.Append(scaleUpperTween);
        seq.Append(moveTween);
        seq.Insert(inertTime, scaleLowerTween);

        seq.onKill += () =>
        {
            gameObject.SetActive(false);
            onCompleted?.Invoke();
        };
    }
    protected virtual void OnDisable()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
