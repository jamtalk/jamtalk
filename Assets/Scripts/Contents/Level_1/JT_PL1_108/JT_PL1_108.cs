using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL1_108 : MultiAnswerContents<Question108, AlphabetWordsData>
{
    public Card_108[] cards;
    public GameObject finger;

    protected override int QuestionCount => 1;
    protected int questionElementCount = 5;
    protected int correctElementCount = 4;
    public Button buttonAudio;
    private bool isStart = false;
    protected override eContents contents => eContents.JT_PL1_108;

    Coroutine coroutine;
    bool isTurn = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        while (!isTurn) yield return null;

        guideFinger.gameObject.SetActive(true);

        var correctCard = cards.Where(x => x.data == currentQuestion.correct[currentQuestion.currentIndex]).First();
        isNext = false;
        guideFinger.DoMove(correctCard.transform.position, () =>
        {
            isStart = true;
            guideFinger.DoClick(() =>
            {
                audioPlayer.Play(currentQuestion.correct[currentQuestion.currentIndex].clip);
                correctCard.turnner.Turnning(1f, () =>
                {
                    AddAnswer(currentQuestion.correct[currentQuestion.currentIndex]);
                });
            });
        });

        while (!isNext) { yield return null; }
    }

    protected override void EndGuidnce()
    {
        StopCoroutine(coroutine);
        foreach (var item in cards)
        {
            item.turnner.SeqStop();
            item.turnner.SetBack();
        }
        base.EndGuidnce();
    }

    protected override void AddAnswer(AlphabetWordsData answer)
    {
        isStart = false;
        base.AddAnswer(answer);
    }

    protected override void Awake()
    {
        base.Awake();
        buttonAudio.onClick.AddListener(() =>
        {
            finger.SetActive(false);
            audioPlayer.Play(currentQuestion.correct[currentQuestion.currentIndex].clip);
        });
    }
    protected override eGameResult GetResult() => eGameResult.Perfect;
    protected override List<Question108> MakeQuestion()
    {
        var targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        var correct = targets
            .SelectMany(x => 
                GameManager.Instance.GetResources(x).Words
                .OrderBy(y => Random.Range(0f, 100f))
                .Take(correctElementCount / 2))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        var questions = GameManager.Instance.alphabets
            .Where(x=>x!=GameManager.Instance.currentAlphabet)
            .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionElementCount)
            .ToArray();

        return new List<Question108>() { new Question108(correct, questions) };
    }

    protected override void ShowQuestion(Question108 question)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < cards.Length)
            {
                cards[i].Init(question.totalQuestion[i]);
                cards[i].turnner.SetBack();
                AddOnClickCardListener(cards[i]);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
        coroutine = StartCoroutine(TurrningCards());
    }
    private void AddOnClickCardListener(Card_108 card)
    {
        card.turnner.onTurned += () =>
        {
            if (isStart && !CheckOver())
            {
                audioPlayer.Play(currentQuestion.correct[currentQuestion.currentIndex].clip);
            }
        };
        card.onClick += (value) =>
        {
            if (currentQuestion.currentIndex >= 4)
                return;

            if (currentQuestion.correct[currentQuestion.currentIndex] == value)
            {
                AddAnswer(currentQuestion.correct[currentQuestion.currentIndex]);
                isStart = true;
            }
        };
        card.checkVaild += (value) =>
        {
            var vaild = value == currentQuestion.correct[currentQuestion.currentIndex];
            audioPlayer.Play(value.clip);
            return vaild;
        };
    }

    //protected override void AddAnswer(string answer)
    //{
    //    base.AddAnswer(answer);
    //    if(!CheckOver())
    //        audioPlayer.Play(GameManager.Instance.GetClipWord(currentQuestion.correct[currentQuestion.currentIndex]));
    //}

    IEnumerator TurrningCards()
    {
        yield return new WaitForSeconds(.1f);
        var cards = this.cards.Where(x => x.gameObject.activeSelf).ToArray();
        var count = 0;
        for(int i = 0;i < cards.Length; i++)
        {
            cards[i].turnner.Turnning(.25f,()=>count+=1);
            yield return new WaitForSeconds(.1f);
        }
        while (count < cards.Length) { yield return null; }
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].turnner.buttonFront.interactable = true;
        }
        audioPlayer.Play(currentQuestion.correct[currentQuestion.currentIndex].clip, () => isTurn = true);
    }
}
public class Question108 : MultiQuestion<AlphabetWordsData>
{
    public int currentIndex;
    public Question108(AlphabetWordsData[] correct, AlphabetWordsData[] questions) : base(correct, questions) 
    {
        currentIndex = 0;
    }

    protected override bool CheckCorrect(AlphabetWordsData answer)
    {
        return correct[currentIndex] == answer;
    }

    public override void SetAnswer(AlphabetWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }

}
