using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnswerImage116 : MonoBehaviour
{
    public Image image;
    Tween tween = null;
    public void Show(Sprite sprite)
    {
        if(tween != null)
        {
            tween.Kill();
            tween = null;
        }
        image.sprite = sprite;
        image.preserveAspect = true;
        tween = image.GetComponent<RectTransform>().DOScale(1.5f, .25f);
        tween.SetLoops(2, LoopType.Yoyo);
        tween.onPlay += () => gameObject.SetActive(true);
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
