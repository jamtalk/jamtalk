using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ButtonExitnction : MonoBehaviour
{
    public eAlphabet value { get; private set; }
    public event Action<string> onClick;
    public ImageButton imageButton;
    public RectTransform star;
    public Button button => imageButton.button;
    public RectTransform rtButton => imageButton.GetComponent<RectTransform>();
    private Sequence seq;
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(imageButton.image.sprite.name);
        });
    }
    public void Init(Sprite sprite)
    {
        imageButton.SetSprite(sprite);
        rtButton.localScale = Vector3.one;
        star.rotation = Quaternion.Euler(Vector3.zero);
        if(seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }
    public void Exitnction()
    {
        seq = DOTween.Sequence();
        var exitTween = rtButton.DOScale(0, .25f);
        seq.Append(exitTween);

        var starTween = star.DOScale(.3f, .25f);
        starTween.SetEase(Ease.Linear);
        starTween.SetLoops(2, LoopType.Yoyo);
        seq.Append(starTween);

        var starRotateTween = star.DORotate(new Vector3(0, 0, -180), .5f);
        starRotateTween.SetEase(Ease.Linear);
        seq.Insert(.25f, starRotateTween);

        seq.Play();
    }
}
