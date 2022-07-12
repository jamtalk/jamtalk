using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_105 : BaseContents
{
    public AlphabetImage105 alphabetImage;
    public Image image;
    public Button[] buttonAudio;
    public STTButton buttonSTT;
    public Text value;
    private Tween buttonTween;

    private int questionCount => 4;
    private int currentIndex = 0;
    private WordSource[] questions;
    private WordSource currentQuestion => questions[currentIndex];
    protected override eContents contents => eContents.JT_PL1_105;
    protected override bool CheckOver() => currentIndex == questionCount;
    protected override int GetTotalScore() => questionCount;
    protected override float GetDuration() => (float)(currentIndex + 1f) / (float)questionCount;
    protected override void Awake()
    {
        base.Awake();
        currentIndex = 0;
        questions = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();

        for (int i = 0; i < buttonAudio.Length; i++)
            buttonAudio[i].onClick.AddListener(() =>
            {
                audioPlayer.Play(currentQuestion.clip, PlayButtonTween);
            });
        
        buttonSTT.onSTT += OnSTTResult;
        buttonSTT.onRecord += STTButtonAnimating;
        ShowQuestion();
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
    private void ShowQuestion()
    {
        alphabetImage.Init(currentQuestion.alphabet);
        image.sprite = currentQuestion.sprite;

        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        audioPlayer.Play(currentQuestion.clip, PlayButtonTween);
        image.preserveAspect = true;
    }
    private void OnSTTResult(string result)
    {
        value.text = result;
        if (currentQuestion.value.ToLower() == result.ToLower())
        {
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                currentIndex += 1;
                if (CheckOver())
                    ShowResult();
                else
                    ShowQuestion();
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