using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_103 : BaseContents
{
    public Image image;
    public Button button;
    public AudioSinglePlayer audioPlayer;

    protected override eContents contents => eContents.JT_PL1_105;
    private eAlphabet value;
    private eAlphabet question => GameManager.Instance.currentAlphabet;

    protected override bool CheckOver() => true;
    private void Awake()
    {
        button.onClick.AddListener(PlayAudio);
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.FullColor, eAlphbetType.Upper, question);
        image.preserveAspect = true;
        PlayAudio();
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
    private void PlayAudio()
    {
        audioPlayer.Play(GameManager.Instance.GetClipPhanics(question));
    }
    private void OnSTTResult(string result)
    {
        if (question.ToString().ToLower() == result.ToLower())
            audioPlayer.Play(1f, GameManager.Instance.GetClipAct1(question), ShowResult);
    }
    private void OnSTTError(string message)
    {
        Debug.LogError(message);
    }
}
