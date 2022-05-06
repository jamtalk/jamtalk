using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropSapceShip_114 : MonoBehaviour
{
    public Button button;
    public Image image;
    public AudioSinglePlayer beamPlayer;
    public AudioSinglePlayer alphbetPlayer;
    public event Action<string> onInner;

    public RectTransform rtObject;
    public eAlphabet alphabet;
    public void Awake()
    {
        button.onClick.AddListener(() => alphbetPlayer.Play(GameManager.Instance.GetClipAct2(alphabet)));
    }
    public void SetInner()
    {
        rtObject.anchoredPosition = new Vector2(0, 250f);
    }
    public void SetOutter()
    {
        rtObject.anchoredPosition = Vector2.zero;
    }
    public void OutObject(eAlphabet alphabet, Action onCompleted)
    {
        SetInner();
        this.alphabet = alphabet;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.FullColor, eAlphbetType.Upper, alphabet);
        image.preserveAspect = true;

        float duration = 2f;
        beamPlayer.Play(duration);
        var tween = rtObject.DOAnchorPosY(0, duration);
        tween.onComplete += () =>
        {
            alphbetPlayer.Play(GameManager.Instance.GetClipAct2(alphabet), onCompleted);
        };
    }
    public void InObject(Sprite sprite)
    {
        SetOutter();
        image.sprite = sprite;
        image.preserveAspect = true;
        alphbetPlayer.Play(GameManager.Instance.GetClipAct3(sprite.name), () =>
        {
            float duration = 2f;
            beamPlayer.Play(duration);
            var tween = rtObject.DOAnchorPosY(250f, duration);
            tween.SetEase(Ease.InCubic);
            tween.onComplete += () => onInner?.Invoke(sprite.name);
        });
    }
}
