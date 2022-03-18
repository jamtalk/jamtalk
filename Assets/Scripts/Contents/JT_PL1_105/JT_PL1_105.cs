using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_105 : BaseContents
{
    public AlphabetImage105 alphabetImage;
    public Image image;
    public AudioSinglePlayer audioPlayer;

    private int questionCount => 4;
    private int currentIndex = 0;
    private string[] questions;
    private string currentQuestion => questions[currentIndex];
    protected override eContents contents => eContents.JT_PL1_105;
    protected override bool CheckOver() => currentIndex == questionCount;
    protected override int GetTotalScore() => questionCount;
    protected override float GetDuration() => (float)(currentIndex + 1f) / (float)questionCount;
    protected override void Awake()
    {
        base.Awake();
        currentIndex = 0;
        questions = GameManager.Instance.GetWords(GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();

        STTManager.Instance.onResult += OnSTTResult;
        STTManager.Instance.onError += OnSTTError;

        ShowQuestion();
    }
    private void OnDisable()
    {
        STTManager.Instance.onResult -= OnSTTResult;
        STTManager.Instance.onError -= OnSTTError;
        STTManager.Instance.onStarted += OnSTTStarted;
    }
    private void OnSTTStarted()
    {
        audioPlayer.Stop();
    }
    private void ShowQuestion()
    {
        alphabetImage.Init(GameManager.Instance.ParsingAlphabet(currentQuestion));
        image.sprite = GameManager.Instance.GetSpriteWord(currentQuestion);
        audioPlayer.Play(GameManager.Instance.GetClipWord(currentQuestion));
        image.preserveAspect = true;
    }
    private void OnSTTResult(string result)
    {
        if(currentQuestion.ToLower() == result.ToLower())
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
    private void OnSTTError(string message)
    {
        Debug.LogError(message);
    }
}