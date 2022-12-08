using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class JT_PL1_104 : BaseContents
{
    public DrawAlphabet drawAlphabet;
    public GameObject mask;
    public SkeletonGraphic aniChar;
    public CanvasScaler scaler;
    public eAlphabetType type;
    protected override eContents contents => eContents.JT_PL1_104;
    protected override int GetTotalScore() => 1;
    protected override bool CheckOver() => questionsCount == currentIndex && type == eAlphabetType.Lower;

    protected eAlphabet[] targets;
    protected int currentIndex = 0;
    protected int questionsCount => 2;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        EndGuidnce();
    }

    protected override void Awake()
    {
        targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        base.Awake();
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        drawAlphabet.onCompleted += ()=>
        {
            CorrectAction();

            mask.gameObject.SetActive(false);
            var tween = drawAlphabet.transform.DOScale(1.5f, .5f);
            tween.SetLoops(2, LoopType.Yoyo);
            audioPlayer.Play(GameManager.Instance.GetResources(targets[currentIndex]).AudioData.act2, () =>
            {
                currentIndex += 1;
                if (CheckOver())
                    ShowResult();
                else
                {
                    ShowQeustionAction();

                    if (questionsCount == currentIndex)
                    {
                        type = eAlphabetType.Lower;
                        currentIndex = 0;
                    }
                    drawAlphabet.Init(targets[currentIndex], type);
                }
            });
        };
        drawAlphabet.Init(GameManager.Instance.currentAlphabet, type);
    }

    private void OnDisable()
    {
        drawAlphabet.onCompleted -= ShowResult;
    }
}
