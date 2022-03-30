using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_108 : MultiAnswerContents<Question108, string>
{
    public Card_108[] cards;

    protected override int QuestionCount => 1;
    protected int questionElementCount = 5;
    protected int correctElementCount = 4;
    public Button buttonAudio;
    protected override eContents contents => eContents.JT_PL1_108;
    protected override void Awake()
    {
        base.Awake();
        buttonAudio.onClick.AddListener(() =>
        {
            audioPlayer.Play(GameManager.Instance.GetClipWord(currentQuestion.correct[currentQuestion.currentIndex]));
        });
    }
    protected override eGameResult GetResult() => eGameResult.Perfect;
    protected override List<Question108> MakeQuestion()
    {
        var correct = GameManager.Instance.GetWords()
            .Where(x => x.First().ToString().ToUpper() == GameManager.Instance.currentAlphabet.ToString())
            .OrderBy(x => Guid.NewGuid().ToString())
            .Take(correctElementCount)
            .ToArray();
        var questions = GameManager.Instance.GetWords()
            .Where(x => x.First().ToString().ToUpper() != GameManager.Instance.currentAlphabet.ToString())
            .OrderBy(x => Guid.NewGuid().ToString())
            .Take(questionElementCount)
            .ToArray();

        return new List<Question108>() { new Question108(correct, questions) };
    }

    protected override void ShowQuestion(Question108 question)
    {
        var words = question.RandomQuestions;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < cards.Length)
            {
                cards[i].Init(words[i]);
                cards[i].turnner.SetBack();
                AddOnClickCardListener(cards[i]);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
        StartCoroutine(TurrningCards());
    }
    private void AddOnClickCardListener(Card_108 card)
    {
        card.onClick += (value) =>
        {
            if (currentQuestion.correct[currentQuestion.currentIndex] == value)
            {
                AddAnswer(value);
            }
        };
        card.checkVaild += (value) =>
        {
            var vaild = value == currentQuestion.correct[currentQuestion.currentIndex];
            audioPlayer.Play(GameManager.Instance.GetClipWord(value));
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
        audioPlayer.Play(GameManager.Instance.GetClipWord(currentQuestion.correct[currentQuestion.currentIndex]));
    }
}
public class Question108 : MultiQuestion<string>
{
    public int currentIndex;
    public Question108(string[] correct, string[] questions) : base(correct, questions) 
    {
        currentIndex = 0;
    }

    protected override bool CheckCorrect(string answer)
    {
        return correct[currentIndex] == answer;
    }

    public override void SetAnswer(string answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }

}
