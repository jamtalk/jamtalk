using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Words_Speak : SingleAnswerContents<BookWordsSpeakQuestion, BookWordData>
{
    public override eSceneName NextScene => eSceneName.Book_Words_Quiz;
    public EventSystem eventSystem;
    public Button button;
    public Text text;
    public STTButton sttButton;
    protected override int QuestionCount => 10;

    protected override eContents contents => eContents.Book_Words_Speak;
    protected override void Awake()
    {
        base.Awake();
        sttButton.onSTT += OnSTT;
        button.onClick.AddListener(() =>
        {
            PlayCorrect(currentQuestion);
        });
    }
    protected override List<BookWordsSpeakQuestion> MakeQuestion()
    {
        return BookData.Instance.bookWords
            .Where(x => x.type == GameManager.Instance.currentBook)
            .Where(x => x.bookNumber == GameManager.Instance.currentBookNumber)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new BookWordsSpeakQuestion(x, new BookWordData[0]))
            .ToList();
    }

    public void OnSTT(string value)
    {
        if(value == currentQuestion.correct.key)
        {
            AddAnswer(currentQuestion.correct);
        }
    }

    protected override void ShowQuestion(BookWordsSpeakQuestion question)
    {
        button.image.sprite = question.correct.sprite;
        button.image.preserveAspect = true;
        text.text = question.correct.key;
        PlayCorrect(question);
    }

    private void PlayCorrect(BookWordsSpeakQuestion question)
    {
        AndroidPluginManager.Instance.PlayTTS(question.correct.key);
    }
}
public class BookWordsSpeakQuestion : SingleQuestion<BookWordData>
{
    public BookWordsSpeakQuestion(BookWordData correct, BookWordData[] questions) : base(correct, questions)
    {
    }
}
