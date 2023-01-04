using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class STTContents<TData,TValueViewer> : SingleAnswerContents<Question_STT<TData>, TData> 
    where TData:ResourceWordsElement
{
    protected virtual bool PlayClipOnShow => true;
    public TValueViewer valueViewer;
    public Image wordImage;
    public Button[] buttonAudio;
    public Button buttonSTT;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();
    public Text sttResultViewer;
    private Tween buttonTween;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override float GetDuration() => (float)(currentQuestionIndex + 1f) / (float)QuestionCount;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        EndGuidnce();
    }
    protected override void Awake()
    {
        base.Awake();
        currentQuestionIndex = 0;

        for (int i = 0; i < buttonAudio.Length; i++)
        {
            buttonAudio[i].onClick.AddListener(() =>
            {
                audioPlayer.Play(currentQuestion.correct.clip); //, PlayButtonTween);
            });
        }

        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += OnSTTResult;
        recorder.onStopRecord += () =>
        {
            StopButtonTween();
            //recorder.SendSTT();
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

    protected abstract void ShowValue(Question_STT<TData> question);
    protected override void ShowQuestion(Question_STT<TData> question)
    {
        ShowValue(question);
        StopButtonTween();

        wordImage.sprite = question.correct.sprite;
        wordImage.preserveAspect = true;

        if (PlayClipOnShow)
            audioPlayer.Play(question.correct.clip);
    }

    protected virtual void OnSTTResult(bool success, string result)
    {

        Debug.LogFormat("question lenth : {0}\n currentQuestionIndex : {1}", questions.Count, currentQuestionIndex);
        sttResultViewer.text = result;
        //if ("hey".Contains(result))
        if (CheckCorrect(result))
        {
            AddAnswer(currentQuestion.correct);
        }
    }

    protected virtual bool CheckCorrect(string value) => currentQuestion.correct.key.ToLower() == value.ToLower();

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
}
public class Question_STT<TData> : SingleQuestion<TData> where TData : ResourceWordsElement
{
    public Question_STT(TData correct) : base(correct, new TData[] { })
    {
    }
}
