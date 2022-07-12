using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TurningCard))]
public class Card114 : MonoBehaviour
{
    public TurningCard card => GetComponent<TurningCard>();
    public Image image;
    public Text text;
    public Card114Data alhpabetData;
    public DigraphsWordsData DigraphsWordsData;
    public event Action<Card114Data> onSelected;
    public event Action<DigraphsWordsData> onSelecte;
    public event Action onDeselected;

    public RectTransform star;
    public void Init(Card114Data data)
    {
        this.alhpabetData = data;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColorCard, data.type, data.alhpabet);
        card.Init(callback: () =>
        {
            if (card.IsFornt)
                onSelected?.Invoke(data);
            else
                onDeselected?.Invoke();
        },alwaysFrontDisable:true);
    }

    public void Init(DigraphsWordsData data, string color)
    {
        this.DigraphsWordsData = data;
        text.text = data.key.Replace(data.key,
            "<color=\"" + color + "\">" + data.key + "</color>");

        Debug.Log(text.text);
        card.Init(callback: () =>
        {
            if (card.IsFornt)
                onSelecte?.Invoke(data);
            else
                onDeselected?.Invoke();
        }, alwaysFrontDisable: true);
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
public class Card114Data
{
    public eAlphabet alhpabet;
    public eAlphabetType type;

    public Card114Data(eAlphabet alhpabet, eAlphabetType type)
    {
        this.alhpabet = alhpabet;
        this.type = type;
    }
}
