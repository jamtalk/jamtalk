using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL1_113 : SingleAnswerContents<Question113, eAlphabet>
{
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_113;
    public Charactor113[] charactors;
    public Item113[] items;
    public Sprite[] spritesProduct;
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0;i < charactors.Length; i++)
        {
            AddCharactorListener(charactors[i]);
        }
    }
    private void AddCharactorListener(Charactor113 item)
    {
        item.onAway += () =>
        {
            AddAnswer(currentQuestion.correct);
        };
    }
    protected override List<Question113> MakeQuestion()
    {
        var alphabets = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(QuestionCount)
            .ToArray();

        var list = new List<Question113>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var incorrect = GameManager.Instance.alphabets
                .Where(x => !alphabets.Contains(x))
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(10)
                .ToArray();

            list.Add(new Question113(alphabets[i], incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question113 question)
    {
        items = items.
            OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        var randomQuestion = question.RandomQuestions;
        for(int i = 0;i < items.Length; i++)
        {
            eAlphabet? value = null;
            if (i < randomQuestion.Length)
                value = randomQuestion[i];
            items[i].Init(value, spritesProduct.OrderBy(x => Random.Range(0f, 100f)).First());
        }
        var ch = charactors[Random.Range(0, charactors.Length)];
        ch.Init(question.correct);
        ch.Call();
    }
}
public class Question113 : SingleQuestion<eAlphabet>
{
    public Question113(eAlphabet correct, eAlphabet[] questions) : base(correct, questions)
    {
    }
}
