using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeagullControler : Pooling<PoolingElement>
{
    public RectTransform[] rects;

    public override PoolingElement GetObject(PoolingElement orizinal, Transform parent)
    {
        return base.GetObject(orizinal, parent);
    }

    public void MoveElement(PoolingElement element, TweenCallback callback)
    {
        var random = Random.Range(0, rects.Length / 2) * 2;
        Sequence seq = DOTween.Sequence();
        element.transform.position = rects[random].transform.position;

        seq.Append(element.transform.DOMove(rects[random + 1].transform.position, 10f));
        seq.onComplete += callback;

        seq.Play();
    }

    public void MoveCompleted(PoolingElement element)
    {
        element.gameObject.SetActive(false);
    }
}
