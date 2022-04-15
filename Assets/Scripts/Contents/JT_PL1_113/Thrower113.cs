using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Thrower113 : MonoBehaviour
{
    public Image imageProduct;
    public Text textValue;
    private Sequence seq = null;
    private RectTransform rt => GetComponent<RectTransform>();
    public void Throw(Item113 item, RectTransform target, System.Action onComplte)
    {
        imageProduct.sprite = item.product.sprite;
        textValue.text = item.textValue.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);

        seq = DOTween.Sequence();
        var upperTime = .125f;
        var moveTime = Vector3.Distance(transform.position, target.position) / 3000f;
        var lowerTime = moveTime / 2f;
        var inertTime = upperTime + moveTime / 2f;

        var scaleUpperTween = transform.DOScale(1.5f, upperTime);
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
            onComplte?.Invoke();
        };
        seq.Play();
    }
    private void OnDisable()
    {
        if(seq!=null)
        {
            seq.Kill();
            seq = null;
        }
    }
}
