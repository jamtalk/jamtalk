using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL2_111 : BaseContents<AlphabetContentsSetting>
{
    [Header("UI")]
    public Image imageValue;
    public Button buttonMonitor;
    public Text textValue;
    public Button buttonRecord;
    public Button buttonPlay;
    public Button buttonPause;
    public Button buttonStop;
    public Button buttonNext;
    [Header("Record Button Sprites")]
    public Sprite spriteRecordOn;
    public Sprite spriteRecordOff;
    [Header("Recorder")]
    public VoiceRecorder recorder;
    [Header("Lines")]
    public GameObject lineSuccess;
    public GameObject lineFail;

    private int QuestionCount => 3;
    private int currentIndex = 0;
    private VowelWordsData[] questions;
    private VowelWordsData CurrentQuestion => questions[currentIndex];
    protected override eContents contents => eContents.JT_PL2_111;

    protected override bool CheckOver() => currentIndex == QuestionCount;

    protected override int GetTotalScore() => QuestionCount;
    protected override bool isGuidence => false;

    protected override void OnAwake()
    {
        base.OnAwake();

        //recorder.OnRecord += (value) =>
        //{
        //    buttonRecord.image.sprite = value ? spriteRecordOn : spriteRecordOff;
        //    AllButtonsIntractable(!value);
        //};
        STTManager.Instance.onEnded += OnEndRecording;
        STTManager.Instance.onResult += OnEndRecording;

        buttonPlay.onClick.AddListener(recorder.source.Play);
        buttonPause.onClick.AddListener(recorder.source.Pause);
        buttonStop.onClick.AddListener(recorder.source.Stop);
        buttonNext.onClick.AddListener(ShowNext);
        buttonMonitor.onClick.AddListener(() =>
        {
            AllButtonsIntractable(false);
            audioPlayer.Play(CurrentQuestion.clip, () => AllButtonsIntractable(true));
        });
        buttonRecord.onClick.AddListener(StartRecording);

        questions = MakeQuestion();
        currentIndex = 0;
        ShowQuestion(CurrentQuestion);
    }
    private void OnDisable()
    {
        STTManager.Instance.onEnded -= OnEndRecording;
        STTManager.Instance.onResult -= OnEndRecording;
    }
    private void ShowQuestion(VowelWordsData data)
    {
        ShowResult(eResult.None);
        imageValue.sprite = data.sprite;
        imageValue.preserveAspect = true;
        textValue.text = data.key;
        AllButtonsIntractable(false);
        audioPlayer.Play(data.clip, StartRecording);
    }
    private void ShowNext()
    {
        currentIndex += 1;
        if (CheckOver())
            ShowResult();
        else
            ShowQuestion(CurrentQuestion);
    }
    private VowelWordsData[] MakeQuestion()
    {
        return GameManager.Instance.GetResources().Vowels
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
    }
    private void StartRecording()
    {
        AllButtonsIntractable(false);
        recorder.Record();
        STTManager.Instance.StartSTT("en-US");
    }
    private void OnEndRecording(string value)
    {
        var result = value == CurrentQuestion.key ? eResult.Success : eResult.Fail;

    }
    private void OnEndRecording()
    {
        recorder.Stop();
    }
    private void AllButtonsIntractable(bool value)
    {
        buttonMonitor.interactable = value;
        buttonRecord.interactable = value;
        buttonPlay.interactable = value;
        buttonPause.interactable = value;
        buttonStop.interactable = value;
    }
    protected void ShowResult(eResult result)
    {
        lineFail.SetActive(false);
        lineSuccess.SetActive(false);
        buttonNext.interactable = false;
        switch (result)
        {
            case eResult.Fail:
                lineFail.SetActive(true);
                break;
            case eResult.Success:
                lineSuccess.SetActive(true);
                buttonRecord.interactable = false;
                buttonNext.interactable = true;
                break;
        }
    }
    protected enum eResult
    {
        None,
        Fail,
        Success
    }
}
