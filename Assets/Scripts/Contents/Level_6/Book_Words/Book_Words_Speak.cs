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

    public Button buttonSTT;
    private Tween buttonTween;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();
    protected override int QuestionCount => 5;
    protected override bool includeExitButton => false;
    protected override bool isGuidence => false;
    protected override bool showPopupOnEnd => false;
    protected override bool showQuestionOnAwake => false;
    protected override eContents contents => eContents.Book_Words;

    protected override void OnAwake()
    {
        isGuide = false;
        base.OnAwake();
        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += (success, value) =>
        {
            PopupManager.Instance.Close();
            if (success)
                AddAnswer(currentQuestion.correct);
        };
    }
    private void RecordAction()
    {
        recorder.RecordOrSendSTT();
        var isRecord = Microphone.IsRecording(recorder.deviceName);
        if (isRecord)
            PlayButtonTween();
        else
            StopButtonTween();
    }

    private void PlayButtonTween()
    {
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        buttonTween = buttonSTT.GetComponent<RectTransform>().DOScale(1.5f, 1f);
        buttonTween.SetEase(Ease.Linear);
        buttonTween.SetLoops(-1, LoopType.Yoyo);
        buttonTween.onKill += () => buttonSTT.GetComponent<RectTransform>().localScale = Vector3.one;
        buttonTween.Play();
    }
    private void StopButtonTween()
    {
        PopupManager.Instance.ShowLoading();
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }

        buttonSTT.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
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
