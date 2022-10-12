using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WitchResult : MonoBehaviour
{
    public Image image;
    public void ShowResult(Sprite sprite,float duration,TweenCallback callback=null)
    {
        var rt = GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        image.sprite = sprite;
        image.preserveAspect = true;
        gameObject.SetActive(true);
        var turm = 5f;
        var upDuration = duration / turm * (turm - 1);
        var downDuration = duration / turm;
        var seq = DOTween.Sequence();

        var upTween = rt.DOScale(1 / turm * (turm + 1), upDuration);
        upTween.SetEase(Ease.Linear);
        seq.Append(upTween);

        var downTween = rt.DOScale(1f, downDuration);
        seq.Append(downTween);

        seq.onComplete += callback;
        seq.Play();
    }
}