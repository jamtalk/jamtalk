using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TurningCard))]
public class Card114 : MonoBehaviour
{
    public TurningCard card => GetComponent<TurningCard>();
    public Image image;
    public eAlphabet alphabet;
    public event Action<eAlphabet> onSelected;
    public event Action onDeselected;

    public RectTransform star;
    public void Init(eAlphabet alphabet, eAlphbetType type)
    {
        this.alphabet = alphabet;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColorCard, type, alphabet);
        card.Init(callback: () =>
        {
            if (card.IsFornt)
                onSelected?.Invoke(alphabet);
            else
                onDeselected?.Invoke();
        },alwaysFrontDisable:true);
    }
    public void SetIntractable(bool value)
    {
        card.buttonBack.interactable = value;
    }
    public void ShowStar()
    {
        star.gameObject.SetActive(true);
        var tween = star.DOScale(1.2f, .25f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(2,LoopType.Yoyo);
        tween.Play();
    }
}
