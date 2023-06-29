using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;

public class JT_PL1_103 : BaseContents<AlphabetContentsSetting>
{
    public Image currentImage;
    public Button button;
    public Button buttonSTT;
    public Image sttButtonBG;
    public Text valueText;
    private Tween buttonTween;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();
    protected override eContents contents => eContents.JT_PL1_105;
    private eAlphabet question => GameManager.Instance.currentAlphabet;
    private eAlphabet value;
    //protected override bool isGuidence => false;

    protected override bool CheckOver() => index >= GetTotalScore();
    protected override int GetTotalScore() => Enum.GetNames(typeof(eAlphabet)).ToList().Count;
    protected int index = 0;
    private eAlphabetType capsLock = eAlphabetType.Upper;
    private System.Action loading;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        currentImage.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, capsLock, value);
        PlayAudio();
        yield return base.ShowGuidnceRoutine();
        while (audioPlayer.length == 0) { yield return null; }
        yield return new WaitForSeconds(audioPlayer.length);
        guideFinger.gameObject.SetActive(true);
        bool isOVer = false;
        guideFinger.DoMove(buttonSTT.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                PlayButtonTween();
                guideFinger.gameObject.SetActive(false);
                PlayAudio();
                isOVer = true;
            });
        });
        while (!isOVer) { yield return null; }
        Debug.LogFormat("클릭 완료, 대기 {0}초", audioPlayer.length + 1f);
        yield return new WaitForSeconds(audioPlayer.length + 1f);
        isOVer = false;
        guideFinger.gameObject.SetActive(true);
        guideFinger.DoClick(() => isOVer = true);
        while (!isOVer) { yield return null; }
        StopButtonTween();
        EndGuidnce();
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        ShowQuestion(0f);
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        value = question;
        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += OnSTTResult;
        //recorder.onDecibelResult += AddAnswer;
        recorder.onStopRecord += () =>
        {
            StopButtonTween();
            sttButtonBG.gameObject.SetActive(false);
            loading = PopupManager.Instance.ShowLoading();
            //AddAnswer(true);
        };
        button.onClick.AddListener(() => PlayAudio());
    }


    protected virtual void OnSTTResult(bool success, string result)
    {
        AddAnswer(success);
        loading?.Invoke();
    }



    private void ShowQuestion(float delay = 3f)
    {
        currentImage.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, capsLock, value);
        currentImage.preserveAspect = true;
        buttonSTT.interactable = false;
        StartCoroutine(ShowQuestionAction(delay));
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
      

    private void AddAnswer(bool success)
    {
        if (!success)
            Debug.Log("Decibel Recognition Failed !");
        else
        {
            if (success)
            {
                CorrectAction();

                audioPlayer.Play(GameManager.Instance.GetResources(value).AudioData.act1, () =>
                {
                    if (capsLock == eAlphabetType.Upper)
                        capsLock = eAlphabetType.Lower;
                    else
                    {
                        capsLock = eAlphabetType.Upper;
                        value++;
                        index++;
                    }

                    if (CheckOver())
                        ShowResult();
                    else
                        ShowQuestion();
                });
            }
            else
                ShowQuestion();
        }
    }


    private void PlayAudio()
    {
        StopButtonTween();

        var clipValue = capsLock == eAlphabetType.Upper ?
            GameManager.Instance.GetResources(value).AudioData.clip :
            GameManager.Instance.GetResources(value).AudioData.phanics;

        audioPlayer.Play(clipValue, () => buttonSTT.interactable = true); 
    }

    private IEnumerator ShowQuestionAction(float value = 1f)
    {
        yield return new WaitForSecondsRealtime(value);

        ShowQeustionAction();
        PlayAudio();
    }

    private void PlayButtonTween()
    {
        StopButtonTween();

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
            buttonSTT.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
