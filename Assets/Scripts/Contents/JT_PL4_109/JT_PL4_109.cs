using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_109 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_109;
    protected override bool CheckOver() => !cards.Select(x => x.card.IsFornt).Contains(false);
    protected override int GetTotalScore() => cards.Where(x => x.card.IsFornt).Count();

    private string[] richs =
        { "blue", "cyan", "magenta", "green", "red", "brown"
            , "grey" , "teal", "orange", "darkblue", "purple"};
    private List<Card114> selected = new List<Card114>();
    public Card114[] cards;

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void MakeQuestion()
    {
        var question = GameManager.Instance.digrpahs
            .Where(x => x >= GameManager.Instance.currentDigrpahs)
            .Take(6)
            .Select(x => GameManager.Instance.GetDigraphs(x)
                .OrderBy(y=>Random.Range(0f,100f)).First())
            .SelectMany(x=> new DigraphsSource[] {x,x})
            .ToArray();

        var randomCards = cards.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        var randomColor = richs.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for (int i = 0; i < cards.Length; i++)
        {
            if (i > 0 && i % 2 != 0)
                continue;
            
            var upper = i;
            var lower = i + 1;

            SetCard(randomCards[upper], question[upper], randomColor[i]);
            SetCard(randomCards[lower], question[lower], randomColor[i]);
        }

        StartCoroutine(StartContent());
    }
    private void SetCard(Card114 card, DigraphsSource data, string color)
    {
        card.Init(data, color);
        card.card.onClick += () => SetCardIntracable(false);
        card.onSelecte += (value) =>
        {
            selected.Add(card);
            if (selected.Count == 2)
            {
                if (selected[0].digraphsData.type == selected[1].digraphsData.type)
                {
                    if (CheckOver())
                    {
                        audioPlayer.Play(data.clip, ShowResult);
                    }
                    else
                    {
                        audioPlayer.Play(data.act, () =>
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
    }
}