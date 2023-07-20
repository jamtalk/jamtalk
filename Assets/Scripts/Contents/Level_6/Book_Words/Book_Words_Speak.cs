using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Words_Speak : SingleAnswerContents<BookContentsSetting, BookWordsSpeakQuestion, BookWordData>, IBookContentsRunner
{
    public override eSceneName NextScene => eSceneName.AC_004;
    public EventSystem eventSystem;
    public Button button;
    public Image targetImage;
    public Text text;
    public STTButton sttButton;
    protected override int QuestionCount => 5;
    protected override bool includeExitButton => false;
    protected override bool isGuidence => false;
    protected override bool showPopupOnEnd => false;
    protected override bool showQuestionOnAwake => false;
    protected override eContents contents => eContents.Book_Words;
    Tween sttTween;
    protected override void OnAwake()
    {
        isGuide = false;
        base.OnAwake();
        sttButton.onRecord += OnRecord;
        sttButton.onSTT += OnSTT;
        button.onClick.AddListener(() =>
        {
            PlayCorrect(currentQuestion);
        });
    }
    private void OnRecord(bool isRecording)
    {
        if (isRecording)
        {
            sttTween = sttButton.transform.DOScale(1.5f, .5f);
            sttTween.SetEase(Ease.Linear);
            sttTween.SetLoops(-1, LoopType.Yoyo);
            sttTween.onKill += () => sttButton.transform.localScale = Vector3.one;
        }
        else
        {
            sttTween.Kill();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventSystem.enabled = true;
            AddAnswer(currentQuestion.correct);
        }
    }
    protected override List<BookWordsSpeakQuestion> MakeQuestion()
    {
        return GameManager.Instance.GetCurrentBookWords()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new BookWordsSpeakQuestion(x, new BookWordData[0]))
            .ToList();
    }

    public void OnSTT(string value)
    {
        OnRecord(false);
        AddAnswer(currentQuestion.correct);
        //if(value == currentQuestion.correct.value)
        //{
        //    AddAnswer(currentQuestion.correct);
        //}
    }

    protected override void ShowQuestion(BookWordsSpeakQuestion question)
    {
        targetImage.sprite = question.correct.sprite;
        targetImage.preserveAspect = true;
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
        SceneLoadingPopup.SpriteLoader.Add(correct.spriteAsync);
    }
}
