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
    private DigraphsWordsData[] current;

    public Image successEffect;
    public Text successText;
    public Image successImage;
    public Sprite successedImage;
    public ImageButton[] buttons;
    public Image[] childrenImages;

    protected override List<Question4_102> MakeQuestion()
    {
        var questions = new List<Question4_102>();


        for( int i = 0; i < QuestionCount; i++)
        {
            current = GameManager.Instance.digrpahs
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
        for(int i = 0; i < buttons.Length; i ++)
        {
            var data = question.totalQuestion[i];
            buttons[i].sprite = data.sprite;
            AddListener(buttons[i], data);
        }
    }

    private void AddListener(ImageButton imageButton, DigraphsWordsData data)
    {
        var button = imageButton.button;
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            audioPlayer.Play(data.clip);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].gameObject.SetActive(false);

            successImage.sprite = data.sprite;
            successImage.preserveAspect = true;
            successText.text = data.key.Replace(data.IncludedDigraphs,
                    "<color=\"red\">" + data.IncludedDigraphs + "</color>");

            successEffect.gameObject.SetActive(true);
            audioPlayer.Play(data.act, () =>
            {
                successEffect.gameObject.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].gameObject.SetActive(true);
                AddAnswer(currentQuestion.currentCorrect);
            });

            button.image.sprite = successedImage;
        });
    }

    private void SetCurrentColor(DigraphsWordsData data)
    {
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