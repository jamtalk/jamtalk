using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookConversationContents : BaseContents<BookContentsSetting>
{
    protected override eContents contents => eContents.Book_Conversation;
    protected override bool isGuidence => false;
    protected int QuestionCount => 3;
    public Button buttonPlay;
    public Button buttonReplay;
    public Image imagePanel;
    public BookConversationElementManager mgr;
    private int currentIndex = 0;
    private BookConversationData CurrentData => datas[currentIndex];
    private BookConversationData[] datas;

    public Button buttonSTT;
    private Tween buttonTween;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();

    protected override bool includeExitButton => false;

    protected override void OnAwake()
    {
        base.OnAwake();
        datas = GameManager.Instance.GetCurrentBook().conversations.OrderBy(x => x.priority).ToArray();
        ShowQuestion(0);
        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += (success,value) =>
        {
            if (success)
                AddAnswer(value);
        };
        buttonPlay.onClick.AddListener(PlayAudio);
        buttonReplay.onClick.AddListener(PlayAudio);
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            AddAnswer(CurrentData.value);
#endif
    }
    private void PlayAudio()
    {
        audioPlayer.Play(CurrentData.value);
    }
    private void AddAnswer(string value)
    {
        if (value == CurrentData.value)
        {
            if (++currentIndex < QuestionCount)
                ShowQuestion(currentIndex);
            else
                ShowResult();
        }
    }
    private void ShowQuestion(int index)
    {
        currentIndex = index;
        var obj = mgr.Create(CurrentData);
        obj.gameObject.SetActive(true);
        imagePanel.sprite = CurrentData.sprite;
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
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }

        buttonSTT.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }
    protected override bool CheckOver() => currentIndex >= QuestionCount;

    protected override int GetTotalScore() => QuestionCount;
}
