using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class JT_PL1_115 : BaseContents
{
    public Card114[] cards;
    private List<Card114> selected = new List<Card114>();
    public AudioSinglePlayer audioPlayer;
    protected override eContents contents => eContents.JT_PL1_115;

    protected override bool CheckOver() => !cards.Select(x => x.card.IsFornt).Contains(false);
    protected override int GetTotalScore() => cards.Where(x => x.card.IsFornt).Count();

    protected override void Awake()
    {
        base.Awake();
        var questions = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(4)
            .SelectMany(x => new eAlphabet[] { x, x })
            .ToArray();
        var randomCards = cards.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for (int i = 0; i < cards.Length; i++)
        {
            if (i > 0 && i % 2 != 0)
                continue;

            var upper = i;
            var lower = i + 1;
            SetCard(randomCards[upper], questions[upper],eAlphbetType.Upper);
            SetCard(randomCards[lower], questions[lower], eAlphbetType.Lower);
        }

        StartCoroutine(StartContent());   
    }
    private void SetCard(Card114 card, eAlphabet alphabet, eAlphbetType type)
    {
        card.Init(alphabet, type);
        card.card.onClick += () => SetCardIntracable(false);
        card.onSelected += (value) =>
        {
            selected.Add(card);
            if (selected.Count == 2)
            {
                if (selected[0].alphabet == selected[1].alphabet)
                {
                    if (CheckOver())
                    {
                        audioPlayer.Play(GameManager.Instance.GetClipAct2(value),ShowResult);
                    }
                    else
                    {
                        audioPlayer.Play(GameManager.Instance.GetClipAct2(value), ()=>
                        {
                            selected[0].ShowStar();
                            selected[1].ShowStar();
                            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
                            selected.Clear();
                            SetCardIntracable(true);
                        });
                    }
                }
                else
                {
                    audioPlayer.Play(GameManager.Instance.GetClipPhanics(value));
                    selected[0].card.Turnning(onCompleted: () => SetCardIntracable(true));
                    selected[1].card.Turnning(onCompleted: () => SetCardIntracable(true));
                    selected.Clear();
                }
            }
            else
            {
                audioPlayer.Play(GameManager.Instance.GetClipPhanics(value));
                SetCardIntracable(true);
            }
        };
    }
    private void SetCardIntracable(bool value)
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetIntractable(value);
    }
    IEnumerator StartContent()
    {
        yield return new WaitForSeconds(3f);
        for(int i = 0; i<cards.Length; i++)
        {
            cards[i].card.Turnning(.25f);
            yield return new WaitForSeconds(.1f);
        }
        SetCardIntracable(true);
    }
}
