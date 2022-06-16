using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class SandEffect : MonoBehaviour
{
    Tween tween = null;
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -20f);
        tween = transform.DORotate(new Vector3(0, 0, 20), 1f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.Play();
    }
    private void OnDisable()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }
}
