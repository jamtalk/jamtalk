using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimatingDino : MonoBehaviour
{
    public RectTransform head;
    public RectTransform tail;
    public RectTransform leg_back_left;
    public RectTransform leg_back_right;
    public RectTransform leg_front_left;
    public RectTransform leg_front_right;
    private void Awake()
    {
        StartTween(head);
        StartTween(tail);
        StartTween(leg_back_left);
        StartTween(leg_back_right);
        StartTween(leg_front_left);
        StartTween(leg_front_right);
    }

    private void StartTween(RectTransform rt)
    {
        var tween = rt.DORotate(new Vector3(0, 0, 0), 1f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
        tween.Play();
    }
}
