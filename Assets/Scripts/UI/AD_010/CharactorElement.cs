using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.UI;

public class CharactorElement : MonoBehaviour
{
    public SkeletonGraphic charactor;
    public Image plat;
    public RectTransform rt;
    //[HideInInspector]
    public int index;

    public void Init(RectTransform rect, int index)
    {
        StartCoroutine(InitCoroutine(rect, index));
    }
    public IEnumerator InitCoroutine(RectTransform rect, int index)
    {
        this.index = index;
        transform.DOMove(rect.position, 0f);
        if(index == 2 ) transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForEndOfFrame();
    }

    public void Move(RectTransform rect, int index, TweenCallback callback = null)
    {
        this.index = index;
        Vector3 scale;
        if (index == 2)
            scale = new Vector3(1.5f, 1.5f, 1.5f);
        else
            scale = new Vector3(1f, 1f, 1f);

        Sequence seq = DOTween.Sequence();

        Tween moveTween = transform.DOMove(rect.position, 1f);
        Tween scaleTween = transform.DOScale(scale, 1f);

        seq.Insert(0, scaleTween);
        seq.Insert(0, moveTween);
        seq.onComplete += callback;
        seq.Play();
    }
}
