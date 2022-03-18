using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_104 : BaseContents
{
    public DrawAlphabet drawAlphabet;
    public AudioSinglePlayer audioPlayer;
    public GameObject mask;
    public CanvasScaler scaler;
    protected override eContents contents => eContents.JT_PL1_104;
    protected override int GetTotalScore() => 1;

    protected override void Awake()
    {
        base.Awake();
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        drawAlphabet.onCompleted += ShowResult;
        drawAlphabet.Init(GameManager.Instance.currentAlphabet, eAlphbetType.Upper);
    }

    protected override bool CheckOver() => true;
    protected override void ShowResult()
    {
        mask.gameObject.SetActive(false);
        audioPlayer.Play(GameManager.Instance.GetClipAct1(), base.ShowResult);
    }

    private void OnDisable()
    {
        drawAlphabet.onCompleted -= ShowResult;
    }
}
