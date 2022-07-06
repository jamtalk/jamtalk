using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_102 : MultiAnswerContents<Question3_102, DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL3_102;
    protected override int QuestionCount => 3;
    private int answerCount = 6;
    private eDigraphs[] digraphs = { eDigraphs.CH, eDigraphs.SH, eDigraphs.TH };

    public Sprite backImage;
    public Image spatulaImage;
    public DoubleClick302[] pancakes;

    protected override List<Question3_102> MakeQuestion()
    {
        var questions = new List<Question3_102>();

        for ( int i = 0; i < QuestionCount; i++)
        {
            var current = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.type == digraphs[i])
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(answerCount)
                .ToArray();
            questions.Add(new Question3_102(current, new DigraphsSource[] { }));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_102 question)
    {
        for(int i = 0; i < pancakes.Length; i++)
        {
            pancakes[i].isCheck = false;
            pancakes[i].isOn = false;
            pancakes[i].images.gameObject.SetActive(false);
            pancakes[i].textPhanix.gameObject.SetActive(true);

            pancakes[i].Init(question.totalQuestion[i]);
            AddListener(pancakes[i]);
        }
    }

    private void AddListener(DoubleClick302 button)
    {
        button.onClick.RemoveAllListeners();
        button.onClickFirst.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            button.data.PlayAct();
        });

        button.onClick.AddListener(() =>
        {
            if (!button.isCheck)
            {
                button.isCheck = true;

                button.data.PlayClip();
                button.image.sprite = backImage;
                button.textPhanix.gameObject.SetActive(false);
                button.images.gameObject.SetActive(true);

                AddAnswer(button.data);
            }
        });
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