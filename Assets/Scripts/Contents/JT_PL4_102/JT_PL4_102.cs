using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_102 : MultiAnswerContents<Question4_102, DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL4_102;
    protected override int QuestionCount => 1;
    private int answerCount = 6;

    public Image successEffect;
    public Text successText;
    public Image successImage;
    public Sprite successedImage;
    public Image[] parentImages;
    public Image[] childrenImages;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override List<Question4_102> MakeQuestion()
    {
        var questions = new List<Question4_102>();


        for( int i = 0; i < QuestionCount; i++)
        {
            var current = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.type == GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(answerCount)
                .ToArray();

            questions.Add(new Question4_102(current, new DigraphsSource[] { }));
        }
        
        return questions;
    }

    protected override void ShowQuestion(Question4_102 question)
    {
        Debug.Log(question.totalQuestion.Length);
        for(int i = 0; i < childrenImages.Length; i ++)
        {
            var data = question.totalQuestion[i];
            childrenImages[i].sprite = data.sprite;
            AddListener(parentImages[i], data);
        }
    }

    private void AddListener(Image button, DigraphsSource data)
    {
        Debug.Log(data.value + " / " + currentQuestion.currentCorrect.value);

        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            data.PlayClip();
            if (data.value == currentQuestion.currentCorrect.value)
            {
                for (int i = 0; i < parentImages.Length; i++)
                    parentImages[i].gameObject.SetActive(false);

                successImage.sprite = data.sprite;
                SetCurrentColor(data);
                successEffect.gameObject.SetActive(true);

                data.PlayAct(() =>
                {
                    successEffect.gameObject.SetActive(false);
                    for (int i = 0; i < parentImages.Length; i++)
                        parentImages[i].gameObject.SetActive(true);
                });

                button.sprite = successedImage;
                AddAnswer(currentQuestion.currentCorrect);
            }
        });
    }

    private void SetCurrentColor(DigraphsSource data)
    {
        var isCheck = data.value.Contains(data.type.ToString().ToLower());
        string value = string.Empty;

        if (!isCheck)
        {
            string temp = string.Empty;
            if (data.type == eDigraphs.OI)
                temp = "oy";
            else if (data.type == eDigraphs.EA)
                temp = "ee";

            value = data.value.Replace(temp,
                "<color=\"red\">" + temp + "</color>");
        }
        else
        {
            value = data.value.Replace(data.type.ToString().ToLower()
                , "<color=\"red\">" + data.type.ToString().ToLower() + "</color>");
        }

        successText.text = value;
    }
}

public class Question4_102 : MultiQuestion<DigraphsSource>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsSource currentCorrect => correct[currentIndex];

    public Question4_102(DigraphsSource[] correct, DigraphsSource[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(DigraphsSource answer) => true;
    public override void SetAnswer(DigraphsSource answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}