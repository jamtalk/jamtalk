using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FingerAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform finger;
    Tween tween;
    private void Stop()
    {
        if(tween!=null)
        {
            tween.Kill();
            tween = null;
        }
    }
    private void Play()
    {
        Stop();
        tween = finger.DOScale(1.2f, 1f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Yoyo);
    }
    private void OnEnable()
    {
        Play();
    }
    private void OnDisable()
    {
        Stop();
    }
}
