using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL1_103 : BaseContents
{
    public Image currentImage;
    public Button button;
    public Button buttonSTT;
    public Image sttButtonBG;
    public Text valueText;
    private Tween buttonTween;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();
    protected override eContents contents => eContents.JT_PL1_105;
    private eAlphabet value;
    private eAlphabet question => GameManager.Instance.currentAlphabet;

    protected override bool CheckOver() => index >= GetTotalScore();
    protected override int GetTotalScore() => 1;
    protected int index;
    private eAlphabetType capsLock = eAlphabetType.Upper;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        EndGuidnce();
    }

    protected override void Awake()
    {
        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += AddAnswer;

        base.Awake();
        button.onClick.AddListener(PlayAudio);
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        capsLock = index == 0 ? eAlphabetType.Upper : eAlphabetType.Lower;
        currentImage.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, capsLock, question);
        currentImage.preserveAspect = true;
        buttonSTT.interactable = false;
        PlayAudio();
    }

    private void RecordAction()
    {
        recorder.RecordOrSendSTT();
        var isRecord = !sttButtonBG.gameObject.activeSelf;
        sttButtonBG.gameObject.SetActive(isRecord);

        if (isRecord)
            PlayButtonTween();
        else
            StopButtonTween();
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

    private void AddAnswer(bool success, string value)
    {
        if (!success)
            Debug.Log("STT Recognition Failed !");
        else
        {
            var currentValue = index == 0 ? question.ToString().ToUpper() : question.ToString().ToLower();
            var isContains = value == currentValue;
            Debug.LogFormat("{0} == {1}, {2}", currentValue, value, isContains);

            if (isContains)
            {
                index++;
                if (CheckOver())
                    ShowResult();
                else
                    ShowQuestion();
            }
            else
                ShowQuestion();
        }
    }


    private void PlayAudio()
    {
        StopButtonTween();

        var clipValue = capsLock == eAlphabetType.Upper ?
            GameManager.Instance.GetResources(question).AudioData.clip :
            GameManager.Instance.GetResources(question).AudioData.phanics;

        audioPlayer.Play(clipValue, () => buttonSTT.interactable = true);
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
