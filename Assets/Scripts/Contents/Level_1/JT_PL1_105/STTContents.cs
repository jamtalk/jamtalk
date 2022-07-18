using DG.Tweening;
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
    public STTButton buttonSTT;
    public Text sttResultViewer;
    private Tween buttonTween;
    protected override bool CheckOver() => currentQuestionIndex == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected override float GetDuration() => (float)(currentQuestionIndex + 1f) / (float)QuestionCount;
    protected override void Awake()
    {
        base.Awake();
        currentQuestionIndex = 0;

        for (int i = 0; i < buttonAudio.Length; i++)
        {
            buttonAudio[i].onClick.AddListener(() =>
            {
                audioPlayer.Play(currentQuestion.correct.clip, PlayButtonTween);
            });
        }

        buttonSTT.onSTT += OnSTTResult;
        buttonSTT.onRecord += STTButtonAnimating;
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
    private void OnDisable()
    {
        buttonSTT.onSTT -= OnSTTResult;
        buttonSTT.onRecord -= STTButtonAnimating;
    }
    protected override void ShowQuestion(Question_STT<TData> question)
    {
        ShowValue(question);
        wordImage.sprite = question.correct.sprite;

        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        if(PlayClipOnShow)
            audioPlayer.Play(question.correct.clip, PlayButtonTween);
        wordImage.preserveAspect = true;
    }
    protected abstract void ShowValue(Question_STT<TData> question);
    private void OnSTTResult(string result)
    {
        sttResultViewer.text = result;
        if (currentQuestion.correct.key.ToLower() == result.ToLower())
        {
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                currentQuestionIndex += 1;
                if (CheckOver())
                    ShowResult();
                else
                    AddAnswer(currentQuestion.correct);
            });
        }
    }
    private void STTButtonAnimating(bool activate)
    {
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        if (!activate)
            PlayButtonTween();
    }
    private void OnSTTError(string message)
    {
        Debug.LogError(message);
    }
}
public class Question_STT<TData> : SingleQuestion<TData> where TData : ResourceWordsElement
{
    public Question_STT(TData correct) : base(correct, new TData[] { })
    {
    }
}
