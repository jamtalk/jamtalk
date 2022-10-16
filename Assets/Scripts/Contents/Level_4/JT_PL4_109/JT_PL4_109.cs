using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL4_109 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_109;
    protected override bool CheckOver() => !cards.Select(x => x.card.IsFornt).Contains(false);
    protected override int GetTotalScore() => cards.Where(x => x.card.IsFornt).Count();

    private string[] richs =
        { "blue", "cyan", "magenta", "green", "red", "brown"
            , "grey" , "teal", "orange", "darkblue", "purple", "maroon"};
    private List<Card114> selected = new List<Card114>();
    public Card114[] cards;
    public Sprite[] cardImages;

    bool isNext = false;
    bool isTurn = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;
        isNext = false;
        for(int i = 0; i < cards.Length / 2; i++)
        {
            var target = cards.Where(x => !x.card.IsFornt)
                .OrderBy(x => Random.Range(0, 100)).First();
            var targets = cards.Where(x => x.DigraphsWordsData == target.DigraphsWordsData)
                .OrderBy(x => Random.Range(0, 100))
                .ToList();

            for (int j = 0; j < targets.Count; j++)
            {
                guideFinger.DoMove(targets[j].transform.position, () =>
                {
                    guideFinger.DoClick(() =>
                    {
                        guideFinger.gameObject.SetActive(false);
                        targets[j].card.Turnning(onCompleted: () =>
                        {
                            if (j == targets.Count - 1)
                            {
                                audioPlayer.Play(targets[j].DigraphsWordsData.act, () =>
                                {
                                    MatchMotion(targets, () => isNext = true);
                                });
                            }
                            else
                            {
                                audioPlayer.Play(targets[j].DigraphsWordsData.clip, () => isNext = true);
                                SetCardIntracable(true);
                            }
                        });
                    });
                });
                while (!isNext) yield return null;
                isNext = false;
            }
        }

        isGuide = false;
        selected.Clear();
        foreach(var item in cards)
            item.star.gameObject.SetActive(false);
        MakeQuestion();
        Debug.Log(selected.Count);
    }
    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void MakeQuestion()
    {
        var question = GameManager.Instance.GetDigraphs()
            .OrderBy(x=>Random.Range(0f,100f))
            .Take(cards.Length/2)
            .SelectMany(x=> new DigraphsWordsData[] {x,x})
            .OrderBy(y => Random.Range(0f, 100f))
            .ToArray();

        var randomCards = cards.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        var randomColor = richs.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].backImage.sprite = cardImages.OrderBy(x => Random.Range(0f, 100f)).First();
            if (i > 0 && i % 2 != 0)
                continue;

            var upper = i;
            var lower = i + 1;

            if (isGuide)
            {
                SetCard(randomCards[upper], question[upper], randomColor[i]);
                SetCard(randomCards[lower], question[lower], randomColor[i]);
            }
        }

        StartCoroutine(StartContent());
    }
    private void SetCard(Card114 card, DigraphsWordsData data, string color)
    {
        card.Init(data, color);
        card.card.onClick += () => SetCardIntracable(false);
        card.onSelecte += (value) =>
        {
            selected.Add(card);
            Debug.Log(selected.Count);
            if (selected.Count == 2)
            {
                if (selected[0].DigraphsWordsData.key == selected[1].DigraphsWordsData.key)
                {
                    if (CheckOver())
                    {
                        if(!isGuide)
                            audioPlayer.Play(data.clip, ShowResult);
                        else
                        {
                            Debug.Log("ischecOver");
                            isGuide = false;
                            selected.Clear();
                            MakeQuestion();
                        }
                    }
                    else
                    {
                        audioPlayer.Play(data.act, () =>
                        {
                            MatchMotion(selected);
                        });
                    }
                }
                else
                {
                    audioPlayer.Play(data.clip);
                    selected[0].card.Turnning(onCompleted: () => SetCardIntracable(true));
                    selected[1].card.Turnning(onCompleted: () => SetCardIntracable(true));
                    selected.Clear();
                }
            }
            else
            {
                audioPlayer.Play(data.clip);
                SetCardIntracable(true);
            }
        };
    }

    private void MatchMotion(List<Card114> cards, Action action = null)
    {
        cards[0].ShowStar();
        cards[1].ShowStar();
        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), action);
        cards.Clear();
        SetCardIntracable(true);
    }

    private void SetCardIntracable(bool value)
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetIntractable(value);
    }
    IEnumerator StartContent()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].card.Turnning(.25f);
            yield return new WaitForSeconds(.1f);
        }
        SetCardIntracable(true);
        isNext = true;
    }
}