using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Book_Words_Quiz : SingleAnswerContents<BookWordQuizeQuestion, BookWordData>
{
    public override eSceneName NextScene => eSceneName.AC_004;
    public EventSystem eventSystem;
    public Image quizImage;
    public Button[] buttons;
    protected override int QuestionCount => 5;
    protected override bool isGuidence => false;

    protected override eContents contents => eContents.Book_Words_Quiz;

    protected override List<BookWordQuizeQuestion> MakeQuestion()
    {
        var incorrectsBooks = GameManager.Instance.GetIncorrectBookWords();
        return GameManager.Instance.GetCurrentBookWords()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x =>
            {
                var incorrects = incorrectsBooks
                    .Where(y=>y.value != x.value)
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(buttons.Length-1)
                    .ToArray();
                return new BookWordQuizeQuestion(x, incorrects);
            }).ToList();
    }

    protected override void ShowQuestion(BookWordQuizeQuestion question)
    {
        quizImage.sprite = question.correct.sprite;
        quizImage.preserveAspect = true;
        buttons = buttons.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for(int i = 0;i < question.totalQuestion.Length; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = question.totalQuestion[i].value;
            AddListener(buttons[i], question.totalQuestion[i]);
        }
    }
    private void AddListener(Button button, BookWordData data)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            eventSystem.enabled = false;
            AndroidPluginManager.Instance.PlayTTS(data.value, () =>
            {
                if (data == currentQuestion.correct)
                    AddAnswer(data);
                eventSystem.enabled = true;
            });
        });
    }
}
public class BookWordQuizeQuestion : SingleQuestion<BookWordData>
{
    public BookWordQuizeQuestion(BookWordData correct, BookWordData[] questions) : base(correct, questions)
    {
    }
}
