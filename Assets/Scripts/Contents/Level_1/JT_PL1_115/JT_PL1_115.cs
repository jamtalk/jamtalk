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
    public EventSystem eventSystem;
    public Card114[] cards;
    private List<Card114> selected = new List<Card114>();
    protected override eContents contents => eContents.JT_PL1_115;

    protected override bool CheckOver() => !cards.Select(x => x.card.IsFornt).Contains(false);
    protected override int GetTotalScore() => cards.Where(x => x.card.IsFornt).Count();
    protected override void ShowResult()
    {
        eventSystem.enabled = true;
        base.ShowResult();
    }

    bool isTurn = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isTurn) yield return null;
        isTurn = false;

        //for (int i = 0; i < (cards.Length / 2); i++)
        //{

        var upper = cards.Where(x => !x.card.IsFornt).ToArray()
            .Where(x => x.alhpabetData.type == eAlphabetType.Upper)
            .OrderBy(x => Random.Range(0, 100))
            .First();
        var lower = cards.Where(x => !x.card.IsFornt).ToArray()
            .Where(x => x.alhpabetData.alhpabet == upper.alhpabetData.alhpabet)
            .Where(x => x.alhpabetData.type == eAlphabetType.Lower).ToArray()
            .OrderBy(x => Random.Range(0, 100))
            .First();

        guideFinger.gameObject.SetActive(true);

        Card114[] guideCards = { upper, lower };
        guideCards = guideCards.OrderBy(x => Random.Range(0, 100)).ToArray();
        var value = upper.alhpabetData;
        foreach (var item in guideCards)
        {
            guideFinger.gameObject.SetActive(true);
            guideFinger.DoMove(item.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    item.card.Turnning(1f, () =>
                    {
                        CardClickMotion(item, item.alhpabetData);
                    });
                    guideFinger.gameObject.SetActive(false);
                });
            });

            while (!isTurn) yield return null;
            isTurn = false;
            //}
        }

        yield return new WaitForSecondsRealtime(1.5f);
        EndGuidnce();
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        foreach (var item in cards)
        {
            item.card.SetFront();
            item.star.gameObject.SetActive(false);
        }

        selected.Clear();
        StartCoroutine(StartContent());
    }

    protected override void Awake()
    {
        base.Awake();
        var targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        if(GameManager.Instance.currentAlphabet > eAlphabet.B)
        {
            Debug.Log(targets.Length);
            var preAlphabets = GameManager.Instance.alphabets
                .Where(x => x < GameManager.Instance.currentAlphabet)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(2);

            targets = targets.Union(preAlphabets).ToArray();

            Debug.Log(targets.Length);
        }
        else
        {
            targets = targets.SelectMany(x => new eAlphabet[] { x, x }).ToArray();
        }
        var questions = targets
            .SelectMany(x => new Card114Data[] { new Card114Data(x,eAlphabetType.Upper), new Card114Data(x, eAlphabetType.Lower) })
            .OrderBy(x=>Random.Range(0f,100f))
            .ToArray();
        Debug.Log(questions.Length + "??\n" + string.Join("\n", questions.Select(x => string.Format("{0} {1}", x.alhpabet, x.type))));
        var randomCards = cards.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for (int i = 0; i < cards.Length; i++)
        {
            SetCard(randomCards[i], questions[i].alhpabet, questions[i].type);
            //SetCard(randomCards[upper], questions[upper],eAlphabetType.Upper);
            //SetCard(randomCards[lower], questions[lower], eAlphabetType.Lower);
        }

        StartCoroutine(StartContent());   
    }
    private void SetCard(Card114 card, eAlphabet alphabet, eAlphabetType type)
    {
        card.Init(new Card114Data(alphabet, type));
        card.card.onClick += () => eventSystem.enabled = false;
        card.onSelected += (value) =>
        {
            CardClickMotion(card, value);
        };
    }

    private void CardClickMotion(Card114 card, Card114Data value)
    {
        selected.Add(card);
        if (selected.Count == 2)
        {
            if (selected[0].alhpabetData.alhpabet == selected[1].alhpabetData.alhpabet
            && selected[0].alhpabetData.type != selected[1].alhpabetData.type)
            {
                if (CheckOver())
                {
                    audioPlayer.Play(GameManager.Instance.GetResources(value.alhpabet).AudioData.act2, () =>
                    {
                        ShowResult();
                    });
                }
                else
                {
                    audioPlayer.Play(GameManager.Instance.GetResources(value.alhpabet).AudioData.act2, () =>
                    {
                        isTurn = true;
                        selected[0].ShowStar();
                        selected[1].ShowStar();
                        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
                        selected.Clear();
                        eventSystem.enabled = true;
                    });
                }
            }
            else
            {
                audioPlayer.Play(GameManager.Instance.GetResources(value.alhpabet).AudioData.phanics);
                selected[0].card.Turnning(onCompleted: () => eventSystem.enabled = true);
                selected[1].card.Turnning(onCompleted: () => eventSystem.enabled = true);
                selected.Clear();
            }
        }
        else
        {
            audioPlayer.Play(GameManager.Instance.GetResources(value.alhpabet).AudioData.phanics, () => isTurn = true);
            eventSystem.enabled = true;
        }
    }
    IEnumerator StartContent()
    {
        yield return new WaitForSeconds(3f);
        for(int i = 0; i<cards.Length; i++)
        {
            cards[i].card.Turnning(.25f);
            yield return new WaitForSeconds(.1f);
        }
        eventSystem.enabled = true;
        isTurn = true;
    }
}
//public class AlphabetQuestion
//{
//    public eAlphabet value;
//    public eAlphabetType type;

//    public AlphabetQuestion(eAlphabet value, eAlphabetType type)
//    {
//        this.value = value;
//        this.type = type;
//    }
//}