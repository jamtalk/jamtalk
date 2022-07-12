using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL1_103 : BaseContents
{
    public Image image;
    public Button button;
    public STTButton buttonSTT;
    public Text valueText;
    private Tween buttonTween;

    protected override eContents contents => eContents.JT_PL1_105;
    private eAlphabet value;
    private eAlphabet question => GameManager.Instance.currentAlphabet;

    protected override bool CheckOver() => true;
    protected override int GetTotalScore() => 1;
    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(PlayAudio);
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, question);
        image.preserveAspect = true;
        buttonSTT.onRecord += PlayButtonTween;
        buttonSTT.onSTT += OnSTTResult;
        PlayAudio();
    }
    private void OnDisable()
    {
        buttonSTT.onRecord -= PlayButtonTween;
        buttonSTT.onSTT -= OnSTTResult;
    }
    private void PlayButtonTween(bool activate)
    {
        if(buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        if (!activate)
        {
            buttonTween = buttonSTT.GetComponent<RectTransform>().DOScale(1.5f, 1f);
            buttonTween.SetEase(Ease.Linear);
            buttonTween.SetLoops(-1, LoopType.Yoyo);
            buttonTween.onKill += () => buttonSTT.GetComponent<RectTransform>().localScale = Vector3.one;
            buttonTween.Play();
        }
    }
    private void OnSTTStarted()
    {
        audioPlayer.Stop();
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
    }
    private void PlayAudio()
    {
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        audioPlayer.Play(GameManager.Instance.GetResources(question).AudioData.phanics,()=>PlayButtonTween(false));
    }
    
    private void OnSTTResult(string result)
    {
        valueText.text = result;
        if (question.ToString().ToLower() == result.ToLower())
            audioPlayer.Play(GameManager.Instance.GetResources(question).AudioData.act2, ShowResult);
    }
    private void OnSTTError(string message)
    {
        Debug.LogError(message);
    }
}
