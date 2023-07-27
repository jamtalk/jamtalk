using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropSapceShip_114 : MonoBehaviour
{
    public Button button;
    public Image image;
    public SkeletonGraphic charactor;
    public AudioSinglePlayer beamPlayer;
    public AudioSinglePlayer alphbetPlayer;
    public event Action<AlphabetWordsData> onInner;
    public RectTransform rtObject;
    public eAlphabet alphabet;
    private Tween tween;
    public void Awake()
    {
        button.onClick.AddListener(() => alphbetPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.act2));

        Fluffy();
    }

    public void Fluffy()
    {
        Sequence seq = DOTween.Sequence();

        var rect = button.GetComponent<RectTransform>();
        var shipTween = rect.DOLocalMoveY(20, 1);
        var charactorTween = charactor.transform.DOMoveY(.4f, 1);

        seq.Insert(0, shipTween);
        seq.Insert(0, charactorTween);

        seq.SetLoops(-1, LoopType.Yoyo);
        seq.Play();
    }
    public void SetInner()
    {
        rtObject.anchoredPosition = new Vector2(0, 250f);
    }
    public void SetOutter()
    {
        rtObject.anchoredPosition = Vector2.zero;
    }
    public void KillTween()
    {
        if (tween != null)
            tween.Kill();
        tween = null;
    }
    public void OutObject(eAlphabet alphabet, Action onCompleted)
    {
        Debug.Log("OUT 호출");
        SetInner();
        this.alphabet = alphabet;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabet);
        image.preserveAspect = true;

        float duration = 2f;
        beamPlayer.Play(duration);
        if(tween != null)
        {
            tween.Kill();
            tween = null;
        }
        tween = rtObject.DOAnchorPosY(0, duration);
        tween.onKill += SetOutter;
        tween.onComplete += () =>
        {
            Debug.Log("오브젝트 나옴");
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
