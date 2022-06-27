using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_107 : SingleAnswerContents<Question4_107, DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL4_107;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;

    public Text currentText;
    public Button buttonCharactor;
    public Button[] buttonQuestions;

    [SerializeField]
    private GameObject charactor;
    private DigraphsSource[] current;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override List<Question4_107> MakeQuestion()
    {
        var questions = new List<Question4_107>();

        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        for (int i = 0; i < QuestionCount; i++)
        {
            var temp = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.type != GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(1)
                .ToArray();

            questions.Add(new Question4_107(current[i], temp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question4_107 question)
    {
        var list = new List<DigraphsSource>();
        for (int i = 0; i < buttonQuestions.Length; i++)
            list.Add(question.totalQuestion[i]);
        list.Add(question.correct);

        var data = list
            .OrderBy(x => Random.Range(0, list.Count))
            .ToArray();

        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].name = data[i].value;
            buttonQuestions[i].image.sprite = data[i].sprite;
            AddListener(buttonQuestions[i], question.correct);
        }

        currentText.text = question.correct.value;
    }

    private void AddListener(Button button, DigraphsSource data)
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            if (button.name.Contains(data.value))
            {
                data.PlayClip(() =>
                {
                    if (CheckOver())
                        ShowResult();
                });
            }
        });
    }

    private void DoMove()
    {

    }
}

public class Question4_107 : SingleQuestion<DigraphsSource>
{
    private Sprite spriteCorrect;
    private Sprite[] spriteQuestions;
    public Sprite[] SpriteQuestions
    {
        get
        {
            return spriteQuestions.Union(new Sprite[] { spriteCorrect })
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
        }
    }
    public Question4_107(DigraphsSource correct, DigraphsSource[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}