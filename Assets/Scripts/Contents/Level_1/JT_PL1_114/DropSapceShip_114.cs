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
    public event Action<AlphabetWordsData> onInner;

    public RectTransform rtObject;
    public eAlphabet alphabet;
    public void Awake()
    {
        button.onClick.AddListener(() => alphbetPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.act2));
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
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabet);
        image.preserveAspect = true;

        float duration = 2f;
        beamPlayer.Play(duration);
        var tween = rtObject.DOAnchorPosY(0, duration);
        tween.onComplete += () =>
        {
            alphbetPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.act2, onCompleted);
        };
    }
    public void InObject(AlphabetWordsData data)
    {
        SetOutter();
        image.sprite = data.sprite;
        image.preserveAspect = true;
        alphbetPlayer.Play(data.act, () =>
        {
            float duration = 2f;
            beamPlayer.Play(duration);
            var tween = rtObject.DOAnchorPosY(250f, duration);
            tween.SetEase(Ease.InCubic);
            tween.onComplete += () => onInner?.Invoke(data);
        });
    }
}
