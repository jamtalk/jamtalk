using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_102 : MultiAnswerContents<Question3_102, DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL3_102;
    protected override int QuestionCount => 1;
    private int answerCount = 6;

    public Image spatulaImage;
    public Text[] texts;
    public Image[] images;

    protected override List<Question3_102> MakeQuestion()
    {
        var questions = new List<Question3_102>();

        for ( int i = 0; i < QuestionCount; i++)
        {
            var current = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.type == GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(answerCount)
                .ToArray();
            questions.Add(new Question3_102(current, new DigraphsSource[] { }));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_102 question)
    {
        for (int i = 0; i < images.Length; i++)
        {
            var data = question.totalQuestion[i];
            images[i].sprite = data.sprite;
            texts[i].text = data.value;
        }
    }

    private void AddListener()
    {

    }
}


public class Question3_102 : MultiQuestion<DigraphsSource>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsSource currentCorrect => correct[currentIndex];

    public Question3_102(DigraphsSource[] correct, DigraphsSource[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(DigraphsSource answer) => true;
    public override void SetAnswer(DigraphsSource answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}