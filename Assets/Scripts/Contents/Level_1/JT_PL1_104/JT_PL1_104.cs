using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL1_104 : BaseContents
{
    public DrawAlphabet drawAlphabet;
    public GameObject mask;
    public CanvasScaler scaler;
    protected override eContents contents => eContents.JT_PL1_104;
    protected override int GetTotalScore() => 1;

    protected eAlphabet[] targets;
    protected int currentIndex = 0;

    protected override void Awake()
    {
        targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        base.Awake();
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        drawAlphabet.onCompleted += ()=>
        {
            mask.gameObject.SetActive(false);
            var tween = drawAlphabet.transform.DOScale(1.5f, .5f);
            tween.SetLoops(2, LoopType.Yoyo);
            audioPlayer.Play(GameManager.Instance.GetResources(targets[currentIndex]).AudioData.act2, () =>
            {
                currentIndex += 1;
                if (currentIndex < targets.Length)
                    drawAlphabet.Init(targets[currentIndex], eAlphabetType.Upper);
                else
                    ShowResult();
            });
        };
        drawAlphabet.Init(GameManager.Instance.currentAlphabet, eAlphabetType.Upper);
    }

    protected override bool CheckOver() => true;
    private void OnDisable()
    {
        drawAlphabet.onCompleted -= ShowResult;
    }
}
