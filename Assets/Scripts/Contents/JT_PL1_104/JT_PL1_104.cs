using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_PL1_104 : BaseContents
{
    public DrawAlphabet drawAlphabet;
    public AudioSinglePlayer audioPlayer;
    public GameObject mask;
    protected override eContents contents => eContents.JT_PL1_104;

    public void Awake()
    {
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
