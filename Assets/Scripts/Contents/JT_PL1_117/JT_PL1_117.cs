using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class JT_PL1_117 : BaseContents
{
    public BingoBoard board;
    public BingoScoreBoard scoreBoard;
    public int BingoCount=>1;
    public RectTransform bingo;
    public AudioSinglePlayer audioPlayer;
    //¹®Á¦
    public int currentIndex = 0;
    public eAlphabet[] questions;
    public eAlphabet currentQuestion => questions[currentIndex];
    protected override eContents contents => eContents.JT_PL1_117;
    protected override bool CheckOver() => board.GetBingoCount() >= BingoCount;
    protected override int GetTotalScore() => BingoCount;
    protected override void Awake()
    {
        base.Awake();
        board.onClick += OnClick;
        scoreBoard.onFailed += ShowResult;

        questions = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x=> Random.Range(0f,100f))
            .ToArray();
        currentIndex = 0;
        PlaySound();
        board.Init(questions
            .OrderBy(x=>Random.Range(0f,100f)).ToArray());
    }
    protected override eGameResult GetResult()
    {
        if (currentIndex == 0)
            return eGameResult.Fail;
        else if (board.GetBingoCount() < BingoCount)
            return eGameResult.Greate;
        else
            return eGameResult.Perfect;
    }
    protected override void ShowResult()
    {
        bingo.gameObject.SetActive(true);
        
        var tween = bingo.DOScale(1.5f, .5f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(2, LoopType.Yoyo);
        tween.onComplete += () => base.ShowResult();
        tween.Play();
    }
    public void OnClick(eAlphabet value)
    {
        if (value == currentQuestion)
        {
            currentIndex += 1;
            scoreBoard.AddScore(100);
        }
        else
            scoreBoard.IncreaseIncorrect();

        PlaySound();

    }
    public void PlaySound()
    {
        if (CheckOver())
            ShowResult();
        else
            audioPlayer.Play(GameManager.Instance.GetClipAlphbet(currentQuestion));
    }
}
