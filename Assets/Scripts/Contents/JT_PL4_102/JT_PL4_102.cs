using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_102 : MultiAnswerContents<Question4_102, DigraphsWordsData>
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

    protected override List<Question4_102> MakeQuestion()
    {
        var questions = new List<Question4_102>();


        for( int i = 0; i < QuestionCount; i++)
        {
            var current = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(answerCount)
                .ToArray();

            questions.Add(new Question4_102(current, new DigraphsWordsData[] { }));
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

    private void AddListener(Image button, DigraphsWordsData data)
    {
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            audioPlayer.Play(data.clip);
            if (data.key == currentQuestion.currentCorrect.key)
            {
                for (int i = 0; i < parentImages.Length; i++)
                    parentImages[i].gameObject.SetActive(false);

                successImage.sprite = data.sprite;
                SetCurrentColor(data);
                successEffect.gameObject.SetActive(true);
                audioPlayer.Play(data.act, () =>
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

    private void SetCurrentColor(DigraphsWordsData data)
    {
        var isCheck = data.key.Contains(data.Digraphs.ToString().ToLower());
        string value = string.Empty;

        if (!isCheck)
        {
            string temp = string.Empty;
            if (data.Digraphs == eDigraphs.OI)
                temp = "oy";
            else if (data.Digraphs == eDigraphs.EA)
                temp = "ee";
            else if (data.Digraphs == eDigraphs.AI)
                temp = "ay";

            value = data.key.Replace(temp,
                "<color=\"red\">" + temp + "</color>");
        }
        else
        {
            value = data.key.Replace(data.Digraphs.ToString().ToLower()
                , "<color=\"red\">" + data.Digraphs.ToString().ToLower() + "</color>");
        }

        successText.text = value;
    }
}

public class Question4_102 : MultiQuestion<DigraphsWordsData>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsWordsData currentCorrect => correct[currentIndex];

    public Question4_102(DigraphsWordsData[] correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(DigraphsWordsData answer) => true;
    public override void SetAnswer(DigraphsWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}