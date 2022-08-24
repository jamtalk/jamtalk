using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseThrower<T> : MonoBehaviour
{
    protected abstract float upperTime { get; }
    protected abstract float lowerTime { get; }
    protected abstract float inertTime { get; }
    protected virtual float upperSize => 1.5f;
    protected virtual float moveTime
    {
        get
        {
            var currentPos = Camera.main.ScreenToWorldPoint(transform.position);
            var targetPos = Camera.main.ScreenToWorldPoint(target.position);
            var distance = Vector3.Distance(currentPos, targetPos);
            return distance / speed;
        }
    }
    public float speed = .1f;
    protected RectTransform rt => GetComponent<RectTransform>();
    protected RectTransform target;
    protected Sequence seq = null;

    protected abstract void SetItem(T item);
    public void Throw(T item, RectTransform target, System.Action onCompleted)
    {
        SetItem(item);
        transform.localScale = Vector3.one;
        this.target = target;

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
