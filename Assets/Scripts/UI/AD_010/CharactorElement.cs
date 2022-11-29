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
        transform.position = rect.position;
        yield return new WaitForEndOfFrame();
        //int colorA = index < 0 ? 255 - (index * -1) : 255 - index;
        //Color color = charactor.color;
        //color.a = colorA;
        //charactor.color = color;
        //plat.color = color;
    }

    public void Move(RectTransform rect, int index, TweenCallback callback = null)
    {
        this.index = index;
        int colorA = index < 0 ? 255 - index * -1 : 255 - index;

        Sequence seq = DOTween.Sequence();

        Tween moveTween = transform.DOMove(rect.position, 1f);
        //Tween trnspTween = charactor.DOFade(colorA, 1f);
        //Tween trnspPlatTween = plat.DOFade(colorA, 1f);

        seq.Insert(0, moveTween);
        //seq.Insert(0, trnspTween);
        //seq.Insert(0, trnspPlatTween);
        seq.onComplete += callback;
        seq.Play();
    }
}
