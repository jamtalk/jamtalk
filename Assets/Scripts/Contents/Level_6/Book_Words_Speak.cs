using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Words_Speak : SingleAnswerContents<BookWordsSpeakQuestion, BookWordData>
{
    public override eSceneName NextScene => eSceneName.AC_004;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            eventSystem.enabled = true;
            AddAnswer(currentQuestion.correct);
        }
    }
    protected override List<BookWordsSpeakQuestion> MakeQuestion()
    {
        return GameManager.Instance.GetCurrentBook().SelectMany(x=>x.words).Distinct()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new BookWordsSpeakQuestion(x, new BookWordData[0]))
            .ToList();
    }

    public void OnSTT(string value)
    {
        if(value == currentQuestion.correct.value)
        {
            AddAnswer(currentQuestion.correct);
        }
    }

    protected override void ShowQuestion(BookWordsSpeakQuestion question)
    {
        button.image.sprite = question.correct.sprite;
        button.image.preserveAspect = true;
        text.text = question.correct.value;
        PlayCorrect(question);
    }

    private void PlayCorrect(BookWordsSpeakQuestion question)
    {
        AndroidPluginManager.Instance.PlayTTS(question.correct.value);
    }
}
public class BookWordsSpeakQuestion : SingleQuestion<BookWordData>
{
    public BookWordsSpeakQuestion(BookWordData correct, BookWordData[] questions) : base(correct, questions)
    {
    }
}
