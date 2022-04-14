using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ButtonExitnction : MonoBehaviour
{
    [SerializeField]
    private RectTransform incorrect;
    public eAlphabet value { get; private set; }
    public event Action<string> onClick;
    public ImageButton imageButton;
    public RectTransform star;
    public Button button => imageButton.button;
    public RectTransform rtButton => imageButton.GetComponent<RectTransform>();
    private Sequence correctSequence;
    private Sequence incorrectSequence;
    private void Awake()
    {
        incorrect.gameObject.SetActive(false);
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(imageButton.image.sprite.name);
        });
    }
    public void Init(Sprite sprite)
    {
        incorrect.gameObject.SetActive(false);
        imageButton.SetSprite(sprite);
        rtButton.localScale = Vector3.one;
        star.rotation = Quaternion.Euler(Vector3.zero);
        if(correctSequence != null)
        {
            correctSequence.Kill();
            correctSequence = null;
        }
        if(incorrectSequence != null)
        {
            incorrectSequence.Kill();
            incorrectSequence = null;
        }
    }
    public void Exitnction()
    {
        correctSequence = DOTween.Sequence();
        var exitTween = rtButton.DOScale(0, .25f);
        correctSequence.Append(exitTween);

        var starTween = star.DOScale(.3f, .25f);
        starTween.SetEase(Ease.Linear);
        starTween.SetLoops(2, LoopType.Yoyo);
        correctSequence.Append(starTween);

        var starRotateTween = star.DORotate(new Vector3(0, 0, -180), .5f);
        starRotateTween.SetEase(Ease.Linear);
        correctSequence.Insert(.25f, starRotateTween);

        correctSequence.Play();
    }
    public void Incorrect()
    {
        incorrectSequence = DOTween.Sequence();
        var twX = incorrect.DOScaleX(1.5f, .75f);
        twX.SetEase(Ease.Linear);
        twX.SetLoops(2, LoopType.Yoyo);

        var twY = incorrect.DOScaleY(1f, .25f);
        twY.SetEase(Ease.Linear);
        twY.SetLoops(2, LoopType.Yoyo);

        incorrectSequence.Append(twY);
        incorrectSequence.Insert(.125f, twX);
        incorrectSequence.onPlay += () =>
        {
            incorrect.localScale = new Vector3(0, 0, 1);
            incorrect.gameObject.SetActive(true);
        };
        incorrectSequence.onKill += () => incorrect.gameObject.SetActive(false);
    }
}
